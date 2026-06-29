using System.Runtime.InteropServices;
using ObjCRuntime;

namespace Ssit.Ads.AdMob.iOS;

// -------------------------------------------------------------------------
// GADAdSize - passed by value from Objective-C.
// Native layout: { CGSize size; NSUInteger flags; }
// On 64-bit iOS: CGSize = 2x CGFloat (double) = 16 bytes, NSUInteger = 8 bytes.
// Total: 24 bytes.
// -------------------------------------------------------------------------

[StructLayout(LayoutKind.Sequential)]
public struct GADAdSize
{
    public CGSize Size;
    public nuint Flags;
}

// -------------------------------------------------------------------------
// GADAdSizes - standard banner sizes as C# constants.
// [Field] with custom struct is not supported by bgen, so values are hardcoded.
// All standard fixed-size banners have Flags = 0 (kGADAdSizeFlagsNone).
// -------------------------------------------------------------------------

public static class GADAdSizes
{
    public static GADAdSize Banner => new GADAdSize { Size = new CGSize(320, 50) };
    public static GADAdSize MediumRectangle => new GADAdSize { Size = new CGSize(300, 250) };
    public static GADAdSize Leaderboard => new GADAdSize { Size = new CGSize(728, 90) };
    public static GADAdSize FullBanner => new GADAdSize { Size = new CGSize(468, 60) };
    public static GADAdSize Skyscraper => new GADAdSize { Size = new CGSize(120, 600) };
}

// -------------------------------------------------------------------------
// GADAdapterInitializationState - state of each mediation adapter at startup.
// -------------------------------------------------------------------------

[Native]
public enum GADAdapterInitializationState : long
{
    NotReady = 0,
    Ready = 1,
}

// -------------------------------------------------------------------------
// GADAdValuePrecision - precision level for paid event monetary reporting.
// -------------------------------------------------------------------------

[Native]
public enum GADAdValuePrecision : long
{
    Unknown = 0,
    Estimated = 1,
    PublisherProvided = 2,
    Precise = 3,
}

// -------------------------------------------------------------------------
// GADErrorCode - error codes returned in NSError.code for GADErrorDomain.
// -------------------------------------------------------------------------

[Native]
public enum GADErrorCode : long
{
    InvalidRequest = 0,
    NoFill = 1,
    NetworkError = 2,
    ServerError = 3,
    OSVersionTooLow = 4,
    Timeout = 5,
    MediationDataError = 7,
    MediationAdapterError = 8,
    MediationNoFill = 9,
    MediationInvalidAdSize = 10,
    InternalError = 11,
    InvalidArgument = 12,
    ReceivedInvalidResponse = 13,
    ApplicationIdentifierMissing = 14,
}