using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiTutorial : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public Animator animator;

    private Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsBoolet;

    public float distanceWhenRun = 2f;

    public float health = 2f;

    //Patroling
    public Vector3 walkPoint;
    bool isWalkPointFinded;
    bool walkPointSet;
    public float walkPointRange;
    public float minWalkRadius = 10f;
    public float maxWalkRadius = 30f;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked, isPatroling, isDetect, isRuningToSafePlace, isCrouch;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, IsDieded = false;

    List<Transform> navPoints = new List<Transform>();
    List<Transform> savePoints = new List<Transform>();
    private int walkPointNumber = 0;

    private Rigidbody rb;

    private Rigidbody[] _rigidbodies;
    private EnemyGun[] _guns;
    private float detectedSightRange = 0f;
    private float currentSightRange;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        if(!IsDieded) {
            DisableRagdoll();
        } else {
            IfDied();
        }

        _guns = GetComponentsInChildren<EnemyGun>();
        // player = GameObject.Find("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
           // navPoints = this.gameObject.transform.Find("/NavPoints").gameObject.transform;

        // foreach (Transform child in transform) {
        //     if (child.tag == "Points") {
        //         foreach (Transform point in child) {
        //             // navPoints.Add(point.transform);
        //             point.gameObject.GetComponent<MeshRenderer>().enabled = false;
        //         }
        //         child.DetachChildren();
        //     }
        // }

        foreach (GameObject savePoint in GameObject.FindGameObjectsWithTag("SavePoints")) {
            savePoints.Add(savePoint.transform);
        }
    }

    public void SetNavPoint(Transform point) {
        navPoints.Add(point);
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
        navMeshAgent.enabled = false;
        animator.enabled = false;
        EnableRagdoll();
        IsDieded = true;
        // GetComponent<Rigidbody>().AddExplosionForce(200000, transform.position, 10);
        isDetect = false;
        alreadyAttacked = false;
        isPatroling = false;
        PlayerSettings.KILLED_TROOPS += 1;
    }

    private void Update()
    {
        //Check for sight and attack range
        if (!IsDieded) {
            currentSightRange = detectedSightRange > 0 ? detectedSightRange : sightRange;
            playerInSightRange = Physics.CheckSphere(transform.position, currentSightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
            if (!playerInSightRange && !playerInAttackRange && !isRuningToSafePlace && !isCrouch) Patroling();
            if (playerInSightRange && !playerInAttackRange && !isRuningToSafePlace && !isCrouch) DetectPlayer();
            if (playerInAttackRange && playerInSightRange && !isRuningToSafePlace && !isCrouch) AttackPlayer();
            if (isRuningToSafePlace && !isCrouch) RunToSafePlace();
            if (isCrouch) Invoke(nameof(CheckDanger), 20);
        }
        animator.SetBool("isDetect", isDetect);
        animator.SetBool("isAttack", alreadyAttacked);
        animator.SetBool("isPatrolling", isPatroling);
        animator.SetBool("isRun", isRuningToSafePlace);
        animator.SetBool("isCrouch", isCrouch);
    }

    private void Patroling() {   
        isDetect = false;
        alreadyAttacked = false;
        if (!walkPointSet && !isPatroling) {
            walkPointSet = true;
            Invoke(nameof(SearchWalkPoint), 5);
            // Invoke(nameof(SetRandomDestination), 5);
        };

        if (detectedSightRange > 0) {
            Invoke(nameof(DetectedSightRangeToZero), 5);
        }

        if (isWalkPointFinded && !isPatroling) {
            isPatroling = true;
            navMeshAgent.SetDestination(walkPoint);
        }

        // Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        // if (distanceToWalkPoint.magnitude < 1f) {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f && isPatroling) {
            walkPointSet = false;
            isWalkPointFinded = false;
            if (walkPointNumber < navPoints.Count - 1) {
                walkPointNumber += 1;
            } else {
                walkPointNumber = 0;
            }
            isPatroling = false;
        }  
    }

    private void SearchWalkPoint() {
        //Calculate random point in range
        // float randomZ = Random.Range(-walkPointRange, walkPointRange) + 2f;
        // float randomX = Random.Range(-walkPointRange, walkPointRange) + 2f;

        // walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        Debug.Log("SetNavPoint 1111 " + navPoints.Count + "walkPointNumber " + walkPointNumber);
        walkPoint = new Vector3(navPoints[walkPointNumber].position.x, transform.position.y, navPoints[walkPointNumber].position.z);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        isWalkPointFinded = true;
    }

    private void DetectedSightRangeToZero() {
        detectedSightRange = 0;
    }

    private void RunToSafePlace() {
        isDetect = false;
        alreadyAttacked = false;
        isPatroling = false;
        isRuningToSafePlace = true;
        this.transform.LookAt(this.transform.position);
        navMeshAgent.speed = 3;
        Vector3 SavePointPosition = new Vector3(savePoints[0].position.x, transform.position.y, savePoints[0].position.z);
        navMeshAgent.SetDestination(SavePointPosition);
        if (this.transform.position == SavePointPosition) {
            navMeshAgent.speed = 1;
            isRuningToSafePlace = false;
            isCrouch = true;
        }
    }

    private void DetectPlayer() {
        if (!IsDieded) {
            LookAtObject(player);
            navMeshAgent.ResetPath();
            isDetect = true;
            alreadyAttacked = false;
            isPatroling = false;
            if (detectedSightRange == 0) {
                detectedSightRange = sightRange * 2f;
            }
            NeedRun();
        }
    }

    private void NeedRun() {
        if (Mathf.Abs(player.position.x - transform.position.x) < distanceWhenRun && Mathf.Abs(player.position.z - transform.position.z) < distanceWhenRun) {
            isRuningToSafePlace = true;
        }
    }

    private void LookAtObject(Transform obj) {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>()) {
            if (child.name == "weapon" || child.name == "mixamorig:Head") {
                child.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
            }
        }
        Vector3 targetPostition = new Vector3(obj.position.x, this.transform.position.y, obj.position.z);
        this.transform.LookAt(targetPostition);
        // this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }


    private void AttackPlayer() {
        isPatroling = false;
        LookAtObject(player);
        NeedRun();
        if (!alreadyAttacked)
        {
            foreach (var _gun in _guns) {
                _gun.Attack();
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void CheckDanger() {
        if (!playerInSightRange && !playerInAttackRange) {
            isCrouch = false;
            Patroling();
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage");
        health -= damage;
        if (health <= 0 && !IsDieded) IfDied();

        // if (health <= 0) Invoke(nameof(DestroyEnemy), 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnCollisionEnter(Collision collision) {
        Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();

        if (otherRb != null) {
            if (otherRb.mass > rb.mass) {
                otherRb.isKinematic = true;
            }
        }
    }
}
