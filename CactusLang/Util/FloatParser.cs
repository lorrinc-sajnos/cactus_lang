using System.Globalization;
using CactusLang.Model.Types;

namespace CactusLang.Util;

public enum FloatType {
    NAN, SINGLE, DOUBLE
}

public struct FloatLiteralResult {
    public double Value { get; private set; }
    public float FloatValue { get; private set; }
    public double DoubleValue { get; private set; }
    public FloatType Type { get; private set; }

    public FloatLiteralResult(float value) {
        Value = value;
        Type = FloatType.SINGLE;
        FloatValue = value;
        DoubleValue = double.NaN;
    }
    public FloatLiteralResult(double value) {
        Value = value;
        Type = FloatType.DOUBLE;
        FloatValue = float.NaN;
        DoubleValue = value;
    }


    // ReSharper disable once InconsistentNaming
    public static FloatLiteralResult NAN = new FloatLiteralResult() {
        Type = FloatType.NAN, 
        Value = double.NaN, 
        FloatValue = float.NaN ,  
        DoubleValue = double.NaN 
    };
}

public static class FloatParser {
    public static FloatLiteralResult Parse(string rawText) {
        FloatType type =  FloatType.NAN;
        string text = rawText;
        char lastChar = text.ToLower()[^1];
        
        if(lastChar==PrimitiveType.FLOAT_POSTFIX_SINGLE) {
            type = FloatType.SINGLE;
            text = rawText.Substring(0, text.Length - 1);
        }
        else if(lastChar==PrimitiveType.FLOAT_POSTFIX_DOUBLE) {
            type = FloatType.DOUBLE;
            text = rawText.Substring(0, text.Length - 1);
        }

        double value;
        try {
            value = double.Parse(text, CultureInfo.InvariantCulture);
        }
        catch (Exception) {
            return FloatLiteralResult.NAN;
        }

        switch (type) {
            case  FloatType.SINGLE:
                return new FloatLiteralResult((float)value);
            case  FloatType.DOUBLE:
                return new FloatLiteralResult((double)value);
            default:
                return FloatLiteralResult.NAN;
        }
        
    }
}
