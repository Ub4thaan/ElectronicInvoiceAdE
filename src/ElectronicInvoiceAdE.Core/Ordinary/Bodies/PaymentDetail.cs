using System;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Detail of an individual payment installment (<DettaglioPagamento>).
/// </summary>
public class PaymentDetail
{
	[XmlElement("Beneficiario")]
	public string? Beneficiary { get; set; }

	[XmlElement("ModalitaPagamento")]
	public string PaymentMethod { get; set; } = Lookups.PaymentMethod.BankTransfer;

	[XmlElement("DataRiferimentoTerminiPagamento")]
	public DateTime? PaymentTermStart { get; set; }

	[XmlElement("GiorniTerminiPagamento")]
	public int? PaymentTermDays { get; set; }

	[XmlElement("DataScadenzaPagamento")]
	public DateTime? DueDate { get; set; }

	[XmlElement("ImportoPagamento")]
	public decimal Amount { get; set; }

	[XmlElement("CodiceUfficioPostale")]
	public string? PostOfficeCode { get; set; }

	[XmlElement("CognomeQuietanzante")]
	public string? PayerSurname { get; set; }

	[XmlElement("NomeQuietanzante")]
	public string? PayerName { get; set; }

	[XmlElement("CFQuietanzante")]
	public string? PayerFiscalCode { get; set; }

	[XmlElement("TitoloQuietanzante")]
	public string? PayerTitle { get; set; }

	[XmlElement("IstitutoFinanziario")]
	public string? FinancialInstitution { get; set; }

	[XmlElement("IBAN")]
	public string? Iban { get; set; }

	[XmlElement("ABI")]
	public string? Abi { get; set; }

	[XmlElement("CAB")]
	public string? Cab { get; set; }

	[XmlElement("BIC")]
	public string? Bic { get; set; }

	[XmlElement("ScontoPagamentoAnticipato")]
	public decimal? EarlyPaymentDiscount { get; set; }

	[XmlElement("DataLimitePagamentoAnticipato")]
	public DateTime? EarlyPaymentDiscountDeadline { get; set; }

	[XmlElement("PenalitaPagamentiRitardati")]
	public decimal? LatePaymentPenalty { get; set; }

	[XmlElement("DataDecorrenzaPenale")]
	public DateTime? LatePaymentPenaltyStart { get; set; }

	[XmlElement("CodicePagamento")]
	public string? PaymentCode { get; set; }
}
