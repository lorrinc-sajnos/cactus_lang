using System.Runtime.InteropServices;

namespace CactusLang.Tests;

public static class Util
{
    public static string EOL { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "\r\n" : "\n";


}