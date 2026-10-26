using CactusLang.Compiling.Codefiles;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Errors;
using CactusLang.Tags;

namespace CactusLang.Model;

public class CactusFile : IMemberHolder {
    public CactusFile(CactusGrove grove, SourceFile source) {
        SourceFile = source;
        FileID = FileID.FromSrcFile(source,grove);
        Filename = source.Path;
        Grove = grove;
        Grove.AddFile(this);


        ErrorHandler = new(this);
        TypeTable = new(this);
    }

    public SourceFile SourceFile { get; }
    public FileID FileID { get; }
    public string Filename { get; }
    public CactusGrove Grove { get; }
    
    public FieldStore<FileField> Fields { get; } = new ();
    public FunctionStore<FileFunction> Functions { get; } = new ();

    public IncludeManager IncludeManager { get; } = new();
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
    
    public MemberFunction? GetExactFunction(FunctionSymbol id) {
        throw new NotImplementedException();
    }

    public bool ContainsExactFunction(FuncSignature id) {
        throw new NotImplementedException();
    }

    public MemberFunction? GetExactFunction(FuncSignature sign) {
        throw new NotImplementedException();
    }

    public MemberFunction? GetMatchingFunction(FuncSignature id) {
        throw new NotImplementedException();
    }

    public bool ContainsMatchingFunction(FuncSignature id) {
        throw new NotImplementedException();
    }

    public FunctionSymbol? GetMatchingId(FuncSignature signature) {
        throw new NotImplementedException();
    }

    public List<MemberFunction> GetFunctions() {
        throw new NotImplementedException();
    }

    public MemberField? GetField(string name) {
        throw new NotImplementedException();
    }

    public List<MemberField> GetFields() {
        throw new NotImplementedException();
    }

    public string Name => throw new NotImplementedException();

    public FileID ParentID => throw new NotImplementedException();

    public BaseType MemberType => throw new NotImplementedException();
}