using System;

namespace nac.CSSParsing.model.Styling;

public class NumberWithUOM
{
    public decimal Value { get; internal set; }
    public string UOM { get; internal set; }

    public int ValueI => (int)Math.Floor(this.Value);
}