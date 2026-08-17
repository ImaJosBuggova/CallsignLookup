using System.Xml.Linq;

namespace CallsignLookup;

public static class QRZ
{
    static readonly XNamespace ns = $"http://xmldata.qrz.com";
    static string? sessionId;

    static async Task<string?> login(HttpClient client, string user, string pass)
    {
        if(sessionId == null)
        {
            if(string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                throw new Exception("QRZ kredencijali nisu konfigurirani. Molimo idite na Postavke.");
            }

            var loginUrl = $"https://xmldata.qrz.com/xml/current/?username={Uri.EscapeDataString(user)};password={Uri.EscapeDataString(pass)}";

            var loginXml = XDocument.Parse(await client.GetStringAsync(loginUrl));

            sessionId = loginXml
                .Descendants(ns + "Key")
                .FirstOrDefault()?
                .Value;
        }

        return sessionId;
    }
    
    public static async Task<bool> SearchQRZ(this Veza veza, HttpClient client, string user, string pass)
    {
        var session = await login(client, user, pass);
        if (string.IsNullOrWhiteSpace(session)) throw new Exception("Prijava u QRZ neuspiješna");
        
        var lookupUrl = $"https://xmldata.qrz.com/xml/current/?s={Uri.EscapeDataString(session)};callsign={Uri.EscapeDataString(veza.BazniPozivniZnak)}";
        var lookupXml = XDocument.Parse(await client.GetStringAsync(lookupUrl));
        return lookupXml.Descendants(ns + "Callsign").Any();
    }
}