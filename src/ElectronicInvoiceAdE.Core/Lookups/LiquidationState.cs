using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

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
