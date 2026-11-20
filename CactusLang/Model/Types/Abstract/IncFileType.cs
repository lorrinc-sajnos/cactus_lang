using CactusLang.Model.CodeStructure.Entities.IncludedFiles;
using CactusLang.Model.Types.EntityTypes;
using CactusLang.Util;

namespace CactusLang.Model.Types.Abstract;

public class IncFileType(IncludedFile file) : EntityType(file) {
	public IncludedFile File = file;
	public override string Name => File.Name;
	public override int Size => 0;

	public override bool IsAbstract => true;


	public override bool CanImplicitCastInto(CactusType other) => false;
}