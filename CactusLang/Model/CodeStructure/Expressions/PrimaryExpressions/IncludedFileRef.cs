using CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.Abstract;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class IncludedFileRef(IncludedFile file) : PrimaryExpression {
	//While it is virtual, type of FileRef *is* a pointer, as it refers to the file, and does not contain its data.

	public IncludedFile IncFile { get; } = file;
	
	public PointerType Type { get; } = new( new IncFileType(file) );
	public override BaseType GetResultType() => Type;
	public override bool IsLValue() => false;
}