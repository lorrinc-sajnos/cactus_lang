using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public abstract class Operator {
    public enum OperatorLvl {
        Mult,
        Add,
        Bit,
        Comparison,
        Assignment
    }

    public OperatorLvl Level { get; private set; }

    private string _id;

    public string GetId() => _id;

    public static Operator GetById(string id) => Operators[id];

    private static readonly Dictionary<string, Operator> Operators = new();

    protected Operator(string format, OperatorLvl level) {
        Level = level;
        _id = format;
        Operators.Add(format, this);
    }
    //protected

    public abstract BaseType Evaluate(BaseType lhs, BaseType rhs);

    //Multiplication level
    public static readonly MathOperator MULT = new("*", OperatorLvl.Mult);
    public static readonly MathOperator DIV = new("/", OperatorLvl.Mult);

    public static readonly IntOperator REMAINDER = new("%", OperatorLvl.Mult);

    //Addition level
    public static readonly MathOperator ADD = new("+", OperatorLvl.Add);

    public static readonly MathOperator SUB = new("-", OperatorLvl.Add);

    //Bit level
    public static readonly IntOperator BIT_AND = new("&", OperatorLvl.Bit);
    public static readonly IntOperator BIT_OR = new("|", OperatorLvl.Bit);
    public static readonly IntOperator BIT_XOR = new("^", OperatorLvl.Bit);
    public static readonly BitShiftOperator BIT_LEFT_SHIFT = new("<<", OperatorLvl.Bit);

    public static readonly BitShiftOperator BIT_RIGHT_SHIFT = new(">>", OperatorLvl.Bit);

    //Comparison operator
    public static readonly CompOperator EQ = new("==", OperatorLvl.Comparison);
    public static readonly CompOperator NEQ = new("!=", OperatorLvl.Comparison);
    public static readonly CompOperator AND = new("&&", OperatorLvl.Comparison);
    public static readonly CompOperator OR = new("||", OperatorLvl.Comparison);
    public static readonly CompOperator LS_THAN = new("<", OperatorLvl.Comparison);
    public static readonly CompOperator GR_THAN = new(">", OperatorLvl.Comparison);
    public static readonly CompOperator LS_THAN_EQ = new("<=", OperatorLvl.Comparison);
    public static readonly CompOperator GR_THAN_EQ = new(">=", OperatorLvl.Comparison);

    public static readonly AssignmentOperator ASG = new("=");
    public static readonly AssignmentOperator ADD_ASG = new("+=");
    public static readonly AssignmentOperator NEG_ASG = new("-=");
    public static readonly AssignmentOperator MUL_ASG = new("*=");
    public static readonly AssignmentOperator DIV_ASG = new("/=");
    public static readonly AssignmentOperator REM_ASG = new("%=");
    public static readonly AssignmentOperator AND_ASG = new("&=");
    public static readonly AssignmentOperator OR_ASG = new("|=");
    public static readonly AssignmentOperator XOR_ASG = new("^=");
    public static readonly AssignmentOperator LSH_ASG = new("<<=");
    public static readonly AssignmentOperator RSH_ASG = new(">>=");
    public override string ToString() => _id;
}