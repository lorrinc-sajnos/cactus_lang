using CactusLang.Util;

namespace CactusLang.Model.Types;

public class ErrorType : BaseType {
    public override string Name => "error";
    public override int Size => 0;
    private ErrorType() { }
    public static readonly ErrorType ERROR = new();

    public override BaseType GetPointer() => ERROR;

    protected override bool CanImplicitCastInto(BaseType other) {
        return false;
    }

    public override string CName =>
        throw new ImpossibleException("Code Generation should never reach an ErrorException");
}