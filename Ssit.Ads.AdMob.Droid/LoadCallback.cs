using Google.Android.Gms.Ads;
using Google.Android.Gms.Ads.Interstitial;

namespace Ssit.Ads.AdMob.Droid;

internal class LoadCallback(Action<InterstitialAd> onLoaded, Action onFailedToLoad = null) : InterstitialCallback
{
    public override void OnAdFailedToLoad(LoadAdError p0)
    {
        base.OnAdFailedToLoad(p0);
        onFailedToLoad?.Invoke();
    }

    public override void OnAdLoaded(InterstitialAd interstitialAd)
    {
        base.OnAdLoaded(interstitialAd);
        onLoaded?.Invoke(interstitialAd);
    }
}