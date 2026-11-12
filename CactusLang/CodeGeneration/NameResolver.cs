using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.MemberHolderTypes;
using CactusLang.Model.Types.PrimitiveTypes;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public class NameResolver {

	CactusGrove _cactusGrove;
	private CactusFile _currFile;
	private readonly string PREFIX;

	public NameResolver(CactusFile file) {
		_cactusGrove = file.Grove;
		_currFile = file;
		PREFIX = GetFile(_currFile);
	}

	#region Constants

	public const string PARAM_SEP = ", ";

	#endregion



	public string GetFileID(FileID id) => String.Join("_F_", id.Folders) + id.Filename + "_FF_";
	public string GetFile(CactusFile file) => GetFileID(file.ID);


	public string GetFileIDAsHeaderFile(FileID id) => id.AsRelPath() + ".h";
	public string GetFileAsHeaderFile(CactusFile file) => GetFileIDAsHeaderFile(file.ID);
	
	public string GetFileIDAsHeaderGuard(FileID id) => id.AsRelPath('_').ToUpper()  + "_H";
	public string GetFileAsHeaderGuard(CactusFile file) => GetFileIDAsHeaderGuard(file.ID);


	public string GetFileFunc(FileFunction func) => GetFile(func.CactusFile) + func.Name;

	public string GetFileField(FileField field) => GetFile(field.CactusFile) + field.Name;
	public string GetMemberField(MemberField field) => GetMemberHolderPrefix(field.Parent) + field.Name;

	private string GetStructFunc(StructFunction func) => func.Name;

	public string GetStructRefFunc(StructFunction func) =>
		$"{GetBaseType(func.Parent.MemberType)}_ref__{GetStructFunc(func)}";

	public string GetStructAccFunc(StructFunction func) =>
		$"{GetBaseType(func.Parent.MemberType)}_acc__{GetStructFunc(func)}";

	
	public string GetFileRefFunc(IncludedFileRef fileRef, StructFunction func) =>
		$"{GetFile(fileRef.IncFile.CactusFile)}{GetFunc(func)}";
	
	
	public string GetBaseType(BaseType type) {
		switch (type) {
			case MemberHolderType memType:
				return GetMemberHolderName(memType.Parent);
			case PointerType pointerType:
				return $"{GetBaseType(pointerType.PointsTo)}*";
			case PrimitiveType pType:
				return pType.Name;
			default:
				return type.Name;
		}
	}
	#region Struct
	public string GetStructRefFuncHeader(StructFunction func) {
		string header = "";

		header += GetBaseType(func.ReturnType) + " ";
		//In C, no local functions, so file function w/ custom name:
		header += GetStructRefFunc(func);
		//Introduce the "this" pointer as first variable
		header += $"({GetBaseType(func.Parent.MemberType)}* this" + GetFuncDclParams_NO_OPEN(func);

		return header;
	}
	public string GetStructAccFuncHeader(StructFunction func) {
		string header = "";

		header += GetBaseType(func.ReturnType) + " ";
		//In C, no local functions, so file function w/ custom name:
		header += GetStructAccFunc(func);
		//Introduce the "thisVal" as first variable
		header += $"({GetBaseType(func.Parent.MemberType)} thisVal{GetFuncDclParams_NO_OPEN(func)}";

		return header;
	}

	public List<string> GetStructAccFuncBody(StructFunction func) {
		List<string> body = new List<string>();

		body.Add($"{GetBaseType(func.Parent.MemberType)}* this = &thisVal;");
		body.Add($"return {GetStructRefFunc(func)}(this{GetFuncCallParams_NO_OPEN(func)};");

		return body;
	}

	public string GetFileFunctionHeader(FileFunction func) {
		string header = "";

		header += GetBaseType(func.ReturnType) + " ";
		header += GetFileFunc(func);
		header += "(" + GetFuncDclParams_NO_OPEN(func);
		return header;
	}

	public string GetFuncDclParams_NO_OPEN(ModelFunction func) {
		string header = "";
		var parameters = func.Symbol.GetParameters();
		if (parameters.Count > 0) {
			//Strunc functions first param is the struct itself
			if (func is StructFunction)
				header += PARAM_SEP;

			for (int i = 0; i < parameters.Count - 1; i++) {
				var param = parameters[i];
				header += $"{GetBaseType(param.Type)} {param.CName}" + PARAM_SEP;
			}

			header += $"{GetBaseType(parameters[^1].Type)} {parameters[^1].CName}";
		}

		header += ")";
		return header;
	}

	public string GetFuncCallParams_NO_OPEN(ModelFunction func) {
		string header = "";
		var parameters = func.Symbol.GetParameters();
		if (parameters.Count > 0) {
			//Strunc functions first param is the struct itself
			if (func is StructFunction)
				header += PARAM_SEP;

			for (int i = 0; i < parameters.Count - 1; i++) {
				var param = parameters[i];
				header += $"{param.CName}" + PARAM_SEP;
			}

			header += $"{parameters[^1].CName}";
		}

		header += ")";
		return header;
	}
	
	#endregion


	#region Functions

	public string GetFunc(ModelFunction mFunc) {

		switch (mFunc) {
			case FileFunction fileFunc:
				return GetFileFunc(fileFunc);
			case MemberFunction memFunc:
				return GetMemberFunction(memFunc);
			default:
				return mFunc.Name;
		}
	}

	public string GetMemberFunction(MemberFunction mFunc) => GetPrefix(mFunc.Parent) + mFunc.Name;
	
	public string GetMemberHolderName(IMemberHolder memHolder) => GetFileID(memHolder.ParentID)+GetPrefix(memHolder);

	public string GetPrefix(IMemberHolder holder) {
		switch (holder) {
			case CactusFile file:
				return GetFile(file);
			default:
				return GetMemberHolderPrefix(holder);
		}

	}

	public string GetMemberHolderPrefix(IMemberHolder holder) => $"M{holder.Name}_";

	#endregion

	public string GetLocalVar(LocalVarRef structFieldRef) => structFieldRef.VarName;

	public string GetField(MemberField fieldRefField) => fieldRefField.Name;

	

}