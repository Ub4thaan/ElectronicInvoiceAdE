using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicInvoiceAdE.Lookups;

/// <summary>
/// Official pension fund types (<TipoCassa>) per Agenzia delle Entrate Technical Specifications v1.9.1.
/// </summary>
public static class PensionFundType
{
    public const string Lawyers = "TC01";
    public const string Accountants = "TC02";
    public const string Surveyors = "TC03";
    public const string EngineersAndArchitects = "TC04";
    public const string Notaries = "TC05";
    public const string Bookkeepers = "TC06";
    public const string Enasarco = "TC07";
    public const string Enpacl = "TC08";
    public const string Enpam = "TC09";
    public const string Enpaf = "TC10";
    public const string Enpav = "TC11";
    public const string Enpaia = "TC12";
    public const string Maritime = "TC13";
    public const string Inpgi = "TC14";
    public const string Onaosi = "TC15";
    public const string Casagit = "TC16";
    public const string Eppi = "TC17";
    public const string Epap = "TC18";
    public const string Enpab = "TC19";
    public const string Enpapi = "TC20";
    public const string Enpap = "TC21";
    public const string Inps = "TC22";

    private static readonly Dictionary<string, LookupCode> Items = new(StringComparer.OrdinalIgnoreCase)
    {
        [Lawyers] = new(Lawyers, "Lawyers", "National Pension and Welfare Fund for Lawyers and Solicitors", "Cassa Nazionale Previdenza e Assistenza Avvocati e Procuratori Legali"),
        [Accountants] = new(Accountants, "Accountants", "Pension Fund for Accountants", "Cassa Previdenza Dottori Commercialisti"),
        [Surveyors] = new(Surveyors, "Surveyors", "Pension and Welfare Fund for Surveyors", "Cassa Previdenza e Assistenza Geometri"),
        [EngineersAndArchitects] = new(EngineersAndArchitects, "EngineersAndArchitects", "National Pension and Welfare Fund for Self-employed Engineers and Architects", "Cassa Nazionale Previdenza e Assistenza Ingegneri e Architetti Liberi Professionisti"),
        [Notaries] = new(Notaries, "Notaries", "National Fund for Solicitors / Notaries", "Cassa Nazionale del Notariato"),
        [Bookkeepers] = new(Bookkeepers, "Bookkeepers", "National Pension and Welfare Fund for Bookkeepers and Commercial Experts", "Cassa Nazionale Previdenza e Assistenza Ragionieri e Periti Commerciali"),
        [Enasarco] = new(Enasarco, "Enasarco", "National Welfare Board for Sales Agents and Representatives (ENASARCO)", "Ente Nazionale Assistenza Agenti e Rappresentanti di Commercio"),
        [Enpacl] = new(Enpacl, "Enpacl", "National Pension and Welfare Board for Employment Consultants (ENPACL)", "Ente Nazionale Previdenza e Assistenza Consulenti del Lavoro"),
        [Enpam] = new(Enpam, "Enpam", "National Pension and Welfare Board for Doctors (ENPAM)", "Ente Nazionale Previdenza e Assistenza Medici"),
        [Enpaf] = new(Enpaf, "Enpaf", "National Pension and Welfare Board for Pharmacists (ENPAF)", "Ente Nazionale Previdenza e Assistenza Farmacisti"),
        [Enpav] = new(Enpav, "Enpav", "National Pension and Welfare Board for Veterinary Physicians (ENPAV)", "Ente Nazionale Previdenza e Assistenza Veterinari"),
        [Enpaia] = new(Enpaia, "Enpaia", "National Pension and Welfare Board for Agricultural Employees (ENPAIA)", "Ente Nazionale Previdenza e Assistenza Impiegati dell'Agricoltura"),
        [Maritime] = new(Maritime, "Maritime", "Pension Fund for Employees of Shipping Companies and Maritime Agencies", "Fondo Previdenza Impiegati Imprese Spedizioni Marittime"),
        [Inpgi] = new(Inpgi, "Inpgi", "National Pension Institute for Italian Journalists (INPGI)", "Istituto Nazionale Previdenza Giornalisti Italiani"),
        [Onaosi] = new(Onaosi, "Onaosi", "National Welfare Board for Orphans of Italian Doctors (ONAOSI)", "Opera Nazionale Assistenza Orfani Sanitari Italiani"),
        [Casagit] = new(Casagit, "Casagit", "Autonomous Supplementary Welfare Fund for Italian Journalists (CASAGIT)", "Cassa Autonoma Assistenza Integrativa Giornalisti Italiani"),
        [Eppi] = new(Eppi, "Eppi", "Pension Board for Industrial Experts (EPPI)", "Ente Previdenza Periti Industriali e Laureati"),
        [Epap] = new(Epap, "Epap", "National Multi-category Pension and Welfare Board (EPAP)", "Ente Previdenza e Assistenza Pluricategoriale"),
        [Enpab] = new(Enpab, "Enpab", "National Pension and Welfare Board for Biologists (ENPAB)", "Ente Nazionale Previdenza e Assistenza Biologi"),
        [Enpapi] = new(Enpapi, "Enpapi", "National Pension and Welfare Board for Nursing Profession (ENPAPI)", "Ente Nazionale Previdenza Professione Infermieristica"),
        [Enpap] = new(Enpap, "Enpap", "National Pension and Welfare Board for Psychologists (ENPAP)", "Ente Nazionale Previdenza e Assistenza Psicologi"),
        [Inps] = new(Inps, "Inps", "National Social Security Institute (INPS)", "Istituto Nazionale Previdenza Sociale")
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
