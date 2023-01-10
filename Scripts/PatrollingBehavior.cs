using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrollingBehavior : StateMachineBehaviour
{
   //  public int duration = 20;
   //  public string pointsTagName = "Points";
   //  float timer;
   //  List<Transform> points = new List<Transform>();
   //  NavMeshAgent agent;

   //  override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   //  {
   //     timer = 0;
   //     Debug.Log(stateInfo);
   //     Transform pointsObject = GameObject.FindGameObjectWithTag(pointsTagName).transform;
   //     foreach (Transform t in pointsObject) {
   //       points.Add(t);
   //     }

   //     agent = animator.GetComponent<NavMeshAgent>();
   //     agent.SetDestination(points[Random.Range(0, points.Count)].position);
   //  }

   //  override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   //  {
   //    Vector3 targetPosition = points[Random.Range(0, points.Count)].position;
   //     if (agent.remainingDistance <= agent.stoppingDistance) {
   //       animator.transform.LookAt(targetPosition);
   //       agent.SetDestination(targetPosition);
   //     }
   //     timer += Time.deltaTime;
   //     if (timer > Random.Range(10, 30)) {
   //        animator.SetBool("isPatrolling", false);
   //     }
   //  }

   //  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   //  {
   //     agent.SetDestination(agent.transform.position);
   //  }
}
