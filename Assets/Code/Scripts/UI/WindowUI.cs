using UnityEngine;

public abstract class WindowUI : MonoBehaviour
{
    public abstract string dataID { get; protected set; }

    public abstract void OnWindowOpened();
    public abstract void OnWindowClosed();
}
