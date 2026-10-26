using CactusLang.Model;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Errors;
using CactusLang.Model.Types;
using CactusLang.Semantics.Errors;

namespace CactusLang.Semantics;

//

public class TypeTable {
    private readonly CactusFile _file;
    private readonly ErrorHandler _errorHandler;
    private readonly OrderedDictionary<string, BaseType> _types;

    public TypeTable(CactusFile file) {
        _file = file;
        _errorHandler = file.ErrorHandler;
        _types = new();
        AddPrimitives();
    }

    //Primitives
    private void AddPrimitives() {
        foreach (var primitive in PrimitiveType.GetPrimitives()) {
            _types.Add(primitive.Name, primitive);
        }
    }

    public void AddStruct(GrammarParser.StructDclContext ctx, FileStruct fileStruct) {
        //string structName = ctx.structName().GetText();
        FileStruct.Type structType = fileStruct.StructType;

        if (_types.ContainsKey(structType.Name)) {
            _errorHandler.Error(CctsError.ALREADY_DEFINED, ctx.structName());
            return;
        }

        _types.Add(structType.Name, structType);
    }

    public FileStruct GetStruct(GrammarParser.StructDclContext ctx) {
        string structName = ctx.structName().GetText();

        return ((FileStruct.Type)_types[structName]).FileStruct;
    }


    public BaseType Get(GrammarParser.TypeContext ctx) {
        string rawText = ctx.GetText();

        //Works, because * can only be at the end of the type
        int ptrLvl = rawText.Count(c => c == '*');

        string trueName = rawText.Substring(0, rawText.Length - ptrLvl);

        if (!_types.ContainsKey(trueName)) {
            return _errorHandler.ErrorInType(CctsError.TYPE_NOT_FOUND, ctx, ctx.GetText());
        }

        var type = _types[trueName];

        //If it is a pointer:
        if (ptrLvl > 0)
            return PointerType.CreatePointer(type, ptrLvl);

        return type;
    }
}