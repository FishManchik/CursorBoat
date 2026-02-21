using System.Runtime.InteropServices;
using UnityEngine;

public interface IInputHandler
{
    bool IsMovePressed { get; }
    Vector3 MouseWorldPosition { get; }
    bool IsSpacePressed { get; }
    bool IsConeSpawnPressed { get; }
    bool IsClearSpacePressed { get; }
    public bool IsThrowPressed { get; }
    public bool IsEscPressed { get;}
}

public class InputHandler : MonoBehaviour, IInputHandler
{
    public bool IsMovePressed { get; private set; }
    public Vector3 MouseWorldPosition { get; private set; }
    public bool IsSpacePressed { get; private set; }
    public bool IsConeSpawnPressed { get; private set; }
    public bool IsClearSpacePressed { get; private set; }
    public bool IsThrowPressed { get; private set; }
    public bool IsEscPressed { get; private set; }

    [SerializeField] private LayerMask groundLayer;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        IsMovePressed = Input.GetMouseButton(0);
        IsThrowPressed = Input.GetMouseButton(1);
        IsSpacePressed = Input.GetKeyDown(KeyCode.Space);
        IsClearSpacePressed = Input.GetKeyDown(KeyCode.R);
        IsEscPressed = Input.GetKeyDown(KeyCode.Escape);

        ToggleConeMode();
        UpdateMouseWorldPosition();
    }

    private void ToggleConeMode()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            IsConeSpawnPressed = IsConeSpawnPressed ? false : true;
        }
    }

    private void UpdateMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            MouseWorldPosition = hit.point;
        }
    }
}

