using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official payment methods (<ModalitaPagamento>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
public static class PaymentMethod
{
    public const string Cash = "MP01";
    public const string Cheque = "MP02";
    public const string BankersDraft = "MP03";
    public const string CashAtTreasury = "MP04";
    public const string BankTransfer = "MP05";
    public const string MoneyOrder = "MP06";
    public const string PrecompiledBankSlip = "MP07";
    public const string PaymentCard = "MP08";
    public const string DirectDebit = "MP09";
    public const string UtilitiesDirectDebit = "MP10";
    public const string FastDirectDebit = "MP11";
    public const string CollectionOrderRiba = "MP12";
    public const string PaymentByNotice = "MP13";
    public const string TaxOfficeQuittance = "MP14";
    public const string SpecialAccountingTransfer = "MP15";
    public const string DirectPaymentBankAccount = "MP16";
    public const string DirectPaymentPostOffice = "MP17";
    public const string PostalAccountBulletin = "MP18";
    public const string SepaDirectDebit = "MP19";
    public const string SepaDirectDebitCore = "MP20";
    public const string SepaDirectDebitB2B = "MP21";
    public const string DeductionOnCollectedSums = "MP22";
    public const string PagoPA = "MP23";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Cash] = new(Cash, "Cash", "Cash", "Contanti"),
        [Cheque] = new(Cheque, "Cheque", "Cheque", "Assegno"),
        [BankersDraft] = new(BankersDraft, "BankersDraft", "Banker's draft", "Assegno circolare"),
        [CashAtTreasury] = new(CashAtTreasury, "CashAtTreasury", "Cash at Treasury", "Contanti presso Tesoreria"),
        [BankTransfer] = new(BankTransfer, "BankTransfer", "Bank transfer", "Bonifico"),
        [MoneyOrder] = new(MoneyOrder, "MoneyOrder", "Money order", "Vaglia cambiario"),
        [PrecompiledBankSlip] = new(PrecompiledBankSlip, "PrecompiledBankSlip", "Pre-compiled bank payment slip", "Bollettino bancario"),
        [PaymentCard] = new(PaymentCard, "PaymentCard", "Payment card", "Carta di pagamento"),
        [DirectDebit] = new(DirectDebit, "DirectDebit", "Direct debit", "RID"),
        [UtilitiesDirectDebit] = new(UtilitiesDirectDebit, "UtilitiesDirectDebit", "Utilities direct debit", "RID utenze"),
        [FastDirectDebit] = new(FastDirectDebit, "FastDirectDebit", "Fast direct debit", "RID veloce"),
        [CollectionOrderRiba] = new(CollectionOrderRiba, "CollectionOrderRiba", "Collection order / Ri.Ba.", "Riba"),
        [PaymentByNotice] = new(PaymentByNotice, "PaymentByNotice", "Payment by notice (MAV)", "MAV"),
        [TaxOfficeQuittance] = new(TaxOfficeQuittance, "TaxOfficeQuittance", "Tax office quittance", "Quietanza erario"),
        [SpecialAccountingTransfer] = new(SpecialAccountingTransfer, "SpecialAccountingTransfer", "Transfer on special accounting accounts", "Giroconto su conti di contabilità speciale"),
        [DirectPaymentBankAccount] = new(DirectPaymentBankAccount, "DirectPaymentBankAccount", "Order for direct payment from bank account", "Domiciliazione bancaria"),
        [DirectPaymentPostOffice] = new(DirectPaymentPostOffice, "DirectPaymentPostOffice", "Order for direct payment from post office account", "Domiciliazione postale"),
        [PostalAccountBulletin] = new(PostalAccountBulletin, "PostalAccountBulletin", "Postal account bulletin", "Bollettino di c/c postale"),
        [SepaDirectDebit] = new(SepaDirectDebit, "SepaDirectDebit", "SEPA Direct Debit", "SEPA Direct Debit"),
        [SepaDirectDebitCore] = new(SepaDirectDebitCore, "SepaDirectDebitCore", "SEPA Direct Debit CORE", "SEPA Direct Debit CORE"),
        [SepaDirectDebitB2B] = new(SepaDirectDebitB2B, "SepaDirectDebitB2B", "SEPA Direct Debit B2B", "SEPA Direct Debit B2B"),
        [DeductionOnCollectedSums] = new(DeductionOnCollectedSums, "DeductionOnCollectedSums", "Deduction on sums already collected", "Trattenuta su somme già riscosse"),
        [PagoPA] = new(PagoPA, "PagoPA", "PagoPA payment system", "PagoPA")
    };

    public static IReadOnlyCollection<LookupCode> All => Items.Values.ToList().AsReadOnly();

    public static bool IsValid(string? code) => code != null && Items.ContainsKey(code);

    public static LookupCode? TryGet(string? code)
    {
        if (code != null && Items.TryGetValue(code, out var item))
            return item;
        return null;
    }
}
