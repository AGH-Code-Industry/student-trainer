using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected bool isAlerted = false;
    protected Transform playerTransform;

    protected virtual void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;
    }

    public bool GetAlertStatus() { return isAlerted; }
    public void SetAlertStatus(bool _status) { isAlerted = _status; }
}