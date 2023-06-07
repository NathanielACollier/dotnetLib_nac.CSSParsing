using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using styleModel = nac.CSSParsing.model.Styling;
using lowLevelModel = nac.CSSParsing.model.LowLevel;

namespace nac.CSSParsing;

public static class StyleParsingHelper
{

    public static IEnumerable<styleModel.Style> ParseCSSRuleSet(string cssText)
    {
        // use ExCSS (Nuget Package) originally from: https://github.com/TylerBrinks/ExCSS
        var classes = nac.CSSParsing.repos.LowLevel_Shortcuts.ParseCSSTextGetClassList(cssText);

        foreach (var item in classes)
        {
            yield return ConvertFromCSSToReportStyle(item);
        }
    }


    public static styleModel.Style ParseSingleCSSRule(string cssDeclarationsText)
    {
        string inlineSelectorName = ".inline"; // use this so compare is exact
        string styleHousingText = $"{inlineSelectorName} {{\n{cssDeclarationsText}\n}}"; // need to do this to get the library to work

        var inlineClass = nac.CSSParsing.repos.LowLevel_Shortcuts.ParseCSSTextGetClassList(styleHousingText)
            .Single();

        return ConvertFromCSSToReportStyle(inlineClass);
    }



    private static string GetDeclarationValue(lowLevelModel.StyleClass rule, string declarationName)
    {
        var declarationQuery = rule.declarations
                                        .Where(d => string.Equals(d.Name, declarationName, StringComparison.OrdinalIgnoreCase));

        if (declarationQuery.Any())
        {
            var declaration = declarationQuery.First();
            return declaration.Value;
        }

        return "";
    }


    private static int ParseCSSFontSize(string term)
    {
        if (term.EndsWith("pt"))
        {
            // remove that part
            term = term.Substring(0, term.Length - 2);
        }

        if (int.TryParse(term, out int fontSizeNumber))
        {
            return fontSizeNumber;
        }
        else
        {
            throw new Exception($"CSS Font Size Value: [{term}] is not a valid integer, and did not end with 'pt'");
        }
    }

    private static styleModel.Style ConvertFromCSSToReportStyle(lowLevelModel.StyleClass rule)
    {
        var result = new styleModel.Style();

        result.name = rule.selector.Substring(1); // we don't want the . in the name.  A css style rule is ".<RuleName>"

        foreach (var decleration in rule.declarations)
        {
            switch (decleration.Name.ToLower())
            {
                case "color":
                    result.fontColor.Set(decleration.Value);
                    break;
                case "background-color":
                    result.backgroundColor.Set(decleration.Value);
                    break;
                case "text-align":
                    result.horizontalAlign.Set((styleModel.HorizontalAlignment)Enum.Parse(typeof(styleModel.HorizontalAlignment), decleration.Value));
                    break;
                case "font-weight":
                    result.fontWeight.Set((styleModel.FontWeight)Enum.Parse(typeof(styleModel.FontWeight), decleration.Value));
                    break;
                case "font-family":
                    // make sure it's trim and then also trim '\'' so it's not like " 'Arial Black' "
                    var family = decleration.Value.Trim();
                    family = family.Trim('\'');
                    result.fontFamily.Set(family);
                    break;
                case "font-size":
                    result.fontSize.Set(ParseCSSFontSize(decleration.Value));
                    break;
                case "vertical-align":
                    result.verticalAlign.Set((styleModel.VerticalAlignment)Enum.Parse(typeof(styleModel.VerticalAlignment), decleration.Value));
                    break;
                case "margin":
                    result.margin.Set(ParseMargin(decleration));
                    break;
                case "border":
                    result.border.Set(ParseBorder(decleration));
                    break;
                case "text-decoration":
                    result.textDecoration.Set(ParseTextDecoration(decleration));
                    break;

            }
        }

        return result;
    }



    private static styleModel.FontTextDecoration ParseTextDecoration(lowLevelModel.Declaration decleration)
    {
        // css documentation here: http://www.w3schools.com/cssref/pr_text_text-decoration.asp
        // enums can't have dashes so we do a comparison to maintain compatibility with css
        if (string.Equals(decleration.Value, "line-through", StringComparison.OrdinalIgnoreCase))
        {
            return styleModel.FontTextDecoration.lineThrough;
        }
        else
        {
            return (styleModel.FontTextDecoration)Enum.Parse(typeof(styleModel.FontTextDecoration), decleration.Value);
        }
    }

    private static styleModel.Margin ParseMargin(lowLevelModel.Declaration marginProperty)
    {
        var margin = new styleModel.Margin();

        var values = marginProperty.Value.Split()
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(t =>
            {
                t = t.Trim();
                if (t.EndsWith("px"))
                {
                    t = t.Substring(0, t.Length - 2);
                }

                if (float.TryParse(t, out float termValue))
                {
                    return termValue;
                }
                else
                {
                    throw new Exception($"ParseMargin Margin Value: [{t}] is not a valid floating point value");
                }
            })
            .ToArray();

        // see: http://www.w3schools.com/css/css_margin.asp
        // order for css is 
        // if 4 then [top,right,bottom,left]
        // if 3 then [top, right/left, bottom]
        // if 2 then [top/bottom, right/left]
        switch (values.Length)
        {
            case 4:
                margin.Top = values[0];
                margin.Right = values[1];
                margin.Bottom = values[2];
                margin.Left = values[3];
                break;
            case 3:
                margin.Top = values[0];
                margin.Right = margin.Left = values[1];
                margin.Bottom = values[2];
                break;
            case 2:
                margin.Top = margin.Bottom = values[0];
                margin.Right = margin.Left = values[1];
                break;
            case 1:
                margin.Left = margin.Top = margin.Right = margin.Bottom = values[0];
                break;
        }

        return margin;
    }


    private static void SetBorderFromTerm(string term, styleModel.Border border)
    {
        // see if it's a pixel border width
        if (term.EndsWith("px"))
        {
            // take all but last characters
            var numberPart = term.Substring(0, term.Length - 2);
            if (int.TryParse(numberPart, out int borderWidth))
            {
                border.Width = borderWidth;
            }
            else
            {
                throw new Exception($"Number part of pixel border term: [{numberPart}] is not valid");
            }
        }
        // see if it's a style by parsing, and if it's not in the list see if it's a color
        else if (Enum.TryParse<styleModel.BorderStyle>(term, ignoreCase: true, result: out styleModel.BorderStyle borderStyle))
        {
            border.Style = borderStyle;
        }
        else
        {
            // if it wasn't a border style it has to be a color
            border.Color = term;
        }
    }

    private static styleModel.Border ParseBorder(lowLevelModel.Declaration borderProperty)
    {
        var border = new styleModel.Border();

        /*
         in css the border is can have examples like this
            '5px solid red'
            'red'
            'double'
         */
        var borderTerms = borderProperty.Value.Split() // this should split on whitespace
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(t => t.Trim());

        // css border property documentation is available here: http://www.w3schools.com/cssref/pr_border.asp
        foreach (var term in borderTerms)
        {
            SetBorderFromTerm(term, border);
        }

        return border;
    }







}
