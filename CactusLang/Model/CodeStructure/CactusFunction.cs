using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.CodeStructure;

public abstract class CactusFunction {
    protected CodeBlock _codeBody;
    public CodeBlock CodeBody => _codeBody;
    
    protected FunctionSymbol _symbol;
    public FunctionSymbol Symbol => _symbol;
    
    public TagContainer TagContainer { get; } = new();
    public CactusFile CactusFile { get; protected set; }
    public string Name => _symbol.Name;
    public CactusType ReturnType => _symbol.ReturnType;


    protected CactusFunction(CactusFile cactusFile, FunctionSymbol symbol) {
        _symbol = symbol;
        _codeBody = new CodeBlock(this);
        CactusFile = cactusFile;
    }
    
    //public abstract string CName { get; }
}