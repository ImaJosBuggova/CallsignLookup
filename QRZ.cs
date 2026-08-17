using System.Xml.Linq;

namespace CallsignLookup;

public static class QRZ
{
    static readonly XNamespace ns = $"http://xmldata.qrz.com";
    static string? sessionId;

    static async Task<string?> login(HttpClient client)
    {
        if(sessionId == null)
        {
            var user = Uri.EscapeDataString("9A3RTJ");
            var pass = Uri.EscapeDataString("L0z!nk4123");

            var loginUrl = $"https://xmldata.qrz.com/xml/current/?username={user};password={pass}";

            var loginXml = XDocument.Parse(await client.GetStringAsync(loginUrl));

            sessionId = loginXml
                .Descendants(ns + "Key")
                .FirstOrDefault()?
                .Value;
        }

        return sessionId;
    }
    
    public static async Task<bool> SearchQRZ(this Veza veza, HttpClient client)
    {
        var session = await login(client);
        if (string.IsNullOrWhiteSpace(session)) throw new Exception("Prijava u QRZ neuspiješna");
        
        var lookupUrl = $"https://xmldata.qrz.com/xml/current/?s={Uri.EscapeDataString(session)};callsign={Uri.EscapeDataString(veza.BazniPozivniZnak)}";
        var lookupXml = XDocument.Parse(await client.GetStringAsync(lookupUrl));
        return lookupXml.Descendants(ns + "Callsign").Any();
    }
}