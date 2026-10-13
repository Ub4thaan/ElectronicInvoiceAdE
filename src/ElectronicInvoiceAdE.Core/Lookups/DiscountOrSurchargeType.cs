using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

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
