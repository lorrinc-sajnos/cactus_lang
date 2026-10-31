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
        ID = FileID.FromAbsolutePath(source.Path,grove);
        Filename = source.Path;
        Grove = grove;

        IncludeManager = new(this);
        ErrorHandler = new(this);
        TypeTable = new(this);
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
    public OrderedDictionary<string, FileStruct> Structs { get; } = new();

    public TypeTable TypeTable { get;  }
    
    public TagContainer TagContainer { get; } = new ();

    
    public ErrorHandler ErrorHandler { get; }
    
    public void AddStruct(FileStruct fileStruct) => Structs.Add(fileStruct.Name, fileStruct);
    
    public void AddTags(List<Tag> tagList) {
        foreach (var tag in tagList) {
            tag.OnDeclared(this);
        }
        TagContainer.AddTags(tagList);
    }
    public FileStruct GetStruct(string name) => Structs[name];
    public List<FileStruct> GetStructs() => Structs.Values.ToList();

    public bool HasErrors() => ErrorHandler.HasErrors();

    public List<string> CFilesGenerated { get; } = new();
    public List<string> HFilesGenerated { get; } = new();

    public MemberFunction? GetExactFunction(FunctionSymbol id) => Functions.GetExactFunction(id);

    public bool ContainsExactFunction(FuncSignature id) => Functions.ContainsExactFunction(id);

    public MemberFunction? GetExactFunction(FuncSignature sign) => Functions.GetExactFunction(sign);

    public MemberFunction? GetMatchingFunction(FuncSignature id) => Functions.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncSignature id) => Functions.ContainsMatchingFunction(id);

    public FunctionSymbol? GetMatchingId(FuncSignature signature) => Functions.GetMatchingId(signature);

    public List<MemberFunction> GetFunctions() => Functions.GetFunctions().ConvertAll(MemberFunction (x) =>x);
    public MemberField? GetField(string name) => Fields.GetField(name);

    public List<MemberField> GetFields() => Fields.GetFields().ConvertAll(MemberField (x) =>x);

    public string Name => Filename;

    public FileID ParentID => throw new NotImplementedException();

    public BaseType MemberType => throw new NotImplementedException();
}