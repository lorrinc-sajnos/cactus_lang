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


    public override bool IsInteger => true;
    public override bool IsNumber => true;
    public override bool IsUnsigned => Value < 0;


    public LiteralIntegerType(BigInteger value, string? sign)  {
        Value = value;
        if (sign is "-")
            value *= -1;
        _canBeUsedAs = IntegerParser.GetLiteralTypes(value);
    }

    //Maybe slow but it runs
    protected override bool CanImplicitCastInto(BaseType superiorType) {
        return _canBeUsedAs.Any(possibleType => possibleType.CanBeUsedAs(superiorType));
    }

    public override string CName => throw new ImpossibleException("CodeGenerator should never reach a literal type");
}