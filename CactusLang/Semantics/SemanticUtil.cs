using CactusLang.Model;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public static class SemanticUtil {
    
    public static FuncSignature GetSignatureFromDclCtx(GP.FuncDclHeaderContext ctx, CactusFile file) {
        string funcName = ctx.funcName().GetText();
        List<BaseType> paramTypes = new();
        foreach(var param in ctx.param()) {
            
            paramTypes.Add(file.TypeTable.Get(param.type()) );
        }

        return new FuncSignature(funcName, paramTypes);
    }
    
    
    public static List<VariableSymbol> GetVarSymbols(GrammarParser.VarDclContext varCtx, TypeTable typeTable) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = typeTable.Get(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }
}