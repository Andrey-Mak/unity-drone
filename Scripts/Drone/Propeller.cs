using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    void Update() {
        transform.Rotate(new Vector3(0f, 720f, 0f) * Time.deltaTime);
    }
}
