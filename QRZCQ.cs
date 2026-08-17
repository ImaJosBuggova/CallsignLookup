using System.Xml.Linq;

namespace CallsignLookup;

public static class QRZCQ
{
    static readonly XNamespace ns = $"https://ssl.qrzcq.com";
    static string? sessionId;

    static async Task<string?> login(HttpClient client)
    {
        if(sessionId == null)
        {
            var user = Uri.EscapeDataString("9A3RTJ");
            var pass = Uri.EscapeDataString("L0z!nk4123");

            var loginUrl = $"https://ssl.qrzcq.com/xml?username={user}&password={pass}";

            var loginXml = XDocument.Parse(await client.GetStringAsync(loginUrl));

            sessionId = loginXml.Descendants(ns + "Key").FirstOrDefault()?.Value;
        }

        return sessionId;
    }

    public static async Task<bool> SearchQRZCQ(this Veza veza, HttpClient client)
    {
        var session = await login(client);
        if (string.IsNullOrWhiteSpace(session)) throw new Exception("Prijava u QRZCQ neuspiješna");

        var lookupUrl = $"https://ssl.qrzcq.com/xml?s={Uri.EscapeDataString(session)}&callsign={Uri.EscapeDataString(veza.BazniPozivniZnak)}";
        var lookupXml = XDocument.Parse(await client.GetStringAsync(lookupUrl));
        return lookupXml.Descendants(ns + "Callsign").Any();
    }
}