using System.Collections.Generic;
using System.Xml.Serialization;
using ElectronicInvoiceAdE.Common;
using ElectronicInvoiceAdE.Lookups;

namespace ElectronicInvoiceAdE.Simplified;

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
