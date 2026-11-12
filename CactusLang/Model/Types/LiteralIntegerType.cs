using System.Numerics;
using CactusLang.Util;

namespace CactusLang.Model.Types;

/// <summary>
/// This is the type of Integer literals. These are neccecary, because literals value matters when assigning, ie. 2 can be used as u08 and i08.
/// </summary>
public class LiteralIntegerType : BaseType {
    private List<BaseType> _canBeUsedAs;

    public BigInteger Value { get; }
    public override string Name => $"Literal Integer ({Value})";
    public override int Size { get; }
    
    public const char FLOAT_POSTFIX_SINGLE = 'f';
    public const char FLOAT_POSTFIX_DOUBLE = 'd';

    public const string INT_PREFIX_BIN = "0b";
    public const string INT_PREFIX_HEX = "0x";
    public const string INT_PREFIX_OCT = "0o";

    public override bool IsInteger => true;
    public override bool IsNumber => true;
    public override bool IsUnsigned => Value < 0;


    public LiteralIntegerType(BigInteger value, string? sign)  {
        if (sign is "-")
            value *= -1;
        Value = value;
        _canBeUsedAs = IntegerParser.GetLiteralTypes(value);
    }

    //Maybe slow but it runs
    public override bool CanImplicitCastInto(BaseType superiorType) {
        return _canBeUsedAs.Any(possibleType => possibleType.CanBeUsedAs(superiorType));
    }

    //public override string CName => throw new ImpossibleException("CodeGenerator should never reach a literal type");
    //public override string CName => Value.ToString();
}