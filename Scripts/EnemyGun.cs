using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float BulletVelocity = 30f;

    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        //     GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //     newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
        // }
    }

    public void Attack() {
        Rigidbody rb = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * BulletVelocity, ForceMode.Impulse);

        // GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        // newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
    }
}
