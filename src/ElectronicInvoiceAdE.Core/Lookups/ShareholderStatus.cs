using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

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
