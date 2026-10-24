using CactusLang.Compiling.Codefiles;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Symbols;
using CactusLang.Semantics;
using CactusLang.Semantics.Errors;
using CactusLang.Tags;

namespace CactusLang.Model;

public class CactusFile {
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

    public IncludeManager IncludeManager { get; }
    public OrderedDictionary<string, FileStruct> Structs { get; } = new();

    public TypeTable TypeTable { get;  }
    
    public TagContainer TagContainer { get; } = new ();

    
    public ErrorHandler ErrorHandler { get; }
    
    public void AddStruct(FileStruct fileStruct) => Structs.Add(fileStruct.Name, fileStruct);
    public CactusFile AddFile(CactusFile file) {
        _includedFiles.Add(file.FileID, file);
        return file;
    }
    public CactusFile GetFile(FileID fileID) => _includedFiles[fileID];

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
}