using UnityEngine;

public class RoomRevealer : MonoBehaviour
{
    [SerializeField] GameObject fogObj;
    [SerializeField] bool dontHideOnceRevealed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (fogObj != null)
            fogObj.SetActive(true);
    }

    public void SetFog(bool state)
    {
        if(state == true && fogObj.activeInHierarchy == false && dontHideOnceRevealed)
            return;

        fogObj.SetActive(state);
    }

    public void Reveal() { SetFog(false); }
    public void Hide() { SetFog(true); }
}
