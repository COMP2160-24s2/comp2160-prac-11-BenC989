using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private Actions actions;
    private InputAction zoomAction;
    private float mouseInput;
    private float minSize = 30f;
    private float maxSize = 100f;
    [SerializeField] private float size = 0.03f;

    private void Awake()
    {
        actions = new Actions();
        zoomAction = actions.camera.zoom;
    }

    private void OnEnable()
    {
        zoomAction.Enable();
    }

    private void OnDisable()
    {
        zoomAction.Disable();
    }

    private void Update()
    {
        // Read input
        mouseInput = zoomAction.ReadValue<float>();

        if ((Camera.main.fieldOfView >= (minSize + (mouseInput * size))) && (Camera.main.fieldOfView <= (maxSize + (mouseInput * size))))
        {
            Camera.main.fieldOfView += -(mouseInput * size);
        }
    }
}