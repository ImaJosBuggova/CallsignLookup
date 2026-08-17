using System.Text.Json;
using System.Text.RegularExpressions;

namespace CallsignLookup;

public static class Parser
{
    static JsonDocument dxcc = JsonDocument.Parse(File.ReadAllText("dxcc.json"));
    
    public static async Task<List<Veza>> UcitajLog(this string filename)
    {
        List<Veza> result = [];
        foreach(var line in (await File.ReadAllLinesAsync(filename)).Where(x => x.StartsWith("QSO:")))
        {
            var data = line.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Skip(1).ToArray();
            var dateStr = data[2].Split('-');

            var bazniPozivniZnak = data[8].ToUpper().Split('/')
                .OrderByDescending(x => x.Length)
                .FirstOrDefault(x => Regex.IsMatch(x, @"^[A-Z0-9]+\d[A-Z0-9]+$", RegexOptions.IgnoreCase)) ?? data[8].ToUpper();

            result.Add(new ()
            {
                Datum = new DateTime(Convert.ToInt32(dateStr[0]), Convert.ToInt32(dateStr[1]), Convert.ToInt32(dateStr[2]), Convert.ToInt32(data[3][..2]), Convert.ToInt32(data[3][2..]), 0),
                Poslano = data[6],
                Primljeno = data[10],
                PozivniZnak = data[8].ToUpper(),
                BazniPozivniZnak = bazniPozivniZnak,
                Continent = dohvatiKontinent(bazniPozivniZnak)
            });
        }
        return result;
    }

    static string dohvatiKontinent(string pozivniZnak)
    {
        foreach (var entity in dxcc.RootElement.GetProperty("dxcc").EnumerateArray())
        {
            if (entity.GetProperty("deleted").GetBoolean())
                continue;
            string regex = entity.GetProperty("prefixRegex").GetString()!;
            if (string.IsNullOrWhiteSpace(regex))
                continue;
            if (Regex.IsMatch(pozivniZnak, regex, RegexOptions.IgnoreCase))
            {
                return entity.GetProperty("continent")[0].GetString() ?? "";
            }
        }
        return "";
    }
}