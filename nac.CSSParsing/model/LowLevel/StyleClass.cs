using System;
using System.Collections.Generic;
using System.Text;

namespace nac.CSSParsing.model.LowLevel;

public class StyleClass
{
    public string selector { get; set; }
    public List<Declaration> declarations { get; set; }
}
