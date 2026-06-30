using Google.Android.Gms.Ads;
using Google.Android.Gms.Ads.Interstitial;

namespace Ssit.Ads.AdMob.Droid;

internal class LoadCallback : InterstitialCallback
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