using System.Reflection;
using System.IO;

namespace GameEngine.Utilities;

public class GetRootFolderName
{
    public static string Nome
    {
        get
        {
            string exePath = Assembly.GetExecutingAssembly().Location;
            DirectoryInfo? dir = new DirectoryInfo(Path.GetDirectoryName(exePath)!);
            while (dir != null && !dir.GetFiles("*.csproj").Any())
                dir = dir.Parent;

            return dir?.Name ?? "Unknown";
        }
    }

    public static string NomeSemPrefixo
    {
        get
        {
            return Nome.Length >= 4 ? Nome.Substring(4) : Nome;
        }
    }
}
