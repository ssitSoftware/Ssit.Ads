using Google.Android.Gms.Ads;

namespace Ssit.Ads.AdMob.Droid;

internal sealed class FullScreenCallback : FullScreenContentCallback
{
    private readonly Action _onDismiss;
    private readonly Action _onFail;

    internal FullScreenCallback(Action onDismiss, Action onFail)
    {
        _onDismiss = onDismiss;
        _onFail = onFail;
    }

    public override void OnAdDismissedFullScreenContent() => _onDismiss();
    public override void OnAdFailedToShowFullScreenContent(AdError error) => _onFail();
}