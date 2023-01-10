using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBullet : MonoBehaviour {
    private bool isActive = true;
    private float timeLeft = 10;

    private void Update() {
        if (timeLeft > 0.0f) {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0.0f) {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        
        if (!isActive) return;
        isActive = false;

        GetComponent<Rigidbody>().useGravity = true;

        // Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        // if (enemy) {
        //     enemy.OnHit();
        // }
        Debug.Log("Enemy Bullet");
    }
}
