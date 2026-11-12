using System.Numerics;
using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Util;

public static class IntegerParser {
    public static BigInteger? Parse(string input) {
        string prefix = "";

        if (input.Length > 2)
            prefix = input[0..2];

        try {
            switch (prefix) {
                default:
                    return BigInteger.Parse(input);
                case LiteralIntegerType.INT_PREFIX_HEX:
                    return ParseToBaseTwoPow(input[2..], 4);
                case LiteralIntegerType.INT_PREFIX_BIN:
                    return ParseToBaseTwoPow(input[2..], 1);
                case LiteralIntegerType.INT_PREFIX_OCT:
                    return ParseToBaseTwoPow(input[2..], 3);
            }
        }
        catch (Exception) {
            return null;
        }
    }

    public static List<BaseType> GetLiteralTypes(BigInteger value) {
        List<BaseType> types = new();

        //8-bit
        if (value >= byte.MinValue && value <= byte.MaxValue)
            types.Add(NumberType.U08);
        if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            types.Add( NumberType.I08);
        //16-bit
        if (value >= ushort.MinValue && value <= ushort.MaxValue)
            types.Add(NumberType.U16);
        if (value >= short.MinValue && value <= short.MaxValue)
            types.Add( NumberType.I16);
        //32-bit
        if (value >= uint.MinValue && value <= uint.MaxValue)
            types.Add( NumberType.U32);
        if (value >= int.MinValue && value <= int.MaxValue)
            types.Add( NumberType.I32);
        //64-bit
        if (value >= ulong.MinValue && value <= ulong.MaxValue)
            types.Add( NumberType.U64);
        if (value >= long.MinValue && value <= long.MaxValue)
            types.Add( NumberType.I64);

        return types;
    }

    private static BigInteger? ParseToBaseTwoPow(string input, int basePow) {
        BigInteger value = 0;

        for (int i = 0; i < input.Length; i++) {
            char digit = input[i];
            int digVal = GetDigitValue(digit);
            if (digVal == -1) return null;
            value <<= basePow;
            value |= digVal;
        }

        return value;
    }

    private static int GetDigitValue(char digit) {
        if (digit >= '0' && digit <= '9')
            return digit - '0';

        char lwr = char.ToLower(digit);

        if (lwr >= 'a' && lwr <= 'f')
            return lwr - 'a' + 10;
        return -1;
    }
}