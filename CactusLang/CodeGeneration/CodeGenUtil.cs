using System.Runtime.InteropServices.JavaScript;
using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public static class CodeGenUtil {
    #region Code templates

    public const string STRUCT_REF_FUNC_HEADER_FORMAT = "{0}_ref__{1}";
    public const string STRUCT_ACC_FUNC_HEADER_FORMAT = "{0}_acc__{1}";
    public const string PARAM_SEP = ", ";

    
    #endregion

    
    
    public static string GetStructRefFuncName(StructFunction func) => $"{func.Parent.MemberType.CName.ToLower()}_ref__{func.CName}";

    public static string GetStructAccFuncName(StructFunction func) => $"{func.Parent.MemberType.CName.ToLower()}_acc__{func.CName}";

    #region Util

    public static string GetStructRefFuncHeader(StructFunction func) {
        string header = "";

        header += func.ReturnType.CName + " ";
        //In C, no local functions, so file function w/ custom name:
        header += GetStructRefFuncName(func);
        //Introduce the "this" pointer as first variable
        header += $"({func.Parent.MemberType.CName}* this" + GetFuncDclParams_NO_OPEN(func);

        return header;
    }

    public static string GetStructAccFuncHeader(StructFunction func) {
        string header = "";

        header += func.ReturnType.CName + " ";
        //In C, no local functions, so file function w/ custom name:
        header += GetStructAccFuncName(func);
        //Introduce the "thisVal" as first variable
        header += $"({func.Parent.MemberType.CName} thisVal{GetFuncDclParams_NO_OPEN(func)}";

        return header;
    }

    public static List<string> GetStructAccFuncBody(StructFunction func) {
        List<string> body = new List<string>();

        body.Add($"{func.Parent.MemberType.CName}* this = &thisVal;");
        body.Add($"return {GetStructRefFuncName(func)}(this{GetFuncCallParams_NO_OPEN(func)};");

        return body;
    }

    public static string GetFileFunctionHeader(FileFunction func) {
        string header = "";

        header += func.ReturnType.CName + " ";
        header += func.CName;
        header += "(" + GetFuncDclParams_NO_OPEN(func);
        return header;
    }

    public static string GetFuncDclParams_NO_OPEN(ModelFunction func) {
        string header = "";
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count > 0) {
            //Strunc functions first param is the struct itself
            if (func is StructFunction)
                header += PARAM_SEP;

            for (int i = 0; i < parameters.Count - 1; i++) {
                var param = parameters[i];
                header += $"{param.Type.CName} {param.CName}" + PARAM_SEP;
            }

            header += $"{parameters[^1].Type.CName} {parameters[^1].CName}";
        }

        header += ")";
        return header;
    }

    public static string GetFuncCallParams_NO_OPEN(ModelFunction func) {
        string header = "";
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count > 0) {
            //Strunc functions first param is the struct itself
            if (func is StructFunction)
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

    public static string GetVarDclString(VarDclStatement varDclStatement) {
        string result = "";
        result += $"{varDclStatement.VarType.CName} ";
        var dclBodies = varDclStatement.GetBodies();
        UtilFunc.SeparatedForEach(dclBodies,
            (body) => {
                result += body.Variable.CName;
                if (body.HasValue) {
                    string varVal = ExpressionCodeFactory.Manufacture(body.Value!);
                    result += $" = {varVal}";
                }
            },
            (_,_) => result += PARAM_SEP
        );
        result += ";";
        return result;
    }

    private const string C_INCLUDE = "#include ";
    
    
    

    #endregion
}