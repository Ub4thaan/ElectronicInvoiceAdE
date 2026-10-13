using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicInvoiceAdE.Ordinary;
using ElectronicInvoiceAdE.Simplified;

namespace ElectronicInvoiceAdE.Serialization;

/// <summary>
/// JSON serialization and deserialization helpers for electronic invoices.
/// </summary>
public static class JsonSerialization
{
	private static readonly JsonSerializerOptions Options = new()
	{
		WriteIndented = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	private static readonly JsonSerializerOptions CompactOptions = new(Options)
	{
		WriteIndented = false
	};

	public static string ToJson(OrdinaryInvoice invoice, bool indented = true)
	{
		ArgumentNullException.ThrowIfNull(invoice);
		var opts = indented ? Options : CompactOptions;
		return JsonSerializer.Serialize(invoice, opts);
	}

	public static string ToJson(SimplifiedInvoice invoice, bool indented = true)
	{
		ArgumentNullException.ThrowIfNull(invoice);
		var opts = indented ? Options : CompactOptions;
		return JsonSerializer.Serialize(invoice, opts);
	}

	public static OrdinaryInvoice? FromOrdinaryJson(string json)
	{
		ArgumentNullException.ThrowIfNull(json);
		return JsonSerializer.Deserialize<OrdinaryInvoice>(json, Options);
	}

	public static SimplifiedInvoice? FromSimplifiedJson(string json)
	{
		ArgumentNullException.ThrowIfNull(json);
		return JsonSerializer.Deserialize<SimplifiedInvoice>(json, Options);
	}
}
