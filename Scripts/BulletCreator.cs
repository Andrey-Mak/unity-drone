using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCreator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float BulletVelocity = 1f;

    void Update()
    {
        // if (Input.GetMouseButtonDown(0)) {
        //     GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //     newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
        // }
    }

    public void Attack() {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
    }
}
