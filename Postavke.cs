using MAES.Core;

namespace CallsignLookup;

public class Postavke : AppData
{
    public bool QRZ { get; set; } = true;
    public string QRZUsername { get; set; } = "";
    public string QRZPassword { get; set; } = "";

    public bool QRZCQ { get; set; }
    public string QRZCQUsername { get; set; } = "";
    public string QRZCQPassword { get; set; } = "";

    public bool HamQTH { get; set; } = true;
    public string HamQTHUsername { get; set; } = "";
    public string HamQTHPassword { get; set; } = "";

    public bool Callook { get; set; } = true;
}