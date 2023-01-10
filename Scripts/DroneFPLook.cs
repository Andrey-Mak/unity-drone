using UnityEngine;

public class DroneFPLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 1;
    public float smoothing = 0.5f;
    public VariableJoystick variableJoystickR;

    Vector2 velocity;
    Vector2 frameVelocity;


    void Reset()
    {
        // Get the character from the DroneFPMovement in parents.
        character = GetComponentInParent<DroneFPMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        // Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get smooth velocity.
        // Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 mouseDelta = new Vector2(variableJoystickR.Horizontal, variableJoystickR.Vertical);
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta * 0.1f, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, -40);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        // character.localRotation = Quaternion.Euler(velocity.y, -velocity.x, 0);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
