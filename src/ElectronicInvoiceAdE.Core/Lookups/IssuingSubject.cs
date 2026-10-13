using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

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
