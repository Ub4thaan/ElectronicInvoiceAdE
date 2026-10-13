using System;

namespace ElectronicInvoiceAdE.Security;

/// <summary>
/// Digital signature info extracted from a CAdES-BES enveloped electronic invoice (.p7m).
/// </summary>
public class SignatureInfo
{
	public string Subject { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
	public DateTime NotBefore { get; set; }
	public DateTime NotAfter { get; set; }
	public string SerialNumber { get; set; } = string.Empty;
	public string Thumbprint { get; set; } = string.Empty;
	public DateTime? SigningTime { get; set; }
}
