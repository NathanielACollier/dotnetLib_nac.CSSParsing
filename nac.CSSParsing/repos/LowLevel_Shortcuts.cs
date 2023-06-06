using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nac.CSSParsing.repos;

public class LowLevel_Shortcuts
{
    public static IEnumerable<model.LowLevel.StyleClass> ParseCSSTextGetClassList(
        string cssText)
    {
        var parser = new repos.CssParser();
        parser.AddCSSText(cssText);

        foreach (var styleClass in parser.Styles)
        {
            var declarations = ParseDeclarations(styleClass.Value);

            var c = new model.LowLevel.StyleClass()
            {
                selector = styleClass.Value.Name,
                declarations = declarations.ToList()
            };
            yield return c;
        }
    }


    public static IEnumerable<model.LowLevel.Declaration> ParseDeclarations(model.StyleClass rule)
    {
        return rule.Attributes.Select(pair => new model.LowLevel.Declaration()
        {
            Name = pair.Key,
            Value = pair.Value
        });
    }


}
