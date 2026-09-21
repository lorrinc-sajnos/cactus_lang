using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFuncCall : UnaryOp {
    public FileStruct Struct { get; }

    private StructFunction _function;
    public StructFunction Function => _function;

    public BaseType ReturnType => _function.ReturnType;


    public ObjectFuncCall(FileStruct referedStruct, StructFunction structFunc) : base(Side.RIGHT, structFunc.FuncId.ToString()) {
        Struct = referedStruct;
        _function = structFunc;
    }

    public override BaseType Evaluate(BaseType val) => ReturnType;
}