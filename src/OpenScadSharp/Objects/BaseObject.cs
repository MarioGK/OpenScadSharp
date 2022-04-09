namespace OpenScadSharp.Objects;

public class BaseObject
{
    public string Code { get; set; }
    
    public BaseObject()
    {
        ScriptManager.Objects.Add(this);
    }

    public override string ToString()
    {
        return $"{Code}";
    }
}