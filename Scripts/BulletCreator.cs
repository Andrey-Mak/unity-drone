using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BulletCreator : MonoBehaviour
{
    public GameObject bulletPrefab;
    public TextMeshProUGUI booletCountText;
    public float BulletVelocity = 1f;
    public int timeBetweenAttacks = 2;

    private float windForce;
    private Vector3 windPosition;
    private bool alreadyAttacked;

    private void Awake() {
        windPosition = new Vector3(Random.Range(-20, 20) * 0.1f, 0, Random.Range(-20, 20) * 0.1f);
        windForce = Random.Range(0, 50) * 0.1f;
        Debug.Log("windPosition: " + windPosition + "; windForce: " + windForce);
    }

    void Update()
    {
        booletCountText.text = "Boolets: " + PlayerSettings.BOOLET_COUNT;
        // if (Input.GetMouseButtonDown(0)) {
        //     GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //     newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
        // }
    }

    public void Attack() {
        if (!alreadyAttacked && PlayerSettings.BOOLET_COUNT > 0) {
            PlayerSettings.BOOLET_COUNT--;
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            newBullet.layer = 8;
            Bullet bullet = newBullet.GetComponent<Bullet>();
            bullet.windForce = windForce;
            bullet.windPosition = windPosition;
            newBullet.GetComponent<Rigidbody>().velocity = transform.forward * BulletVelocity;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }
}
