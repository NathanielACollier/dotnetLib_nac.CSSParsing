using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System;
using System.Linq;

namespace Tests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestBasicCSSParsing()
    {
        /*
         Figured this out by reading documentation here: https://github.com/TylerBrinks/ExCSS
         */
        var parser = new nac.CSSParsing.repos.CssParser();
        parser.AddCSSText(@"
                .duck{
                    color:red;
                    background-color:blue;
                    font-weight:bold;
                    font-size:15pt;
                }
            ");

        foreach (var styleClass in parser.Styles.Select(s => s.Value))
        {
            var selector = styleClass.Name;
            var declarations = styleClass.Attributes;

            // declarations = ExCSS.Property
        }
    }


    [TestMethod]
    public void classWithMultipleSelector()
    {
        var rules = nac.CSSParsing.repos.LowLevel_Shortcuts.ParseCSSTextGetClassList(@"
                .test1{
                    color:orange;
                    font-weight:bold;
                }
            ");

        Assert.IsTrue(rules.Count() > 0);
        Assert.IsTrue(rules.First().declarations.Count > 1);
    }


    [TestMethod]
    public void testParsing()
    {
        var rules = nac.CSSParsing.repos.LowLevel_Shortcuts.ParseCSSTextGetClassList(@"
                .inline {
                    text-decoration: strike-through
                }
            ");

        Assert.IsTrue(rules.Count() > 0);
        Assert.IsTrue(rules.First().declarations.Count > 0);
    }


    [TestMethod]
    public void parseCSSDeclarationsInsteadOfFullCSS()
    {
        // this is for when you are parsing like <span style='color:red;font-weight:bold;'></span>
        var style = nac.CSSParsing.StyleParsingHelper.ParseSingleCSSRule(
            "color:red;font-weight:bold;font-size:12pt;");

        Assert.IsTrue(style.fontColor == Color.Red);
    }


    [TestMethod]
    public void parseCSSBasic()
    {
        var style = nac.CSSParsing.StyleParsingHelper.ParseCSSRuleSet(@"
                .testClass1{
                    color:red;
                    font-weight:bold;
                    font-size:12pt;
                }
            ");

        Assert.IsTrue(style.First().fontColor == Color.Red);
        Assert.IsTrue(style.First().fontWeight == FontWeight.bold);
        Assert.IsTrue(style.First().fontSize == 12);
    }


    [TestMethod]
    public void ColorSimple()
    {
        var s = nac.CSSParsing.StyleParsingHelper.ParseSingleCSSRule("color:red");
        Assert.IsTrue(s.fontColor.Value == Color.Red);
    }


    [TestMethod]
    public void BorderSimple()
    {
        var s = nac.CSSParsing.StyleParsingHelper.ParseSingleCSSRule("border: 3px solid green");
        Assert.IsTrue(s.border.Value.Color == System.Drawing.Color.Green);
        Assert.IsTrue(s.border.Value.Width == 3);
        Assert.IsTrue(s.border.Value.Style == BorderStyle.Solid);
    }


    [TestMethod]
    public void TextDecoration()
    {
        var s = nac.CSSParsing.StyleParsingHelper.ParseSingleCSSRule("text-decoration: line-through");
        Assert.IsTrue(s.textDecoration == FontTextDecoration.lineThrough);
    }



    [TestMethod]
    public void FontFamilyNameParsing()
    {
        // with single quotes around it
        var s = nac.CSSParsing.StyleParsingHelper.ParseSingleCSSRule("font-family: 'arial black'");

        Assert.IsTrue(string.Equals(s.fontFamily.Value, "arial black", StringComparison.OrdinalIgnoreCase));

        // without single quotes
        s = nac.CSSParsing.StyleParsingHelper.ParseSingleCSSRule("font-family: times new roman");

        Assert.IsTrue(string.Equals(s.fontFamily.Value, "Times New Roman", StringComparison.OrdinalIgnoreCase));
    }

}