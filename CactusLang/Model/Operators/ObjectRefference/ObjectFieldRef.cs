using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFieldRef : UnaryOp {
    public FileStruct Struct { get; }

    private StructField _field;
    public StructField Field => _field;

    public ObjectFieldRef(FileStruct referedStruct, StructField field) : base(Side.RIGHT, field.Name) {
        Struct = referedStruct;
        _field = field;
    }

    public override BaseType Evaluate(BaseType val) => _field.Symbol.Type;
}