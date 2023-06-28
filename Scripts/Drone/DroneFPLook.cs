using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DroneFPLook : MonoBehaviour
{
    Camera camera;
    public float defaultFOV = 70;
    public float maxZoomFOV = 9;
    [Range(0, 1)]
    private float currentZoom = 0;
    private float zoom = 0;
    public TextMeshProUGUI zoomText;
    public float sensitivity = 5;

    [SerializeField]
    Transform target;

    [SerializeField]
    Transform character;
    public float smoothing = 1f;
    public float speedRotation = 0.15f;
    public VariableJoystick variableJoystickR;

    Vector2 velocity;
    Vector2 frameVelocity;

    void Reset()
    {
        // Get the character from the DroneFPMovement in parents.
        character = GetComponentInParent<DroneFPMovement>().transform;
    }

    void Awake()
    {
        // Get the camera on this gameObject and the defaultZoom.
        camera = GetComponent<Camera>();
        if (camera)
        {
            defaultFOV = camera.fieldOfView;
        }
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        zoomText.text = "x" + (currentZoom).ToString("F0");
        // Get smooth velocity.
        // Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 mouseDelta = new Vector2(variableJoystickR.Horizontal, variableJoystickR.Vertical);
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta * speedRotation, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, -10 + currentZoom);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        cameraZoom();
    }

    public void ZoomIn(int zoomValue) {
        if (currentZoom < maxZoomFOV) {
            currentZoom += zoomValue;
            target.localScale = new Vector3(target.localScale.x - 0.0003f, target.localScale.y - 0.0003f, target.localScale.z - 0.0003f);
        }
    }

    public void ZoomOut(int zoomValue) {
        if (currentZoom > 1) {
            currentZoom -= zoomValue;
            target.localScale = new Vector3(target.localScale.x + 0.0003f, target.localScale.y + 0.0003f, target.localScale.z + 0.0003f);
        }
    }

    private void cameraZoom() {
        if (currentZoom * 0.1 > zoom) {
            zoom += 0.01f;
        }
        if (currentZoom * 0.1 < zoom) {
            zoom -= 0.01f;
        }
        zoom = Mathf.Clamp01(zoom);
        camera.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, zoom);
    }
}
