/**
 * A singleton class to allow point-and-click movement of the marble.
 * 
 * It publishes a TargetSelected event which is invoked whenever a new target is selected.
 * 
 * Author: Malcolm Ryan
 * Version: 1.0
 * For Unity Version: 2022.3
 */

using UnityEngine;
using UnityEngine.InputSystem;

// note this has to run earlier than other classes which subscribe to the TargetSelected event
[DefaultExecutionOrder(-100)]
public class UIManager : MonoBehaviour
{
#region UI Elements
    [SerializeField] private Transform crosshair;
    [SerializeField] private Transform target;
#endregion 

#region Singleton
    static private UIManager instance;
    static public UIManager Instance
    {
        get { return instance; }
    }
#endregion 

#region Actions
    private Actions actions;
    private InputAction mouseAction;
    private InputAction deltaAction;
    private InputAction selectAction;
#endregion

#region Events
    public delegate void TargetSelectedEventHandler(Vector3 worldPosition);
    public event TargetSelectedEventHandler TargetSelected;
#endregion

#region Init & Destroy
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UIManager in the scene.");
        }

        instance = this;

        actions = new Actions();
        mouseAction = actions.mouse.position;
        deltaAction = actions.mouse.delta;
        selectAction = actions.mouse.select;

        Cursor.visible = false;
        target.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        actions.mouse.Enable();
    }

    void OnDisable()
    {
        actions.mouse.Disable();
    }
#endregion Init

#region Update
    void Update()
    {
        MoveCrosshair();
        SelectTarget();
    }

    private void MoveCrosshair() 
    {
        Vector2 mousePos = mouseAction.ReadValue<Vector2>();

        Plane plane = new Plane(-Vector3.up, 0.5f); // 0.5f is the position of the marble

        // Create a ray from the Mouse click position
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        // Initialise the enter variable
        float enter = 0.0f;

        if (plane.Raycast(ray, out enter))
        {
            // Get the point that is clicked
            Vector3 hitPoint = ray.GetPoint(enter);

            // Move the crosshair to the point clicked
            crosshair.position = hitPoint;
        }

        // FIXME: Move the crosshair position to the mouse position (in world coordinates)
        /*Vector2 mousePos = mouseAction.ReadValue<Vector2>(); // Screen coordinates?
        float zPos = 9.5f;
        Vector3 worldCoords = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, zPos));
        crosshair.position = new Vector3(worldCoords.x, worldCoords.y, worldCoords.z);*/
    }

    private void SelectTarget()
    {
        if (selectAction.WasPerformedThisFrame())
        {
            // set the target position and invoke 
            target.gameObject.SetActive(true);
            target.position = crosshair.position;     
            TargetSelected?.Invoke(target.position);       
        }
    }

#endregion Update

}
