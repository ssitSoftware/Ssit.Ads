using Google.Android.Gms.Ads.Initialization;

namespace Ssit.Ads.AdMob.Droid;

internal sealed class InitListener : Java.Lang.Object, IOnInitializationCompleteListener
{
    private readonly Action _onComplete;
    internal InitListener(Action onComplete) => _onComplete = onComplete;
    public void OnInitializationComplete(IInitializationStatus p0) => _onComplete();
}