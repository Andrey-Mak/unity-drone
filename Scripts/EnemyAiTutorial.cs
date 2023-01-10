using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent agent;

    public Animator animator;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 2f;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked, isPatroling, isDetect;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, IsDieded = false;

    List<Transform> navPoints = new List<Transform>();
    private int walkPointNumber = 0;

    private Rigidbody[] _rigidbodies;
    private EnemyGun[] _guns;

    private void Awake()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        if(!IsDieded) {
            DisableRagdoll();
        } else {
            IfDied();
        }

        _guns = GetComponentsInChildren<EnemyGun>();

        // player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
           // navPoints = this.gameObject.transform.Find("/NavPoints").gameObject.transform;
        foreach (Transform child in transform) {
            if (child.tag == "Points") {
                foreach (Transform point in child) {
                    navPoints.Add(point.transform);
                }
                child.DetachChildren();
             }
        }
    }

    private void DisableRagdoll() {
        foreach (var rigidbody in _rigidbodies) {
            rigidbody.isKinematic = true;
        }
    }

    private void EnableRagdoll() {
        foreach (var rigidbody in _rigidbodies) {
            rigidbody.isKinematic = false;
            rigidbody.AddForce(Vector3.up * 100f);
        }
    }

    public void IfDied() {
        Debug.Log("IfDied");
        agent.enabled = false;
        animator.enabled = false;
        EnableRagdoll();
        IsDieded = true;
        // GetComponent<Rigidbody>().AddExplosionForce(200000, transform.position, 10);
        isDetect = false;
        alreadyAttacked = false;
        isPatroling = false;
    }

    private void Update()
    {
        //Check for sight and attack range
        if (!IsDieded) {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) DetectPlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
        animator.SetBool("isDetect", isDetect);
        animator.SetBool("isAttack", alreadyAttacked);
        animator.SetBool("isPatrolling", isPatroling);
    }

    private void Patroling()
    {   
        isDetect = false;
        alreadyAttacked = false;
        isPatroling = true;
        if (!walkPointSet) {
            Invoke(nameof(SearchWalkPoint), 5);
        };

        if (walkPointSet) {
            agent.SetDestination(walkPoint);

        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
            if (walkPointNumber < navPoints.Count - 1) {
                walkPointNumber += 1;
            } else {
                walkPointNumber = 0;
            }
            isPatroling = false;
        }  
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        // float randomZ = Random.Range(-walkPointRange, walkPointRange) + 2f;
        // float randomX = Random.Range(-walkPointRange, walkPointRange) + 2f;

        // walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPoint = new Vector3(navPoints[walkPointNumber].position.x, transform.position.y, navPoints[walkPointNumber].position.z);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void DetectPlayer()
    {   
        if (!IsDieded) {
            LookAtPlayer();
            agent.ResetPath();
            isDetect = true;
            alreadyAttacked = false;
            isPatroling = false;
        }
    }

    private void ChasePlayer()
    {   
        // agent.SetDestination(player.position);
    }

    private void LookAtPlayer() {
        Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        this.transform.LookAt(targetPostition);
    }


    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        // agent.SetDestination(transform.position);
        isPatroling = false;
        LookAtPlayer();

        if (!alreadyAttacked)
        {
            ///Attack code here
            foreach (var _gun in _guns) {
                _gun.Attack();
            }

            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            // rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage");
        IfDied();
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 5f);
    }
    private void DestroyEnemy()
    {
        // Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
