using System.Collections.Generic;
using UnityEngine;

public class DroneFPMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public float height = 10f;
    public VariableJoystick variableJoystickL;
    public KeyCode runningKey = KeyCode.LeftShift;

    [SerializeField]
    Transform character;
    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    private float heightSpeed = 0f;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        if (Mathf.Round(transform.position.y) != height) {
            heightSpeed = Mathf.Round(transform.position.y) < height ? 1 : -1;
            rigidbody.constraints = RigidbodyConstraints.None;
        } else {
            heightSpeed = 0;
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationY;
        }

        // Get targetVelocity from input.
        // Vector2 targetVelocity = new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);
        // Debug.Log(heightSpeed);

        Vector3 targetVelocity = new Vector3(variableJoystickL.Horizontal * targetMovingSpeed, heightSpeed, variableJoystickL.Vertical * targetMovingSpeed);

        // Apply movement.
        // rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        // rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        rigidbody.velocity = transform.rotation * targetVelocity;

        // Vector3 direction = Vector3.forward * variableJoystickL.Vertical + Vector3.right * variableJoystickL.Horizontal;
        // rigidbody.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    public void Rise(float newHeight) {
        height = newHeight;
    }
}