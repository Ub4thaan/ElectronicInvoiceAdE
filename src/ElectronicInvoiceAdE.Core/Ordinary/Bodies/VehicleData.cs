using System;
using System.Xml.Serialization;

namespace ElectronicInvoiceAdE.Ordinary.Bodies;

/// <summary>
/// Vehicle data (<DatiVeicoli>) pursuant to art. 38, paragraph 4 of DL 331/1993 for new intra-community motor vehicles.
/// </summary>
public class VehicleData
{
    [XmlElement("Data")]
    public DateTime FirstRegistrationDate { get; set; }

    [XmlElement("TotalePercorso")]
    public string TotalTravel { get; set; } = string.Empty;
}
