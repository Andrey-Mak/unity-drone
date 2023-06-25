using System.Collections.Generic;
using UnityEngine;

public class DroneFPMovement : MonoBehaviour
{
    public float speed = 7f;
    public VariableJoystick variableJoystickL;

    Rigidbody rigidbody;


    private float rotatePosition = 0f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(variableJoystickL.Horizontal * speed, 0, variableJoystickL.Vertical * speed);
        rigidbody.velocity = transform.rotation * targetVelocity;


        // rigidbody.AddForce(new Vector3(variableJoystickL.Horizontal * speed, 0f, variableJoystickL.Vertical * speed));
    }
}