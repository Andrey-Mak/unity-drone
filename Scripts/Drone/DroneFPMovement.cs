using System.Collections.Generic;
using UnityEngine;

public class DroneFPMovement : MonoBehaviour
{
    public float speed = 50f;
    public VariableJoystick variableJoystickL;

    Rigidbody rigidbody;


    private float rotatePosition = 0f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(new Vector3(variableJoystickL.Horizontal * speed, 0f, variableJoystickL.Vertical * speed));
    }
}