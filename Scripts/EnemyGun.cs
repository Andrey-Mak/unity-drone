using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float BulletVelocity = 100f;
    public ParticleSystem explosion;

    void Awake() {
        explosion.Play(false);
        // if (Input.GetMouseButtonDown(0)) {
        //     GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //     newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
        // }
    }

        void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

    public void Attack() {
        GameObject _enemyBuulet = (GameObject) Instantiate(bulletPrefab, transform.position, transform.rotation);
        _enemyBuulet.layer = 7;
        Rigidbody rb = _enemyBuulet.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * Random.Range(BulletVelocity - 1, BulletVelocity + 1), ForceMode.Impulse);
        explosion.Play(true);
        // GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        // newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
    }
}
