using System.Text.Json;

namespace CallsignLookup;

public static class Callook
{
    public static async Task<bool> SearchCallook(this Veza veza, HttpClient client)
    {
        var uri = $"https://callook.info/{Uri.EscapeDataString(veza.BazniPozivniZnak)}/json";
        //Console.WriteLine(uri);
        using JsonDocument doc = JsonDocument.Parse(await client.GetStringAsync(uri));
        if (doc.RootElement.GetProperty("status").GetString()?.ToUpper() == "VALID") return true;
        return false;
    }
}