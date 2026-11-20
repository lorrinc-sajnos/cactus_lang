using CactusLang.Model;
using CactusLang.Model.CodeStructure.Entities.Structs;
using CactusLang.Model.Errors;
using CactusLang.Model.Types;
using CactusLang.Model.Types.EntityTypes;
using CactusLang.Model.Types.PrimitiveTypes;
using CactusLang.Semantics.Errors;
using GP = GrammarParser;

namespace CactusLang.Semantics;

//

public class FileTypeTable {
    private readonly CactusFile _file;
    private readonly ErrorHandler _errorHandler;
    private readonly OrderedDictionary<string, CactusType> _types;
    private readonly OrderedDictionary<string, CactusStruct> _structs;
    
    public FileTypeTable(CactusFile file) {
        _file = file;
        _errorHandler = file.ErrorHandler;
        _types = new();
        _structs = new();
        AddPrimitives();
    }
    
    public CactusType? GetFromName(string rawText) {
        //Works, because * can only be at the end of the type
        int ptrLvl = rawText.Count(c => c == '*');
        string trueName = rawText.Substring(0, rawText.Length - ptrLvl);

        if (!_types.TryGetValue(trueName, out var type))
            return null;

        //If it is a pointer:
        if (ptrLvl > 0)
            return PointerType.CreatePointer(type, ptrLvl);
        return type;
    }

    public CactusType GetFromCtx(GP.TypeContext ctx) {
        //Resolve, if type is included from a different file:
        string rawText = ctx.GetText();
        if (ctx.parentObj() != null && ctx.parentObj().Length > 0) {
            var parentChain = ctx.parentObj().ToList();
            return GetIncludedType(parentChain, ctx);
        }

        var type = GetFromName(rawText);
        if (type == null)
            return _errorHandler.ErrorInType(CactusError.TYPE_NOT_FOUND, ctx, ctx.GetText());

        return type;
    }

    private CactusType GetIncludedType(List<GP.ParentObjContext> parentChain, GP.TypeContext ctx) {
        //TODO currently only first
        var currObj = parentChain[0];
        var parentName = parentChain[0].GetText();
        var parentFile = _file.IncludeManager.GetIncludedFile(parentName);
        if (parentFile == null)
            return _errorHandler.ErrorInType(CactusError.FILE_DOENST_EXISTS, currObj, currObj.GetText());

        string rootTxt = ctx.root().GetText();
        int ptrLvl = rootTxt.Count(c => c == '*');
        var key = rootTxt[..^ptrLvl];

        var innerMemHolder = parentFile.GetMemberHolder(key);

        if (innerMemHolder == null)
            return _errorHandler.ErrorInType(CactusError.TYPE_NOT_FOUND, ctx, ctx.GetText());

        var innerType = innerMemHolder.MemberType;
        if (ptrLvl > 0)
            return PointerType.CreatePointer(innerType, ptrLvl);
        return innerType;
    }

    //Primitives
    private void AddPrimitives() {
        foreach (var primitive in NumberType.GetPrimitives()) {
            _types.Add(primitive.Name, primitive);
        }
    }

    #region Structs    
    
    public bool DeclareStruct(CactusStruct cactusStruct) {
        //string structName = ctx.structName().GetText();
        EntityType structType = cactusStruct.StructType;

        if (!_types.TryAdd(structType.Name, structType)) {
            return false;
        }
        
        _structs.Add(structType.Name, cactusStruct);
        return true;
    }

    public CactusStruct? GetDeclStruct(string name) => _structs.GetValueOrDefault(name);
    public CactusStruct? GetStructFromTokens(GP.StructDclContext ctx) => GetDeclStruct(ctx.structName().GetText());

    #endregion

}