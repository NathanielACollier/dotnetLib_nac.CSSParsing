using System;
using System.Collections.Generic;
using System.Text;

namespace nac.CSSParsing.model;

public class StyleClass
{
    private string _name = string.Empty;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    private SortedList<string, string> _attributes = new SortedList<string, string>();
    public SortedList<string, string> Attributes
    {
        get { return _attributes; }
        set { _attributes = value; }
    }
}
