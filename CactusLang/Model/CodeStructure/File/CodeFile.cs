using CactusLang.Model.Codefiles;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Tags;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure.File;

public class CodeFile(CodeSourceFile source) {
    public string Filename { get; } = source.Path;

    public FieldStore<FileField> Fields { get; } = new ();

    public FunctionStore<FileFunction> Functions { get; } = new ();

    public OrderedDictionary<string, FileStruct> Structs { get; } = new();

    public TagContainer TagContainer { get; } = new ();

    public void AddStruct(FileStruct fileStruct) => Structs.Add(fileStruct.Name, fileStruct);

    public void AddTags(List<Tag> tagList) {
        foreach (var tag in tagList) {
            tag.OnDeclared(this);
        }
        TagContainer.AddTags(tagList);
    }
    public FileStruct GetStruct(string name) => Structs[name];
    public List<FileStruct> GetStructs() => Structs.Values.ToList();
    
}