using CactusLang.CodeGeneration.CCodeGenerator;
using CactusLang.Model;

namespace CactusLang.CodeGeneration;

public class GroveGenerator {
    public GroveGenerator(CactusGrove grove) {
        Grove = grove;

        string headerPath = $"{Grove.RootFolder}/src/type_table.h";
        TypeTable_H = new CFile(headerPath);
    }

    public CactusGrove Grove { get; }


    public void GenerateSourceFiles() {
        GenerateCactusFiles();
        GenerateTypeTableFile();
    }

    private void GenerateCactusFiles() {
        var mainCodeGenerator = new CactusFileGenerator(this, Grove.RootFile, Grove.RootFile.SourceFile.Path);

        mainCodeGenerator.GenerateEntryFileCode();
        mainCodeGenerator.GenerateFiles();

        var otherFiles = Grove.GetFiles().ToList();
        otherFiles.Remove(Grove.RootFile);
        foreach (var file in otherFiles) {
            var codeGenerator = new CactusFileGenerator(this, file, file.SourceFile.Path);

            codeGenerator.GenerateCode();
            codeGenerator.GenerateFiles();
        }
    }

    public CFile TypeTable_H { get; }

    private void GenerateTypeTableFile() {
        TypeTable_H.AddAsMultipleLines(
            CodeScope.Location.TOP,
            $$""""
              #ifndef __CCTS_TYPE_TABLE_H
              #define __CCTS_TYPE_TABLE_H
              """"
        );
        
        //Collect structs
        foreach (var structDef in Grove.GetAllStructs()) {
            
        }
        
        
        TypeTable_H.AddLineBot("#endif");
        
        TypeTable_H.WriteToFile();
    }

    public List<CFile> CFilesGenerated { get; } = [];
    public List<CFile> HFilesGenerated { get; } = [];
}