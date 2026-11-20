namespace CactusLang.CodeGeneration.CCodeGenerator;

public class CodeScope : CodeSegment {
    
    public enum Location {
        TOP,
        MID,
        BOT
    }
    
    private List<CodeSegment> _top;
    private List<CodeSegment> _mid;
    private List<CodeSegment> _bot;

    private Dictionary<Location, List<CodeSegment>> _resolver;

    private CodeScope? _parent;

    public CodeScope() : this(null) {
    }

    private CodeScope(CodeScope parent) {
        _top = new List<CodeSegment>();
        _mid = new List<CodeSegment>();
        _bot = new List<CodeSegment>();
        _parent = parent;
        
        _resolver = new(){
            [Location.TOP] = _top,
            [Location.MID] = _mid,
            [Location.BOT] = _bot
        };
        
        if (parent != null) {
            Depth = parent.Depth + 1;
        }
        else {
            Depth = 0;
        }
    }

    private string _currLine = "";


    public void Add(string text) => _currLine += text;


    private void AddStr(List<CodeSegment> list, string segment) =>
        AddSegment(list, new CodeLine(_currLine + segment, Depth));
    private void AddStr(Location location, string segment)  => AddStr(_resolver[location], segment);
        

    private void AddSegment(List<CodeSegment> list, CodeSegment segment) {
        list.Add(segment);
        _currLine = "";
    }

    public void AddLineTop(string line) => AddStr(_top, line);
    public void AddLineTop() => AddStr(_top, "");


    public void AddLine() => AddStr(_mid, "");
    public void AddLine(string line) => AddStr(_mid, line);
    
    public void AddLine(Location location, string line) => AddStr(location, line);



    public void AddLineBot(string line) => AddStr(_bot, line);
    public void AddLineBot() => AddSegment(_bot, new CodeLine("", Depth));

    public CodeScope StepIn() {
        var codeScope = new CodeScope(this);
        AddSegment(_mid, codeScope);
        return codeScope;
    }

    public CodeScope StepOut() => _parent!;


    public CodeScope StepInTop() {
        var codeScope = new CodeScope(this);
        AddSegment(_top, codeScope);
        return codeScope;
    }

    public CodeScope StepOutTop() {
        return StepOut();
    }


    public CodeScope StepInBot() {
        var codeScope = new CodeScope(this);
        AddSegment(_bot, codeScope);
        return codeScope;
    }

    public CodeScope StepOutBot() {
        return StepOut();
    }


    public override void Print() {
        foreach (var segment in _top)
            segment.Print();

        foreach (var segment in _mid)
            segment.Print();

        for (var i = 1; i <= _bot.Count; i++) {
            var segment = _bot[^i];
            segment.Print();
        }

        Console.WriteLine();
    }

    public override void AppendToWriter(TextWriter writer) {
        foreach (var segment in _top)
            segment.AppendToWriter(writer);

        foreach (var segment in _mid)
            segment.AppendToWriter(writer);

        for (var i = 1; i <= _bot.Count; i++) {
            var segment = _bot[^i];
            segment.AppendToWriter(writer);
        }

        writer.WriteLine();
    }
}