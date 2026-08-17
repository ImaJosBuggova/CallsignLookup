using System.Xml.Linq;

namespace CallsignLookup;

public static class HamQTH
{
    static readonly XNamespace ns = "https://www.hamqth.com";
    static string? sessionId;

    static async Task<string?> login(HttpClient client)
    {
        if(sessionId == null)
        {
            var user = Uri.EscapeDataString("9A3RTJ");
            var pass = Uri.EscapeDataString("L0z!nk4123");
            var loginUrl = $"https://www.hamqth.com/xml.php?u={user}&p={pass}";
            var loginXml = XDocument.Parse(await client.GetStringAsync(loginUrl));

            sessionId = loginXml.Descendants(ns + "session_id").FirstOrDefault()?.Value;
        }

        return sessionId;
    }
    
    public static async Task<bool> SearchHamQTH(this Veza veza, HttpClient client)
    {
        var session = await login(client);
        if (string.IsNullOrWhiteSpace(session)) throw new Exception("Prijava u HamQTH neuspiješna");

        var searchUrl = $"https://www.hamqth.com/xml.php?id={Uri.EscapeDataString(session)}&callsign={Uri.EscapeDataString(veza.BazniPozivniZnak)}&prg=CallsignLookup";
        var searchXml = XDocument.Parse(await client.GetStringAsync(searchUrl));

        return searchXml.Descendants(ns + "search").Any();
    }
}