using System.Text.Json;
using System.Text.RegularExpressions;

namespace CallsignLookup;

public class Veza
{
    public DateTime Datum { get; set; }
    public string PozivniZnak { get; set; } = "";
    public string Poslano { get; set; } = "";
    public string Primljeno { get; set; } = "";
    public bool Checked { get; set; }
    public string Validirao { get; set; } = "";
    public string BazniPozivniZnak { get; set; } = "";
    public string Continent { get; set; } = "";
}