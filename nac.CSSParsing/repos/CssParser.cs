using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace nac.CSSParsing.repos;

public class CssParser
{
    private SortedList<string, model.StyleClass> _scc;
    public SortedList<string, model.StyleClass> Styles
    {
        get { return this._scc; }
        set { this._scc = value; }
    }

    public CssParser()
    {
        this._scc = new SortedList<string, model.StyleClass>();
    }

    public void AddStyleSheet(string path)
    {
        var cssText = System.IO.File.ReadAllText(path);
        ProcessStyleSheet(cssText);
    }

    public void AddCSSText(string cssText)
    {
        ProcessStyleSheet(cssText);
    }

    private void ProcessStyleSheet(string cssText)
    {
        string[] parts = cssText.Split('}');
        foreach (string s in parts)
        {
            if (CleanUp(s).IndexOf('{') > -1)
            {
                FillStyleClass(s);
            }
        }
    }

    private void FillStyleClass(string s)
    {
        model.StyleClass sc = null;
        string[] parts = s.Split('{');
        string styleName = CleanUp(parts[0]).Trim().ToLower();

        if (this._scc.ContainsKey(styleName))
        {
            sc = this._scc[styleName];
            this._scc.Remove(styleName);
        }
        else
        {
            sc = new model.StyleClass();
        }

        sc.Name = styleName;

        string[] atrs = CleanUp(parts[1]).Replace("}", "").Split(';');
        foreach (string a in atrs)
        {
            if (a.Contains(":"))
            {
                string _key = a.Split(':')[0].Trim().ToLower();
                if (sc.Attributes.ContainsKey(_key))
                {
                    sc.Attributes.Remove(_key);
                }
                sc.Attributes.Add(_key, a.Split(':')[1].Trim().ToLower());
            }
        }
        this._scc.Add(sc.Name, sc);
    }

    private string CleanUp(string s)
    {
        string temp = s;
        string reg = "(/\\*(.|[\r\n])*?\\*/)|(//.*)";
        Regex r = new Regex(reg);
        temp = r.Replace(temp, "");
        temp = temp.Replace("\r", "").Replace("\n", "");
        return temp;
    }


}
