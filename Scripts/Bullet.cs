using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : MonoBehaviour {
    public float Radius = 1f;
    public float Force = 1000f;
    public ParticleSystem explosion;
    public GameObject Crater;

    private bool isActive = true; 
    private float timeLeft = 1000f;

    private Rigidbody rb;

    public float windForce = 0f;
    public Vector3 windPosition = new Vector3(0, 0, 0);

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        explosion.Stop();
    }

    private void Update() {
        rb.AddForce(windPosition * windForce * Random.Range(-100, 100) * 0.01f, ForceMode.Acceleration);
        if (timeLeft > 0.0f) {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0.0f) {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        
        if (!isActive) return;
        isActive = false;

        rb.useGravity = true;

        Terrain ground = collision.gameObject.GetComponent<Terrain>();

        if (ground) {
            Transform CraterTransform = Crater.GetComponent<Transform>();
            MeshRenderer CraterMeshRenderer = Crater.GetComponent<MeshRenderer>();
            CraterTransform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            CraterMeshRenderer.enabled = true;
        }

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
                rigidbody.AddExplosionForce(Force, transform.position, Radius, 500.0F);
                // timeLeft = explosion.main.duration;
                explosion.Play();
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<SphereCollider>().enabled = false;
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
