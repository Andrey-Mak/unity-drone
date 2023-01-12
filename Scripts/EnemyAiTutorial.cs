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

    public float distanceWhenRun = 2f;

    public float health = 2f;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

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

    private Rigidbody[] _rigidbodies;
    private EnemyGun[] _guns;
    private float detectedSightRange = 0f;
    private float currentSightRange;

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
                    point.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                child.DetachChildren();
            }
        }

        foreach (GameObject savePoint in GameObject.FindGameObjectsWithTag("SavePoints")) {
            savePoints.Add(savePoint.transform);
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
        if (!walkPointSet) {
            Invoke(nameof(SearchWalkPoint), 5);
        };

        if (detectedSightRange > 0) {
            Invoke(nameof(DetectedSightRangeToZero), 5);
        }

        if (walkPointSet) {
            isPatroling = true;
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

    private void SearchWalkPoint() {
        //Calculate random point in range
        // float randomZ = Random.Range(-walkPointRange, walkPointRange) + 2f;
        // float randomX = Random.Range(-walkPointRange, walkPointRange) + 2f;

        // walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        walkPoint = new Vector3(navPoints[walkPointNumber].position.x, transform.position.y, navPoints[walkPointNumber].position.z);

        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
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
        agent.speed = 3;
        Vector3 SavePointPosition = new Vector3(savePoints[0].position.x, transform.position.y, savePoints[0].position.z);
        agent.SetDestination(SavePointPosition);
        if (this.transform.position == SavePointPosition) {
            agent.speed = 1;
            isRuningToSafePlace = false;
            isCrouch = true;
        }
    }

    private void DetectPlayer() {
        if (!IsDieded) {
            LookAtObject(player);
            agent.ResetPath();
            isDetect = true;
            alreadyAttacked = false;
            isPatroling = false;
            if (detectedSightRange == 0) {
                detectedSightRange = sightRange * 2f;
            }
            if (Mathf.Abs(player.position.x - transform.position.x) < distanceWhenRun && Mathf.Abs(player.position.z - transform.position.z) < distanceWhenRun) {
                isRuningToSafePlace = true;
            }
        }
    }

    private void LookAtObject(Transform obj) {

        // Debug.Log(this.transform.eulerAngles.x + "; " + this.transform.eulerAngles.y + "; " + this.transform.eulerAngles.z);

        foreach (Transform child in transform.GetComponentsInChildren<Transform>()) {
            if (child.name == "weapon" || child.name == "mixamorig:Head") {
                // Vector3 relativePos = player.position - child.position;
                // Debug.Log("child.eulerAngles: " + child.eulerAngles.x + "; " + child.eulerAngles.y + "; " + child.eulerAngles.z);
                // Quaternion rotation = Quaternion.LookRotation(relativePos);
                // Quaternion current = child.localRotation;
                // child.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 1);
                // child.rotation = rotation;
                child.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
                // child.eulerAngles = new Vector3(child.eulerAngles.x, child.eulerAngles.y + 80, child.eulerAngles.z);
                // child.LookAt(new Vector3(obj.position.x, obj.position.y, obj.position.z));
            }
        }
        Vector3 targetPostition = new Vector3(obj.position.x, this.transform.position.y, obj.position.z);
        this.transform.LookAt(targetPostition);
        // this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }


    private void AttackPlayer() {
        //Make sure enemy doesn't move
        // agent.SetDestination(transform.position);
        isPatroling = false;
        LookAtObject(player);

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
