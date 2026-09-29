using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Supply / delivery type (<TipoCessionePrestazione>).
/// </summary>
public static class SupplyType
{
    public const string Discount = "SC";
    public const string Bonus = "PR";
    public const string Rebate = "AB";
    public const string AccessoryExpense = "AC";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Discount] = new(Discount, "Discount", "Discount", "Sconto"),
        [Bonus] = new(Bonus, "Bonus", "Bonus / prize", "Premio"),
        [Rebate] = new(Rebate, "Rebate", "Rebate / price allowance", "Abbuono"),
        [AccessoryExpense] = new(AccessoryExpense, "AccessoryExpense", "Accessory expense", "Spesa accessoria")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();
    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);
    public static LookupCode? TryGet(string? code) => code != null && Items.TryGetValue(code, out var item) ? item : null;
}

/// <summary>
/// Sole shareholder status (<SocioUnico>).
/// </summary>
public static class ShareholderStatus
{
    public const string SoleShareholder = "SU";
    public const string MultipleShareholders = "SM";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [SoleShareholder] = new(SoleShareholder, "SoleShareholder", "Sole shareholder", "Socio unico"),
        [MultipleShareholders] = new(MultipleShareholders, "MultipleShareholders", "Multiple shareholders", "Più soci")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();
    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);
    public static LookupCode? TryGet(string? code) => code != null && Items.TryGetValue(code, out var item) ? item : null;
}

/// <summary>
/// Company liquidation state (<StatoLiquidazione>).
/// </summary>
public static class LiquidationState
{
    public const string InLiquidation = "LS";
    public const string NotInLiquidation = "LN";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [InLiquidation] = new(InLiquidation, "InLiquidation", "In liquidation", "In liquidazione"),
        [NotInLiquidation] = new(NotInLiquidation, "NotInLiquidation", "Not in liquidation", "Non in liquidazione")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();
    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);
    public static LookupCode? TryGet(string? code) => code != null && Items.TryGetValue(code, out var item) ? item : null;
}

/// <summary>
/// Discount or surcharge indicator (<Tipo> in <ScontoMaggiorazione>).
/// </summary>
public static class DiscountOrSurchargeType
{
    public const string Discount = "SC";
    public const string Surcharge = "MG";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Discount] = new(Discount, "Discount", "Discount", "Sconto"),
        [Surcharge] = new(Surcharge, "Surcharge", "Surcharge / mark-up", "Maggiorazione")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();
    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);
    public static LookupCode? TryGet(string? code) => code != null && Items.TryGetValue(code, out var item) ? item : null;
}

/// <summary>
/// Third party issuing subject (<SoggettoEmittente>).
/// </summary>
public static class IssuingSubject
{
    public const string BuyerOrCustomer = "CC";
    public const string ThirdParty = "TZ";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [BuyerOrCustomer] = new(BuyerOrCustomer, "BuyerOrCustomer", "Customer / Buyer", "Cessionario / Committente"),
        [ThirdParty] = new(ThirdParty, "ThirdParty", "Third party intermediary", "Terzo")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();
    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);
    public static LookupCode? TryGet(string? code) => code != null && Items.TryGetValue(code, out var item) ? item : null;
}
