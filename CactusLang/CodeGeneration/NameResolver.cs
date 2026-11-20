using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.FileContent;

using CactusLang.Model.CodeStructure.Entities;
using CactusLang.Model.CodeStructure.Entities.Structs;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.EntityTypes;
using CactusLang.Model.Types.PrimitiveTypes;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public class NameResolver {
    CactusGrove _cactusGrove;
    private CactusFile _currFile;
    private string PREFIX { get; }

    public NameResolver(CactusFile file) {
        _cactusGrove = file.Grove;
        _currFile = file;
        PREFIX = GetFile(_currFile);
    }

    public static string Sanitize(string str) =>
        str.Replace('*', 'X')
            .Replace("#", "_OBJ");

    #region Constants

    public const string PARAM_SEP = ", ";

    #endregion

    public string GetFileID(FileID id) => String.Join("_F_", id.Folders) + id.Filename + "_FF_";
    public string GetFile(CactusFile file) => GetFileID(file.ID);


    public string GetFileIdAsHeaderFile(FileID id) => id.AsRelPath() + ".h";
    public string GetFileAsHeaderFile(CactusFile file) => GetFileIdAsHeaderFile(file.ID);

    public string GetFileIdAsHeaderGuard(FileID id) => id.AsRelPath('_').ToUpper() + "_H";
    public string GetFileAsHeaderGuard(CactusFile file) => GetFileIdAsHeaderGuard(file.ID);


    public static string GetParamPostfix(CactusFunction func) {
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count <= 0) return "";

        return "_" + String.Join("_", parameters.Select(p => Sanitize(p.Type.Name)));
    }

    public string GetFileFunc(FileFunction func) =>
        GetFile(func.CactusFile) + Sanitize(func.Name) + GetParamPostfix(func);

    public string GetFileField(FileVariable variable) => GetFile(variable.CactusFile) + variable.Name;
    
    public string GetMemberField(CactusField variable) => GetMemberHolderPrefix(variable.Parent) + variable.Name;
    private string GetStructFunc(StructMethod func) => func.Name + GetParamPostfix(func);

    public string GetStructRefFunc(StructMethod func) =>
        $"{GetBaseType(func.Parent.MemberType)}_ref__{GetStructFunc(func)}";

    public string GetStructAccFunc(StructMethod func) =>
        $"{GetBaseType(func.Parent.MemberType)}_acc__{GetStructFunc(func)}";


    public string GetFileRefFunc(IncludedFileRef fileRef, StructMethod func) =>
        $"{GetFile(fileRef.IncFile.CactusFile)}{GetFunc(func)}";


    public string GetBaseType(CactusType type) {
        switch (type) {
            case EntityType memType:
                return GetMemberHolderName(memType.Parent);
            case PointerType pointerType:
                return $"{GetBaseType(pointerType.PointsTo)}*";
            case PrimitiveType pType:
                return pType.Name;
            default:

                return type.Name;
        }
    }

    #region Struct

    public string GetStructMethodHeader(StructMethod func) {
        string header = "";

        header += GetBaseType(func.ReturnType) + " ";
        //In C, no local functions, so file function w/ custom name:
        header += GetStructRefFunc(func);
        //Introduce the "this" pointer as first variable
        header += $"({GetBaseType(func.Parent.MemberType)}* this" + GetFuncDclParams_NO_OPEN(func);

        return header;
    }

    public string GetStructAccFuncHeader(StructMethod func) {
        string header = "";

        header += GetBaseType(func.ReturnType) + " ";
        //In C, no local functions, so file function w/ custom name:
        header += GetStructAccFunc(func);
        //Introduce the "thisVal" as first variable
        header += $"({GetBaseType(func.Parent.MemberType)} thisVal{GetFuncDclParams_NO_OPEN(func)}";

        return header;
    }

    public List<string> GetStructAccFuncBody(StructMethod func) {
        List<string> body = new List<string>();

        body.Add($"{GetBaseType(func.Parent.MemberType)}* this = &thisVal;");
        body.Add($"return {GetStructRefFunc(func)}(this{GetFuncCallParams_NO_OPEN(func)};");

        return body;
    }

    public string GetStructCID(CactusStruct cctsStruct) {
        return "";

    }
    

    public string GetFileFunctionHeader(FileFunction func) {
        string header = "";

        header += GetBaseType(func.ReturnType) + " ";
        header += GetFileFunc(func);
        header += "(" + GetFuncDclParams_NO_OPEN(func);
        return header;
    }

    public string GetFuncDclParams_NO_OPEN(CactusFunction func) {
        string header = "";
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count > 0) {
            //Strunc functions first param is the struct itself
            if (func is StructMethod)
                header += PARAM_SEP;

            for (int i = 0; i < parameters.Count - 1; i++) {
                var param = parameters[i];
                header += $"{GetBaseType(param.Type)} {param.CName}" + PARAM_SEP;
            }

            header += $"{GetBaseType(parameters[^1].Type)} {parameters[^1].CName}";
        }

        header += ")";
        return header;
    }

    public string GetFuncCallParams_NO_OPEN(CactusFunction func) {
        string header = "";
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count > 0) {
            //Strunc functions first param is the struct itself
            if (func is StructMethod)
                header += PARAM_SEP;

            for (int i = 0; i < parameters.Count - 1; i++) {
                var param = parameters[i];
                header += $"{param.CName}" + PARAM_SEP;
            }

            header += $"{parameters[^1].CName}";
        }

        header += ")";
        return header;
    }

    #endregion


    #region Methods

    public string GetFunc(CactusFunction mFunc) {
        switch (mFunc) {
            case FileFunction fileFunc:
                return GetFileFunc(fileFunc);
            case CactusMethod memFunc:
                return GetMemberFunction(memFunc);
            default:
                return mFunc.Name;
        }
    }

    public string GetMemberFunction(CactusMethod mFunc) =>
        GetPrefix(mFunc.Parent) + Sanitize(mFunc.Name) + GetParamPostfix(mFunc);

    public string GetMemberHolderName(IEntity memHolder) => GetFileID(memHolder.ParentID) + GetPrefix(memHolder);

    public string GetPrefix(IEntity holder) {
        switch (holder) {
            case CactusFile file:
                return GetFile(file);
            default:
                return GetMemberHolderPrefix(holder);
        }
    }

    public string GetMemberHolderPrefix(IEntity holder) => $"M{holder.Name}_";

    #endregion

    public string GetLocalVar(LocalVarRef structFieldRef) => structFieldRef.VarName;

    public string GetField(CactusField variableRefVariable) => variableRefVariable.Name;
}