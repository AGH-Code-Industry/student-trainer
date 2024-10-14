using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BattleTurnView : MonoBehaviour
{
    private Image _image;

    public void SetTurnData(BattleTurn turn)
    {
        if (_image == null) _image = GetComponent<Image>();
        _image.sprite = turn.sprite;
    }
}