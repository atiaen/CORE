using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Runtime.InteropServices;

public static class MiscHelpers
{
    static Random random = new Random();

    public static Font GetBaseFont()
    {
        return LoadFont("path to font"); ;
    }

    public static string GetOS()
    {
        string os = "";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            os = "win-x64";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            os = "osx-x64";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            os = "linux-x64";
        }

        return os;
    }

    public static double GetRandomNumber(double minimum, double maximum)
    {
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

}