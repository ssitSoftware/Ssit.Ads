namespace Ssit.Ads.Core;

public interface IAdsManager
{
    bool IsAdReady { get; }
    Task ShowAd();
}