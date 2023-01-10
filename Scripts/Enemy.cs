using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // private Rigidbody[] _rigidbodies;
    // private Animator animator;

    // private void Awake() {
    //     _rigidbodies = GetComponentsInChildren<Rigidbody>();
    //     DisableRagdoll();
    // }

    // private void DisableRagdoll() {
    //     foreach (var rigidbody in _rigidbodies) {
    //         rigidbody.isKinematic = true;
    //     }
    // }

    // private void EnableRagdoll() {
    //     foreach (var rigidbody in _rigidbodies) {
    //         rigidbody.isKinematic = false;
    //     }
    // }

    // public void OnHit() {
    //     Debug.Log("OnHit");
    //     EnableRagdoll();
    //     // GetComponent<Rigidbody>().AddExplosionForce(200000, transform.position, 10);
    //     GetComponent<Rigidbody>().AddForce(Vector3.up * 1000f);
    // }

}
