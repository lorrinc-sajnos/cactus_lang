using CactusLang.Model;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public static class SemanticUtil {
    
    public static FuncCandidate GetSignatureFromDclCtx(GP.FuncDclHeaderContext ctx, TypeTable typeTable) {
        string funcName = ctx.funcName().GetText();
        List<BaseType> paramTypes = new();
        foreach(var param in ctx.param()) 
            paramTypes.Add(typeTable.GetFromCtx(param.type()));
        
        var returnType = typeTable.GetFromCtx( ctx.returnType().type());

        return new FuncCandidate(funcName, paramTypes, returnType);
    }
    
    
    public static List<VariableSymbol> GetVarSymbols(GrammarParser.VarDclContext varCtx, TypeTable typeTable) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = typeTable.GetFromCtx(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }
}