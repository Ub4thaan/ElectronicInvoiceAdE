using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;
using ElectronicInvoiceAdE.Ordinary.Bodies;

namespace ElectronicInvoiceAdE.Simplified;

/// <summary>
/// VAT information on a simplified invoice line (&lt;DatiIVA&gt;).
/// </summary>
public class SimplifiedVatData
{
    [XmlElement("Imposta")]
    public decimal? TaxAmount { get; set; }

    [XmlElement("Aliquota")]
    public decimal? Rate { get; set; }
}

/// <summary>
/// Line item for goods or services in a simplified invoice (&lt;DatiBeniServizi&gt;).
/// </summary>
public class SimplifiedGoodsAndServices
{
    [XmlElement("Descrizione")]
    public string Description { get; set; } = string.Empty;

    [XmlElement("Importo")]
    public decimal Amount { get; set; }

    [XmlElement("DatiIVA")]
    public SimplifiedVatData? VatData { get; set; }

    [XmlElement("Natura")]
    public string? VatNature { get; set; }

    [XmlElement("RiferimentoNormativo")]
    public string? RegulatoryReference { get; set; }
}

/// <summary>
/// Reference to an amended/rectified invoice in a simplified credit/debit note (&lt;DatiFatturaRettificata&gt;).
/// </summary>
public class RectifiedInvoiceData
{
    [XmlElement("NumeroFattura")]
    public string InvoiceNumber { get; set; } = string.Empty;

    [XmlElement("DataFattura")]
    public DateTime InvoiceDate { get; set; }

    [XmlElement("ElementiRettificati")]
    public string? RectifiedElements { get; set; }
}

/// <summary>
/// General document data for a simplified invoice (&lt;DatiGeneraliDocumento&gt;).
/// </summary>
public class SimplifiedGeneralDocumentData
{
    [XmlElement("TipoDocumento")]
    public string DocumentType { get; set; } = Lookups.DocumentType.SimplifiedInvoice;

    [XmlElement("Divisa")]
    public string Currency { get; set; } = "EUR";

    [XmlElement("Data")]
    public DateTime Date { get; set; }

    [XmlElement("Numero")]
    public string Number { get; set; } = string.Empty;

    [XmlElement("DatiBollo")]
    public StampDuty? StampDuty { get; set; }
}

/// <summary>
/// General data block for a simplified invoice (&lt;DatiGenerali&gt;).
/// </summary>
public class SimplifiedGeneralData
{
    [XmlElement("DatiGeneraliDocumento")]
    public SimplifiedGeneralDocumentData DocumentGeneralData { get; set; } = new();

    [XmlElement("DatiFatturaRettificata")]
    public RectifiedInvoiceData? RectifiedInvoice { get; set; }
}

/// <summary>
/// Body of a simplified invoice (&lt;FatturaElettronicaBody&gt;).
/// </summary>
public class SimplifiedInvoiceBody
{
    [XmlElement("DatiGenerali")]
    public SimplifiedGeneralData GeneralData { get; set; } = new();

    [XmlElement("DatiBeniServizi")]
    public List<SimplifiedGoodsAndServices> GoodsAndServices { get; set; } = new();

    [XmlElement("Allegati")]
    public List<Attachment> Attachments { get; set; } = new();
}

/// <summary>
/// Customer identification in a simplified invoice (&lt;CessionarioCommittente&gt;).
/// </summary>
public class SimplifiedCustomer
{
    [XmlElement("IdentificativiFiscali")]
    public SimplifiedTaxIdentification? TaxIdentification { get; set; }

    [XmlElement("AltriDatiIdentificativi")]
    public SimplifiedOtherIdentification? OtherIdentification { get; set; }
}

public class SimplifiedTaxIdentification
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId? TaxId { get; set; }

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }
}

public class SimplifiedOtherIdentification
{
    [XmlElement("Denominazione")]
    public string? CompanyName { get; set; }

    [XmlElement("Nome")]
    public string? FirstName { get; set; }

    [XmlElement("Cognome")]
    public string? LastName { get; set; }

    [XmlElement("Sede")]
    public Address? Address { get; set; }

    [XmlElement("StabileOrganizzazione")]
    public Address? PermanentEstablishment { get; set; }

    [XmlElement("RappresentanteFiscale")]
    public SimplifiedTaxRepresentative? Representative { get; set; }
}

public class SimplifiedTaxRepresentative
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId TaxId { get; set; } = new();

    [XmlElement("Denominazione")]
    public string? CompanyName { get; set; }

    [XmlElement("Nome")]
    public string? FirstName { get; set; }

    [XmlElement("Cognome")]
    public string? LastName { get; set; }
}

/// <summary>
/// Supplier data in a simplified invoice (&lt;CedentePrestatore&gt;).
/// </summary>
public class SimplifiedSupplier
{
    [XmlElement("IdFiscaleIVA")]
    public TaxId TaxId { get; set; } = new();

    [XmlElement("CodiceFiscale")]
    public string? FiscalCode { get; set; }

    [XmlElement("Denominazione")]
    public string? CompanyName { get; set; }

    [XmlElement("Nome")]
    public string? FirstName { get; set; }

    [XmlElement("Cognome")]
    public string? LastName { get; set; }

    [XmlElement("Sede")]
    public Address Address { get; set; } = new();

    [XmlElement("StabileOrganizzazione")]
    public Address? PermanentEstablishment { get; set; }

    [XmlElement("RappresentanteFiscale")]
    public SimplifiedTaxRepresentative? Representative { get; set; }

    [XmlElement("IscrizioneREA")]
    public ReaRegistration? ReaRegistration { get; set; }

    [XmlElement("RegimeFiscale")]
    public string TaxRegime { get; set; } = Lookups.TaxRegime.Ordinary;
}

/// <summary>
/// Header for simplified invoice (&lt;FatturaElettronicaHeader&gt;).
/// </summary>
public class SimplifiedInvoiceHeader
{
    [XmlElement("DatiTrasmissione")]
    public Ordinary.Headers.TransmissionData TransmissionData { get; set; } = new();

    [XmlElement("CedentePrestatore")]
    public SimplifiedSupplier Supplier { get; set; } = new();

    [XmlElement("CessionarioCommittente")]
    public SimplifiedCustomer Customer { get; set; } = new();
}

/// <summary>
/// Italian Simplified Electronic Invoice (&lt;FatturaElettronicaSemplificata&gt; format FSM10).
/// </summary>
[XmlRoot("FatturaElettronicaSemplificata", Namespace = XmlNamespaces.FatturaSemplificataV10)]
public class SimplifiedInvoice
{
    public SimplifiedInvoice()
    {
        Header = new();
        Bodies = new();
    }

    [XmlAttribute("versione")]
    public string Version { get; set; } = TransmissionFormat.Simplified;

    [XmlAttribute("SistemaEmittente")]
    public string? IssuingSystem { get; set; }

    [XmlElement("FatturaElettronicaHeader")]
    public SimplifiedInvoiceHeader Header { get; set; }

    [XmlElement("FatturaElettronicaBody")]
    public List<SimplifiedInvoiceBody> Bodies { get; set; }

    [XmlIgnore]
    public SimplifiedInvoiceBody PrimaryBody
    {
        get
        {
            if (Bodies.Count == 0)
            {
                Bodies.Add(new SimplifiedInvoiceBody());
            }
            return Bodies[0];
        }
    }
}
