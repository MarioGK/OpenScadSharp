using System.Text;
using OpenScadSharp.Objects;

namespace OpenScadSharp;

public static class ScriptManager
{
    public static readonly List<BaseObject> Objects = new();
    private static readonly StringBuilder StringBuilder = new();
    
    public static string ScriptToText()
    {
        foreach (var obj in Objects)
        {
            StringBuilder.AppendLine(obj.ToString());
        }
        var scad = StringBuilder.ToString();
        StringBuilder.Clear();
        return scad;
    }
}
