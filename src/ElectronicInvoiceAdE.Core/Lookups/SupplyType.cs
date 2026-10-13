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
