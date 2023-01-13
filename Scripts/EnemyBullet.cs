using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float damage = 10;
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

        PlayerManager.onPlayerDamage(collision);
    }
}
