using CactusLang.Model.CodeStructure.CodeBlocks;

namespace CactusLang.Model.CodeStructure.Statements;

public class RawCCodeStatement : Statement {
    public string RawCode { get; }

    public RawCCodeStatement(CodeBlock codeBlock, string rawCode, bool hasBrackets = true) :  base(codeBlock) {
        if (!hasBrackets) {
            RawCode = rawCode + ";";
        }
        else {
            int length = rawCode.Length;

            RawCode = rawCode[3..(length - 4)];
        }
    }
}