﻿using System;
using System.Collections.Generic;
using System.Text;

namespace nac.CSSParsing.model.Styling;

public class Border
{
    public float Width { get; set; }
    public string Color { get; set; }

    public BorderStyle Style { get; set; }

    public Border()
    {
        this.Style = BorderStyle.Solid;
        this.Color = "Black";
        this.Width = 1;
    }
}
