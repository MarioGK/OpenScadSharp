using System.Text;

namespace OpenScadSharp;

public static class ScriptManager
{
    public static readonly StringBuilder StringBuilder = new();
    
    public static string ScriptToText()
    {
        var scad = StringBuilder.ToString();
        StringBuilder.Clear();
        return scad;
    }
}
