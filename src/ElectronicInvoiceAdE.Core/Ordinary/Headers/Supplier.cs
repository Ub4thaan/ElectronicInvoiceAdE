using System;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Headers;


/// <summary>
/// Seller / Supplier data (<CedentePrestatore>).
/// </summary>
public class Supplier
{
	[XmlElement("DatiAnagrafici")]
	public SupplierIdentification Identification { get; set; } = new();

	[XmlElement("Sede")]
	public Address Address { get; set; } = new();

	[XmlElement("StabileOrganizzazione")]
	public Address? PermanentEstablishment { get; set; }

	[XmlElement("IscrizioneREA")]
	public ReaRegistration? ReaRegistration { get; set; }

	[XmlElement("Contatti")]
	public ContactInfo? Contacts { get; set; }

	[XmlElement("RiferimentoAmministrazione")]
	public string? AdministrativeReference { get; set; }
}
