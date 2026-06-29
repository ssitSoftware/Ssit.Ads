namespace Ssit.Ads.Core;

public class DummyAdsManager : IAdsManager
{
    public bool IsAdReady => false;
    public Task ShowAd() => Task.CompletedTask;
}