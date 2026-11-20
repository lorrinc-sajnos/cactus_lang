using CactusLang.Compiling.Codefiles;

namespace CactusLang.Model.Symbols;

public class FileID {
    public string? ForeignRoot { get; }

    public string Filename { get; }
    public string[] Folders { get; }

    public string Extension { get; }


    internal FileID(string[] folders, string filename, string? foreignRoot = null) {
        Filename = Path.GetFileNameWithoutExtension(filename);

        Extension = Path.GetExtension(filename);
        if (Extension == "")
            Extension = ".ccts";
        Folders = folders;

        ForeignRoot = foreignRoot;
    }

    public static FileID FromAbsolutePathDcl(string absPath, CactusGrove grove, string? foreignRootFolder = null) {
        string relPath = Path.GetRelativePath(grove.RootFolder, absPath);
        return FromRelPathDcl(relPath, grove, foreignRootFolder);
    }

    public static FileID FromRelPathDcl(string relPath, CactusGrove grove, string? foreignRootFolder = null) {
        string[] parts = relPath.Split('\\', '/');

        for (int i = 0; i < parts.Length; i++)
            parts[i] = parts[i].Trim();

        string filename = Path.GetFileName(parts[^1]);

        string[] folders = parts[..^1];

        return new FileID(folders, filename, foreignRootFolder);
    }

    public override bool Equals(object? obj) {
        if (obj is not FileID otherFileId)
            return base.Equals(obj);

        if (Filename != otherFileId.Filename)
            return false;
        if (Folders.Length != otherFileId.Folders.Length)
            return false;
        if (Extension != otherFileId.Extension)
            return false;
        for (int i = 0; i < Folders.Length; i++)
            if (Folders[i] != otherFileId.Folders[i])
                return false;

        return true;
    }

    public string AsRelPath(char joinChar = '/') => String.Join(joinChar, Folders) + Filename;
    public string AsRelPathWExtention(char joinChar = '/') => AsRelPath(joinChar) + Extension;
    public override string ToString() => AsRelPath();

    public string CPrefix => String.Join("_$_", Folders) + Filename + "_$$_";
}