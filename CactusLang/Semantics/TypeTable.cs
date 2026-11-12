using CactusLang.Model;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Errors;
using CactusLang.Model.Types;
using CactusLang.Model.Types.MemberHolderTypes;
using CactusLang.Model.Types.PrimitiveTypes;
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
        foreach (var primitive in NumberType.GetPrimitives()) {
            _types.Add(primitive.Name, primitive);
        }
    }

    public void AddStruct(GrammarParser.StructDclContext ctx, FileStruct fileStruct) {
        //string structName = ctx.structName().GetText();
        MemberHolderType structType = fileStruct.StructType;

        if (_types.ContainsKey(structType.Name)) {
            _errorHandler.Error(CctsError.ALREADY_DEFINED, ctx.structName());
            return;
        }

        _types.Add(structType.Name, structType);
    }

    public FileStruct GetStruct(GrammarParser.StructDclContext ctx) {
        string structName = ctx.structName().GetText();
        //TODO szebbé tenni
        return (FileStruct)(((MemberHolderType)_types[structName]).Parent);
    }

    public BaseType? GetFromName(string rawText)
    {
        //Works, because * can only be at the end of the type
        int ptrLvl = rawText.Count(c => c == '*');
        string trueName = rawText.Substring(0, rawText.Length - ptrLvl);
        
        if (!_types.ContainsKey(trueName))
            return null;
        

        var type = _types[trueName];

        //If it is a pointer:
        if (ptrLvl > 0)
            return PointerType.CreatePointer(type, ptrLvl);
        return type;
    }


    public BaseType GetFromCtx(GrammarParser.TypeContext ctx) {
        //Resolve, if type is included from a different file:
        string rawText = ctx.GetText();
        if (ctx.parentObj() != null && ctx.parentObj().Length>0)
        {
            var parentChain = ctx.parentObj().ToList();
            return GetIncludedType(parentChain, ctx);
        }
        var type = GetFromName(rawText);
        if  (type == null)
            return _errorHandler.ErrorInType(CctsError.TYPE_NOT_FOUND, ctx, ctx.GetText());
            
        return type;
    }

    private BaseType GetIncludedType(List<GrammarParser.ParentObjContext> parentChain, GrammarParser.TypeContext ctx)
    {
        //TODO currently only first
        var currObj = parentChain[0];
        var parentName = parentChain[0].GetText();
        var parentFile = _file.IncludeManager.GetIncludedFile(parentName);
        if (parentFile == null)
            return _errorHandler.ErrorInType(CctsError.FILE_DOENST_EXISTS, currObj, currObj.GetText());

        string rootTxt = ctx.root().GetText();
        int ptrLvl = rootTxt.Count(c => c == '*');
        var key = rootTxt[..^ptrLvl];
        
        var innerMemHolder = parentFile.GetMemberHolder(key);
        
        if (innerMemHolder == null)
            return _errorHandler.ErrorInType(CctsError.TYPE_NOT_FOUND, ctx, ctx.GetText());

        var  innerType = innerMemHolder.MemberType;
        if (ptrLvl > 0)
            return PointerType.CreatePointer(innerType, ptrLvl);
        return innerType;
    }
}