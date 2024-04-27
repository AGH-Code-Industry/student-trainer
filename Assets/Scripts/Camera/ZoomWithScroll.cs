using UnityEngine;
using Cinemachine;

public class SmoothAdjustFreeLookCameraWithScroll : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float scrollSensitivity = 0.1f;
    public float smoothTime = 0.2f; // Time taken to smooth the movement
    [SerializeField] private float targetYAxisValue;
    private float yAxisVelocity = 0.0f; // Current velocity, this value is modified by SmoothDamp

    void Start()
    {
        freeLookCamera = GameObject.FindGameObjectWithTag("3rdPersonCamera").GetComponent<CinemachineFreeLook>();
        if (freeLookCamera != null)
        {
            // Initialize target Y Axis value
            targetYAxisValue = Mathf.Clamp(targetYAxisValue, 0f, 1f);
        }
    }

    void Update()
    {
        if (freeLookCamera != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            // Update target Y Axis value based on scroll input
            targetYAxisValue += scroll * scrollSensitivity;
            targetYAxisValue = Mathf.Clamp(targetYAxisValue, 0f, 1f); // Ensure target is within valid range

            // Smoothly interpolate the camera's Y Axis value towards the target value
            freeLookCamera.m_YAxis.Value = Mathf.SmoothDamp(freeLookCamera.m_YAxis.Value, targetYAxisValue, ref yAxisVelocity, smoothTime);
        }
    }
}