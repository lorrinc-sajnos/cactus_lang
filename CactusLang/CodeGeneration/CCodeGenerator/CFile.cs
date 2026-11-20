using System.Diagnostics;

namespace CactusLang.CodeGeneration.CCodeGenerator;

public class CFile(string path) {

    public string Filename { get; } = Path.GetFileName(path);
    public string Filepath { get; } = path;
    public CodeScope CurrentScope { get; private set; } = new();

    # region Modification
    public void Add(string code) => CurrentScope.Add(code);
    
    public void AddLineTop(string line) => CurrentScope.AddLineTop(line);
    public void AddLineTop() => CurrentScope.AddLineTop();
    
    
    public void AddLine() => CurrentScope.AddLine();

    public void AddLine(CodeScope.Location location, string line) => CurrentScope.AddLine(location, line);
    public void AddLine(string line) => CurrentScope.AddLine(line);

    public void AddAsMultipleLines(string multLines) => AddAsMultipleLines(CodeScope.Location.MID, multLines);
    public void AddAsMultipleLines(CodeScope.Location location,string multLines) {
        foreach (var line in multLines.Split('\n').ToList()) {
            AddLine(location, line);
        }
    }

    public void AddLineBot(string line) => CurrentScope.AddLineBot(line);
    public void AddLineBot() => CurrentScope.AddLineBot();


    public void AddLines(CodeScope.Location loc, List<string> multLines) {
        foreach (var line in multLines)
            AddLine(loc, line);
    }
    
    
    public void StepIn() => CurrentScope = CurrentScope.StepIn();
    public void StepOut() => CurrentScope = CurrentScope.StepOut();

    public void StepInTop() => CurrentScope = CurrentScope.StepInTop();
    public void StepOutTop() => CurrentScope = CurrentScope.StepOutTop();

    private void StepInBot() => CurrentScope = CurrentScope.StepInBot();
    private void StepOutBot() => CurrentScope = CurrentScope.StepOutBot();

    public void WriteToFile() {
        FileStream fileStream = new FileStream(Filepath, FileMode.Create);

        using (StreamWriter writer = new(fileStream)) {
            CurrentScope.AppendToWriter(writer);
        }

        fileStream.Close();
    }
    # endregion
    
    public void Print()  =>  CurrentScope.Print();
    
    
    
}