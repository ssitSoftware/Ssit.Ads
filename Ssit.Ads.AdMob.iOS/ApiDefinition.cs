using System;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Ssit.Ads.AdMob.iOS
{
    // -------------------------------------------------------------------------
    // GADRequest - ad request configuration.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    interface GADRequest : INSCopying
    {
        [Static]
        [Export("request")]
        GADRequest Request();

        [NullAllowed]
        [Export("scene", ArgumentSemantic.Weak)]
        UIWindowScene Scene { get; set; }

        [NullAllowed]
        [Export("keywords", ArgumentSemantic.Copy)]
        string[] Keywords { get; set; }

        [NullAllowed]
        [Export("contentURL", ArgumentSemantic.Copy)]
        string ContentURL { get; set; }

        [NullAllowed]
        [Export("neighboringContentURLStrings", ArgumentSemantic.Copy)]
        string[] NeighboringContentURLStrings { get; set; }

        [Export("registerAdNetworkExtras:")]
        void RegisterAdNetworkExtras(NSObject extras);

        [return: NullAllowed]
        [Export("adNetworkExtrasFor:")]
        NSObject AdNetworkExtrasFor(Class aClass);

        [Export("removeAdNetworkExtrasFor:")]
        void RemoveAdNetworkExtrasFor(Class aClass);
    }

    [Protocol]
    interface GADAdNetworkExtras
    {
    }

    // -------------------------------------------------------------------------
    // GADResponseInfo / GADAdNetworkResponseInfo - fill metadata.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    interface GADResponseInfo
    {
        [NullAllowed]
        [Export("responseIdentifier")]
        string ResponseIdentifier { get; }

        [NullAllowed]
        [Export("adNetworkClassName")]
        string AdNetworkClassName { get; }

        [NullAllowed]
        [Export("loadedAdNetworkResponseInfo")]
        GADAdNetworkResponseInfo LoadedAdNetworkResponseInfo { get; }

        [Export("adNetworkInfoArray")]
        GADAdNetworkResponseInfo[] AdNetworkInfoArray { get; }

        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GADAdNetworkResponseInfo
    {
        [NullAllowed]
        [Export("adNetworkClassName")]
        string AdNetworkClassName { get; }

        [NullAllowed]
        [Export("adUnitMapping")]
        NSDictionary AdUnitMapping { get; }

        [NullAllowed]
        [Export("error")]
        NSError Error { get; }

        [Export("latency")]
        double Latency { get; }

        [Export("dictionaryRepresentation")]
        NSDictionary DictionaryRepresentation { get; }
    }

    // -------------------------------------------------------------------------
    // GADErrorConstants - exposes the GADErrorDomain string constant.
    // -------------------------------------------------------------------------

    [Static]
    [Partial]
    interface GADErrorConstants
    {
        [Field("GADErrorDomain", "__Internal")]
        NSString ErrorDomain { get; }
    }

    // -------------------------------------------------------------------------
    // GADMobileAds - singleton SDK entry point.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface GADMobileAds
    {
        [Static]
        [Export("sharedInstance")]
        GADMobileAds SharedInstance { get; }

        [Export("startWithCompletionHandler:")]
        [Async]
        void Start([NullAllowed] Action<GADInitializationStatus> completionHandler);

        [Export("sdkVersion")]
        string SdkVersion { get; }

        [Export("applicationVolume")]
        float ApplicationVolume { get; set; }

        [Export("applicationMuted")]
        bool ApplicationMuted { get; set; }

        [Export("disableSDKCrashReporting")]
        void DisableSDKCrashReporting();

        [Export("disableMediationInitialization")]
        void DisableMediationInitialization();

        [Export("requestConfiguration")]
        GADRequestConfiguration RequestConfiguration { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GADInitializationStatus
    {
        [Export("adapterStatusesByClassName")]
        NSDictionary AdapterStatusesByClassName { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GADAdapterStatus
    {
        [Export("state")]
        GADAdapterInitializationState State { get; }

        [Export("latency")]
        double Latency { get; }
    }

    [BaseType(typeof(NSObject))]
    interface GADRequestConfiguration
    {
        [NullAllowed]
        [Export("testDeviceIdentifiers", ArgumentSemantic.Copy)]
        string[] TestDeviceIdentifiers { get; set; }

        [NullAllowed]
        [Export("maxAdContentRating", ArgumentSemantic.Copy)]
        string MaxAdContentRating { get; set; }

        [Export("tagForChildDirectedTreatment:")]
        void TagForChildDirectedTreatment(bool childDirectedTreatment);

        [Export("tagForUnderAgeOfConsent:")]
        void TagForUnderAgeOfConsent(bool underAgeOfConsent);
    }

    // -------------------------------------------------------------------------
    // GADBannerView - UIView subclass for banner ads.
    // -------------------------------------------------------------------------

    [BaseType(typeof(UIView))]
    interface GADBannerView
    {
        [Export("initWithAdSize:")]
        NativeHandle Constructor(GADAdSize adSize);

        [Export("initWithAdSize:origin:")]
        NativeHandle Constructor(GADAdSize adSize, CGPoint origin);

        [NullAllowed]
        [Export("adUnitID", ArgumentSemantic.Copy)]
        string AdUnitID { get; set; }

        [NullAllowed]
        [Export("rootViewController", ArgumentSemantic.Weak)]
        UIViewController RootViewController { get; set; }

        [Export("adSize")]
        GADAdSize AdSize { get; set; }

        [NullAllowed]
        [Export("delegate", ArgumentSemantic.Weak)]
        NSObject WeakDelegate { get; set; }

        [NullAllowed]
        [Export("responseInfo")]
        GADResponseInfo ResponseInfo { get; }

        [Export("autoloadEnabled")]
        bool AutoloadEnabled { [Bind("isAutoloadEnabled")] get; set; }

        [Export("loadRequest:")]
        void LoadRequest([NullAllowed] GADRequest request);

        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        Action<GADAdValue> PaidEventHandler { get; set; }
    }

    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface GADBannerViewDelegate
    {
        [EventArgs("GADBannerView")]
        [Export("bannerViewDidReceiveAd:")]
        void DidReceiveAd(GADBannerView bannerView);

        [EventArgs("GADBannerViewError")]
        [Export("bannerView:didFailToReceiveAdWithError:")]
        void DidFailToReceiveAd(GADBannerView bannerView, NSError error);

        [EventArgs("GADBannerView")]
        [Export("bannerViewWillPresentScreen:")]
        void WillPresentScreen(GADBannerView bannerView);

        [EventArgs("GADBannerView")]
        [Export("bannerViewWillDismissScreen:")]
        void WillDismissScreen(GADBannerView bannerView);

        [EventArgs("GADBannerView")]
        [Export("bannerViewDidDismissScreen:")]
        void DidDismissScreen(GADBannerView bannerView);

        [EventArgs("GADBannerView")]
        [Export("bannerViewDidRecordImpression:")]
        void DidRecordImpression(GADBannerView bannerView);

        [EventArgs("GADBannerView")]
        [Export("bannerViewDidRecordClick:")]
        void DidRecordClick(GADBannerView bannerView);
    }

    // -------------------------------------------------------------------------
    // GADFullScreenContentDelegate - shared protocol for interstitials/rewarded.
    // -------------------------------------------------------------------------

    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface GADFullScreenContentDelegate
    {
        [EventArgs("GADFullScreenContentError")]
        [Export("ad:didFailToPresentFullScreenContentWithError:")]
        void DidFailToPresentFullScreenContent(NSObject ad, NSError error);

        [EventArgs("GADFullScreenContent")]
        [Export("adWillPresentFullScreenContent:")]
        void WillPresentFullScreenContent(NSObject ad);

        [EventArgs("GADFullScreenContent")]
        [Export("adDidDismissFullScreenContent:")]
        void DidDismissFullScreenContent(NSObject ad);

        [EventArgs("GADFullScreenContent")]
        [Export("adDidRecordImpression:")]
        void DidRecordImpression(NSObject ad);

        [EventArgs("GADFullScreenContent")]
        [Export("adDidRecordClick:")]
        void DidRecordClick(NSObject ad);

        [EventArgs("GADFullScreenContent")]
        [Export("adWillDismissFullScreenContent:")]
        void WillDismissFullScreenContent(NSObject ad);
    }

    // -------------------------------------------------------------------------
    // GADAdValue - monetary value for paid events.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    interface GADAdValue
    {
        [Export("value")]
        NSDecimalNumber Value { get; }

        [Export("currencyCode")]
        string CurrencyCode { get; }

        [Export("precision")]
        GADAdValuePrecision Precision { get; }
    }

    // -------------------------------------------------------------------------
    // GADInterstitialAd - full-screen interstitial ad.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface GADInterstitialAd
    {
        [Export("adUnitID")]
        string AdUnitID { get; }

        [NullAllowed]
        [Export("fullScreenContentDelegate", ArgumentSemantic.Weak)]
        NSObject WeakFullScreenContentDelegate { get; set; }

        [NullAllowed]
        [Export("responseInfo")]
        GADResponseInfo ResponseInfo { get; }

        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        Action<GADAdValue> PaidEventHandler { get; set; }

        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        [Async(ResultTypeName = "GADInterstitialAdLoadResult")]
        void Load(string adUnitID, [NullAllowed] GADRequest request, Action<GADInterstitialAd, NSError> completionHandler);

        [Export("presentFromRootViewController:")]
        void Present([NullAllowed] UIViewController rootViewController);

        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, out NSError error);
    }

    // -------------------------------------------------------------------------
    // GADAdReward - reward amount and type for rewarded ads.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    interface GADAdReward
    {
        [Export("type")]
        string Type { get; }

        [Export("amount")]
        NSDecimalNumber Amount { get; }

        [Export("initWithRewardType:rewardAmount:")]
        NativeHandle Constructor(string rewardType, NSDecimalNumber rewardAmount);
    }

    // -------------------------------------------------------------------------
    // GADRewardedAd - rewarded ad.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface GADRewardedAd
    {
        [Export("adUnitID")]
        string AdUnitID { get; }

        [NullAllowed]
        [Export("fullScreenContentDelegate", ArgumentSemantic.Weak)]
        NSObject WeakFullScreenContentDelegate { get; set; }

        [NullAllowed]
        [Export("responseInfo")]
        GADResponseInfo ResponseInfo { get; }

        [Export("adReward")]
        GADAdReward AdReward { get; }

        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        Action<GADAdValue> PaidEventHandler { get; set; }

        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        [Async(ResultTypeName = "GADRewardedAdLoadResult")]
        void Load(string adUnitID, [NullAllowed] GADRequest request, Action<GADRewardedAd, NSError> completionHandler);

        [Export("presentFromRootViewController:userDidEarnRewardHandler:")]
        void Present([NullAllowed] UIViewController rootViewController, Action userDidEarnRewardHandler);

        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, out NSError error);
    }

    // -------------------------------------------------------------------------
    // GADRewardedInterstitialAd - rewarded interstitial ad.
    // -------------------------------------------------------------------------

    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    interface GADRewardedInterstitialAd
    {
        [Export("adUnitID")]
        string AdUnitID { get; }

        [NullAllowed]
        [Export("fullScreenContentDelegate", ArgumentSemantic.Weak)]
        NSObject WeakFullScreenContentDelegate { get; set; }

        [NullAllowed]
        [Export("responseInfo")]
        GADResponseInfo ResponseInfo { get; }

        [Export("adReward")]
        GADAdReward AdReward { get; }

        [NullAllowed]
        [Export("paidEventHandler", ArgumentSemantic.Copy)]
        Action<GADAdValue> PaidEventHandler { get; set; }

        [Static]
        [Export("loadWithAdUnitID:request:completionHandler:")]
        [Async(ResultTypeName = "GADRewardedInterstitialAdLoadResult")]
        void Load(string adUnitID, [NullAllowed] GADRequest request, Action<GADRewardedInterstitialAd, NSError> completionHandler);

        [Export("presentFromRootViewController:userDidEarnRewardHandler:")]
        void Present([NullAllowed] UIViewController rootViewController, Action userDidEarnRewardHandler);

        [Export("canPresentFromRootViewController:error:")]
        bool CanPresent([NullAllowed] UIViewController rootViewController, out NSError error);
    }
}
