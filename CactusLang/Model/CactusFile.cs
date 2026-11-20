using CactusLang.Compiling.Codefiles;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Stores;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Errors;
using CactusLang.Tags;

namespace CactusLang.Model;

public class CactusFile : IMemberHolder {
    private CactusFile(CactusGrove grove, SourceFile source) {
        SourceFile = source;
        ID = FileID.FromAbsolutePathDcl(source.Path,grove);
        Filename = source.Path;
        Grove = grove;

        IncludeManager = new(this);
        ErrorHandler = new(this);
    }

    public static CactusFile CreateAndAddFile(CactusGrove grove, SourceFile source) {
        var file = new CactusFile(grove, source);
        grove.AddFile(file);
        return file;
    }

    public SourceFile SourceFile { get; }
    public FileID ID { get; }
    public string Filename { get; }
    public CactusGrove Grove { get; }
    
    public FieldStore<FileField> Fields { get; } = new ();
    public FunctionStore<FileFunction> Functions { get; } = new ();
    public IncludeManager IncludeManager { get; }

    public DeclStructStore DeclStructStore { get; } = new ();
    
    public TagContainer TagContainer { get; } = new ();

    
    public ErrorHandler ErrorHandler { get; }
    
    
    public void AddTags(List<Tag> tagList) {
        foreach (var tag in tagList) {
            tag.OnDeclared(this);
        }
        TagContainer.AddTags(tagList);
    }
    
    public bool HasErrors() => ErrorHandler.HasErrors();
    
    #region MemberHolder Interface

    public MemberFunction? GetExactFunction(FunctionSymbol id) => Functions.GetExactFunction(id);

    public bool ContainsExactFunction(FuncCandidate id) => Functions.ContainsExactFunction(id);

    public MemberFunction? GetExactFunction(FuncCandidate sign) => Functions.GetExactFunction(sign);

    public MemberFunction? GetMatchingFunction(FuncCandidate id) => Functions.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncCandidate id) => Functions.ContainsMatchingFunction(id);

    public FunctionSymbol? GetMatchingId(FuncCandidate signature) => Functions.GetMatchingId(signature);

    public List<MemberFunction> GetFunctions() => Functions.GetFunctions().ConvertAll(MemberFunction (x) =>x);
    public MemberField? GetField(string name) => Fields.GetField(name);

    public List<MemberField> GetFields() => Fields.GetFields().ConvertAll(MemberField (x) =>x);
    
    #endregion

    public string Name => Filename;

    public FileID ParentID => throw new NotImplementedException();

    public BaseType MemberType => throw new NotImplementedException();

}