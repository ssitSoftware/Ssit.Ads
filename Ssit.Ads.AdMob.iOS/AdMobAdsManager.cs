using System.Diagnostics.CodeAnalysis;
using AppTrackingTransparency;
using CoreFoundation;
using Ssit.Ads.Core;

namespace Ssit.Ads.AdMob.iOS;

[SuppressMessage("Interoperability", "CA1416")]
public class AdMobAdsManager : IAdsManager
{
    public class Configuration
    {
        public string AdUnitId = "ca-app-pub-3940256099942544/4411468910";
    }

    private GADInterstitialAd _interstitialAd;
    private FullScreenDelegate _currentDelegate;

    public bool IsAdReady => _interstitialAd != null;
    private readonly string _adUnitId;

    public AdMobAdsManager(Configuration config = null)
    {
        _adUnitId = (config ?? new Configuration()).AdUnitId;
        
        DispatchQueue.MainQueue.DispatchAfter(
            new DispatchTime(DispatchTime.Now, TimeSpan.FromSeconds(1)),
            () =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(14, 0))
                {
                    ATTrackingManager.RequestTrackingAuthorization(status =>
                    {
                        switch (status)
                        {
                            case ATTrackingManagerAuthorizationStatus.Authorized:
                                Console.WriteLine("User accepted tracking. Loading tailored ads.");
                                break;
                            default:
                                Console.WriteLine("User denied tracking. Defaulting to generic ads.");
                                break;
                        }
                        Initialize();
                    });
                }
                else
                {
                    Initialize();
                }
            }
        );
    }
    
    private void Initialize()
    {
        GADMobileAds.SharedInstance.Start(null);
        StartPreload();
    }

    public async Task ShowAd()
    {
        if (_interstitialAd == null) return;

        var tcs = new TaskCompletionSource<bool>();
        DispatchQueue.MainQueue.DispatchAsync(() =>
        {
            var rootVc = GetRootViewController();
            if (rootVc == null) { tcs.TrySetResult(false); return; }

            _currentDelegate = new FullScreenDelegate(
                () => tcs.TrySetResult(true),
                () => tcs.TrySetResult(false));
            _interstitialAd.WeakFullScreenContentDelegate = _currentDelegate;
            _interstitialAd.Present(rootVc);
        });

        await tcs.Task;

        _interstitialAd = null;
        _currentDelegate = null;
        StartPreload();
    }

    private void StartPreload()
    {
        _interstitialAd = null;
        GADInterstitialAd.Load(_adUnitId, GADRequest.Request(), (ad, error) =>
        {
            if (error == null) _interstitialAd = ad;
        });
    }

    private static UIViewController GetRootViewController()
    {
        var scene = UIApplication.SharedApplication.ConnectedScenes
                        .OfType<UIWindowScene>()
                        .FirstOrDefault(s => s.ActivationState == UISceneActivationState.ForegroundActive)
                    ?? UIApplication.SharedApplication.ConnectedScenes
                        .OfType<UIWindowScene>()
                        .FirstOrDefault();

        var window = scene?.Windows.FirstOrDefault(w => w.IsKeyWindow)
                     ?? scene?.Windows.FirstOrDefault();
        return window?.RootViewController;
    }
}