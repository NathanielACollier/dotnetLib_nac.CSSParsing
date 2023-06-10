using nac.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nac.CSSParsing.model.Styling;

public class Style
{
    public string name { get; set; }
    public Optional<FontWeight> fontWeight { get; set; }
    public Optional<FontTextDecoration> textDecoration { get; set; }
    public Optional<int> fontSize { get; set; }
    public Optional<string> fontFamily { get; set; }
    public Optional<string> fontColor { get; set; }
    public Optional<string> backgroundColor { get; set; }

    public Optional<HorizontalAlignment> horizontalAlign { get; set; }
    public Optional<VerticalAlignment> verticalAlign { get; set; }

    public Optional<Margin> margin { get; set; }
    public Optional<Border> border { get; set; }

    public Style()
    {
        this.fontWeight = new Optional<FontWeight>();
        this.fontSize = new Optional<int>();
        this.fontFamily = new Optional<string>();
        this.fontColor = new Optional<string>();
        this.horizontalAlign = new Optional<HorizontalAlignment>();
        this.verticalAlign = new Optional<VerticalAlignment>();
        this.margin = new Optional<Margin>();
        this.backgroundColor = new Optional<string>();
        this.border = new Optional<Border>();
        this.textDecoration = new Optional<FontTextDecoration>();
    }


    public void SetUnsetFromOtherStyle(Style _s)
    {
        var properties = typeof(Style).GetProperties();
        var optionalProperties = properties.Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Optional<>));
        foreach (var prop in optionalProperties)
        {
            dynamic ourValue = prop.GetValue(this);
            dynamic theirValue = prop.GetValue(_s);

            if (ourValue.IsSet == false && theirValue.IsSet == true)
            {
                ourValue.Set(theirValue.Value);
            }
        }
    }





}
