using Android.Runtime;
using Google.Android.Gms.Ads;
using Google.Android.Gms.Ads.Initialization;
using Google.Android.Gms.Ads.Interstitial;
using Ssit.Ads.Core;

namespace Ssit.Ads.AdMob.Droid;

public class AdMobAdsManager : IAdsManager
{
    private sealed class FullScreenCallback : FullScreenContentCallback
    {
        private readonly Action _onDismiss;
        private readonly Action _onFail;

        public FullScreenCallback(Action onDismiss, Action onFail)
        {
            _onDismiss = onDismiss;
            _onFail = onFail;
        }

        public override void OnAdDismissedFullScreenContent() => _onDismiss();
        public override void OnAdFailedToShowFullScreenContent(AdError error) => _onFail();
    }
    
    private sealed class InitListener : Java.Lang.Object, IOnInitializationCompleteListener
    {
        private readonly Action _onComplete;
        public InitListener(Action onComplete) => _onComplete = onComplete;
        public void OnInitializationComplete(IInitializationStatus p0) => _onComplete();
    }
    
    private abstract class InterstitialCallback : InterstitialAdLoadCallback  
    {  
        [Register("onAdLoaded", "(Lcom/google/android/gms/ads/interstitial/InterstitialAd;)V", "GetOnAdLoadedHandler")]  
        public virtual void OnAdLoaded(InterstitialAd interstitialAd)  
        {  
        }  
     
        private static Delegate cb_onAdLoaded;  
        private static Delegate GetOnAdLoadedHandler()  
        {  
            if (cb_onAdLoaded is null)  
                cb_onAdLoaded = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr>)n_onAdLoaded);  
            return cb_onAdLoaded;  
        }  
        private static void n_onAdLoaded(IntPtr jnienv, IntPtr native__this, IntPtr native_p0)  
        {  
            InterstitialCallback thisobject = GetObject<InterstitialCallback>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);  
            InterstitialAd resultobject = GetObject<InterstitialAd>(native_p0, JniHandleOwnership.DoNotTransfer);  
            thisobject.OnAdLoaded(resultobject);  
        }  
    }
    
    private class LoadCallback : InterstitialCallback
    {
        private readonly Action<InterstitialAd> _onLoaded;
        private readonly Action _onFailedToLoad;

        public LoadCallback(Action<InterstitialAd> onLoaded, Action onFailedToLoad = null)
        {
            _onLoaded = onLoaded;
            _onFailedToLoad = onFailedToLoad;
        }

        public override void OnAdFailedToLoad(LoadAdError p0)
        {
            base.OnAdFailedToLoad(p0);
            _onFailedToLoad?.Invoke();
        }

        public override void OnAdLoaded(InterstitialAd interstitialAd)
        {
            base.OnAdLoaded(interstitialAd);
            _onLoaded?.Invoke(interstitialAd);
        }
    }
    
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