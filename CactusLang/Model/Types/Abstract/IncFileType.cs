using CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;
using CactusLang.Model.Types.MemberHolderTypes;
using CactusLang.Util;

namespace CactusLang.Model.Types.Abstract;

public class IncFileType(IncludedFile file) : MemberHolderType(file) {
	public IncludedFile File = file;
	public override string Name => File.Name;
	public override int Size => 0;

	public override bool IsAbstract => true;


	public override bool CanImplicitCastInto(BaseType other) => false;
}