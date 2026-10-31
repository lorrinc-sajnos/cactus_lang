using CactusLang.Compiling.Codefiles;

namespace CactusLang.Model.Symbols;

public class FileID
{
    public string Filename { get; }
    public string[] Folders { get; }

    public string Extension { get; }


    internal FileID(string[] folders, string filename)
    {
        Filename = Path.GetFileNameWithoutExtension(filename);

        Extension = Path.GetExtension(filename);
        if (Extension == "")
            Extension = ".ccts";
        Folders = folders;
    }

    public static FileID FromAbsolutePath(string absPath, CactusGrove grove) {

        string relPath = Path.GetRelativePath(grove.RootFolder, absPath);
        return FromRelPath(relPath, grove);
    }

    public static FileID FromRelPath(string relPath, CactusGrove grove) {

        string[] parts = relPath.Split('\\', '/');

        for (int i = 0; i < parts.Length; i++)
            parts[i] = parts[i].Trim();

        string filename = Path.GetFileName(parts[^1]);

        string[] folders = parts[..^1];

        return new FileID(folders, filename);
    }

    public override bool Equals(object? obj)
    {
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

    public string AsRelPath() => String.Join('/', Folders) + Filename + Extension;
    public override string ToString() => AsRelPath();

    public string CPrefix => String.Join("_$_", Folders) + Filename + "_$$_";
}