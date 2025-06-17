using System.Collections.Generic;

public class SystemFreezer
{
    private List<string> freezes = new List<string>();

    public bool Frozen
    {
        get { return freezes.Count > 0; }
    }

    public void Freeze(string freezeID)
    {
        if (!freezes.Contains(freezeID))
            freezes.Add(freezeID);
    }

    public void Unfreeze(string freezeID)
    {
        if (freezes.Contains(freezeID))
            freezes.Remove(freezeID);
    }
}
