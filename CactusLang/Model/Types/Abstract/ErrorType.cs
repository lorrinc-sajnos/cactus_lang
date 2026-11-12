using CactusLang.Util;

namespace CactusLang.Model.Types.Abstract;

public class ErrorType : BaseType {
    public override string Name => "error";
    public override int Size => 0;

    public override bool IsAbstract => true;

    private ErrorType() { }
    public static readonly ErrorType ERROR = new();

    public override BaseType GetPointer() => ERROR;

    public override bool CanImplicitCastInto(BaseType other) {
        return false;
    }
}