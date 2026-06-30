using Google.Android.Gms.Ads;
using Google.Android.Gms.Ads.Interstitial;
using Ssit.Ads.Core;

namespace Ssit.Ads.AdMob.Droid;

public class AdMobAdsManager : IAdsManager
{
    public class Configuration
    {
        public string AdUnitId = "ca-app-pub-3940256099942544/1033173712";
    }

    private readonly Activity _activity;
    private readonly string _adUnitId;
    private volatile InterstitialAd _interstitialAd;
    private LoadCallback _pendingLoad; // kept alive until callback fires

    public bool IsAdReady => _interstitialAd != null;

    public AdMobAdsManager(Activity activity, Configuration config = null)
    {
        _adUnitId = (config ?? new Configuration()).AdUnitId;
        _activity = activity;
        _activity.RunOnUiThread(() =>
            MobileAds.Initialize(_activity, new InitListener(StartPreload)));
    }

    public async Task ShowAd()
    {
        if (_interstitialAd == null) return;

        var tcs = new TaskCompletionSource<bool>();
        _activity.RunOnUiThread(() =>
        {
            var ad = _interstitialAd;
            if (ad == null) { tcs.TrySetResult(false); return; }

            ad.FullScreenContentCallback = new FullScreenCallback(
                () => tcs.TrySetResult(true),
                () => tcs.TrySetResult(false));
            ad.Show(_activity);
        });

        await tcs.Task;

        _interstitialAd = null;
        StartPreload();
    }

    private void StartPreload()
    {
        _interstitialAd = null;
        _activity.RunOnUiThread(() =>
        {
            var request = new AdRequest.Builder().Build();
            _pendingLoad = new LoadCallback(
                onLoaded: ad =>
                {
                    _interstitialAd = ad;
                    _pendingLoad = null;
                },
                onFailedToLoad: async () =>
                {
                    _pendingLoad = null;
                    await Task.Delay(TimeSpan.FromSeconds(2));
                    StartPreload();
                });
            InterstitialAd.Load(_activity, _adUnitId, request, _pendingLoad);
        });
    }
}