namespace Ssit.Ads.AdMob.iOS;

internal sealed class FullScreenDelegate : GADFullScreenContentDelegate
{
    private readonly Action _onDismiss;
    private readonly Action _onFail;

    internal FullScreenDelegate(Action onDismiss, Action onFail)
    {
        _onDismiss = onDismiss;
        _onFail = onFail;
    }

    public override void DidDismissFullScreenContent(NSObject ad) => _onDismiss();
    public override void DidFailToPresentFullScreenContent(NSObject ad, NSError error) => _onFail();
}