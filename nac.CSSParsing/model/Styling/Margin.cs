using System;
using System.Collections.Generic;
using System.Text;

namespace nac.CSSParsing.model.Styling;

public class Margin
{
    public float Left { get; set; }
    public float Right { get; set; }
    public float Top { get; set; }
    public float Bottom { get; set; }

    public Margin()
    {
        Left = 20;
        Right = 20;
        Top = 36;
        Bottom = 20; // need enough for a 12pt font to appear
    }




}