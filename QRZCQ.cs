using System.Xml.Linq;

namespace CallsignLookup;

public static class QRZCQ
{
    static readonly XNamespace ns = $"https://ssl.qrzcq.com";
    static string? sessionId;

    static async Task<string?> login(HttpClient client, string user, string pass)
    {
        if(sessionId == null)
        {
            if(string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                throw new Exception("QRZCQ kredencijali nisu konfigurirani. Molimo idite na Postavke.");
            }

            var loginUrl = $"https://ssl.qrzcq.com/xml?username={Uri.EscapeDataString(user)}&password={Uri.EscapeDataString(pass)}";

            var loginXml = XDocument.Parse(await client.GetStringAsync(loginUrl));

            sessionId = loginXml.Descendants(ns + "Key").FirstOrDefault()?.Value;
        }

        return sessionId;
    }

    public static async Task<bool> SearchQRZCQ(this Veza veza, HttpClient client, string user, string pass)
    {
        var session = await login(client, user, pass);
        if (string.IsNullOrWhiteSpace(session)) throw new Exception("Prijava u QRZCQ neuspiješna");

        var lookupUrl = $"https://ssl.qrzcq.com/xml?s={Uri.EscapeDataString(session)}&callsign={Uri.EscapeDataString(veza.BazniPozivniZnak)}";
        var lookupXml = XDocument.Parse(await client.GetStringAsync(lookupUrl));
        return lookupXml.Descendants(ns + "Callsign").Any();
    }
}