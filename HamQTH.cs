using System.Xml.Linq;

namespace CallsignLookup;

public static class HamQTH
{
    static readonly XNamespace ns = "https://www.hamqth.com";
    static string? sessionId;

    static async Task<string?> login(HttpClient client, string user, string pass)
    {
        if(sessionId == null)
        {
            if(string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                throw new Exception("HamQTH kredencijali nisu konfigurirani. Molimo idite na Postavke.");
            }

            var loginUrl = $"https://www.hamqth.com/xml.php?u={Uri.EscapeDataString(user)}&p={Uri.EscapeDataString(pass)}";
            var loginXml = XDocument.Parse(await client.GetStringAsync(loginUrl));

            sessionId = loginXml.Descendants(ns + "session_id").FirstOrDefault()?.Value;
        }

        return sessionId;
    }
    
    public static async Task<bool> SearchHamQTH(this Veza veza, HttpClient client, string user, string pass)
    {
        var session = await login(client, user, pass);
        if (string.IsNullOrWhiteSpace(session)) throw new Exception("Prijava u HamQTH neuspiješna");

        var searchUrl = $"https://www.hamqth.com/xml.php?id={Uri.EscapeDataString(session)}&callsign={Uri.EscapeDataString(veza.BazniPozivniZnak)}&prg=CallsignLookup";
        var searchXml = XDocument.Parse(await client.GetStringAsync(searchUrl));

        return searchXml.Descendants(ns + "search").Any();
    }
}