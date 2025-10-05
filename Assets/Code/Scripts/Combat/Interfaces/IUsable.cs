using System.Collections.Generic;

public interface IUsable
{
    void Use(Character user, string binding);
    IEnumerable<string> GetBindings();
}