using CactusLang.Model;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public static class SemanticUtil {
    
    public static FuncCandidate GetSignatureFromDclCtx(GP.FuncDclHeaderContext ctx, FileTypeTable fileTypeTable) {
        string funcName = ctx.funcName().GetText();
        List<CactusType> paramTypes = new();
        foreach(var param in ctx.param()) 
            paramTypes.Add(fileTypeTable.GetFromCtx(param.type()));
        
        var returnType = fileTypeTable.GetFromCtx( ctx.returnType().type());

        return new FuncCandidate(funcName, paramTypes, returnType);
    }
    
    
    public static List<VariableSymbol> GetVarSymbols(GrammarParser.VarDclContext varCtx, FileTypeTable fileTypeTable) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        CactusType type = fileTypeTable.GetFromCtx(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }
}