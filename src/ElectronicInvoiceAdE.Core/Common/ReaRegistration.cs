using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Common;

/// <summary>
/// Italian Business Register (REA) registration details (<IscrizioneREA>).
/// </summary>
public class ReaRegistration
{
    [XmlElement("Ufficio")]
    public string Office { get; set; } = string.Empty;

    [XmlElement("NumeroREA")]
    public string ReaNumber { get; set; } = string.Empty;

    [XmlElement("CapitaleSociale")]
    public decimal? ShareCapital { get; set; }

    [XmlElement("SocioUnico")]
    public string? SoleShareholder { get; set; }

    [XmlElement("StatoLiquidazione")]
    public string LiquidationState { get; set; } = Lookups.LiquidationState.NotInLiquidation;
}
