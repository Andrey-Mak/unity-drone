using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour {
    public float Radius;
    public float Force;
    public ParticleSystem explosion;

    private bool isActive = true;
    private float timeLeft = 10;

    private void Start() {
        explosion.Stop();
    }

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
        Explode();
    }

    public void Explode() {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, Radius);

        for (int i = 0; i < overlappedColliders.Length; i++) {
            Debug.Log(overlappedColliders[i].name);
            EnemyAiTutorial enemy = overlappedColliders[i].GetComponentInParent<EnemyAiTutorial>();
            if (enemy) {
                enemy.TakeDamage(1);
                // enemy.OnHit();
            }
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;
            Animator animator = overlappedColliders[i].GetComponentInParent<Animator>();
            UnityEngine.AI.NavMeshAgent agent = overlappedColliders[i].GetComponentInParent<NavMeshAgent>();
            if (animator) {
                animator.enabled = false;
            }
            if (agent) {
                agent.enabled = false;
            }
            if (rigidbody) {
                // rigidbody.AddForceAtPosition(transform.up * Force, transform.position);
                rigidbody.isKinematic = false;
                rigidbody.AddExplosionForce(Force, transform.position, Radius, 3.0F);
                timeLeft = explosion.main.duration;
                explosion.Play();
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                // Destroy(gameObject);
                // StartCoroutine(Explosion());
            }
        }
    }

    // private IEnumerator Explosion() {
    //     // ParticleSystem newExplosion = Instantiate(ParticleSystem, transform.position, Quaternion.identity);
    //     yield return new WaitForSeconds(5);
    //     Destroy(newExplosion);
    // }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
