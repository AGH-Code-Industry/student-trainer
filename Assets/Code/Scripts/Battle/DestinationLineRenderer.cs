using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DestinationLineRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField] LayerMask clickableLayers;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers))
        {
            _lineRenderer.SetPosition(1, hit.transform.position);
        }
    }
}