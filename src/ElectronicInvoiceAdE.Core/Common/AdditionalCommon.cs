using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Contact channels (<Contatti> / <ContattiTrasmittente>).
/// </summary>
public class ContactInfo
{
    [XmlElement("Telefono")]
    public string? Phone { get; set; }

    [XmlElement("Fax")]
    public string? Fax { get; set; }

    [XmlElement("Email")]
    public string? Email { get; set; }
}

/// <summary>
/// Discount or surcharge applied (<ScontoMaggiorazione>).
/// </summary>
public class DiscountOrSurcharge
{
    [XmlElement("Tipo")]
    public string Type { get; set; } = Lookups.DiscountOrSurchargeType.Discount;

    [XmlElement("Percentuale")]
    public decimal? Percentage { get; set; }

    [XmlElement("Importo")]
    public decimal? Amount { get; set; }
}

/// <summary>
/// Administrative / custom management details (<AltriDatiGestionali>).
/// Used for fuel card license plates, sports worker exemptions, intent declarations, etc.
/// </summary>
public class OtherManagementData
{
    [XmlElement("TipoDato")]
    public string DataType { get; set; } = string.Empty;

    [XmlElement("RiferimentoTesto")]
    public string? TextReference { get; set; }

    [XmlElement("RiferimentoNumero")]
    public decimal? NumberReference { get; set; }

    [XmlElement("RiferimentoData")]
    public DateTime? DateReference { get; set; }
}

/// <summary>
/// Base class for procurement and document references (<DatiDocumento>).
/// Used by Purchase Orders, Contracts, Agreements, Receptions, Related Invoices.
/// </summary>
public class DocumentReference
{
    [XmlElement("RiferimentoNumeroLinea")]
    public List<int> LineNumbers { get; set; } = new();

    [XmlElement("IdDocumento")]
    public string DocumentId { get; set; } = string.Empty;

    [XmlElement("Data")]
    public DateTime? Date { get; set; }

    [XmlElement("NumItem")]
    public string? ItemNumber { get; set; }

    [XmlElement("CodiceCommessaConvenzione")]
    public string? AgreementCode { get; set; }

    [XmlElement("CodiceCUP")]
    public string? CupCode { get; set; }

    [XmlElement("CodiceCIG")]
    public string? CigCode { get; set; }
}
