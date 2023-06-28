using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject soldier;
    List<Transform> spawnPoints = new List<Transform>();

    public int numberOfSoldiers = 25;

    void Start()
    {
        foreach (Transform child in transform) {
            if (child.tag == "Spawn") {
                spawnPoints.Add(child.transform);
            }
        }
        SpawnRandomObjects();
    }

    void SpawnRandomObjects()
    {
        for (int i = 0; i < numberOfSoldiers; i++) {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];
                        Debug.Log("randomIndex " + randomIndex);
            GameObject soldierObj = Instantiate(soldier, spawnPoint.position, spawnPoint.rotation);
            foreach (Transform navPoint in spawnPoint) {
                if (navPoint.tag == "Points") {
                    navPoint.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    soldierObj.GetComponent<EnemyAiTutorial>().SetNavPoint(navPoint.transform);
                }
            }
        }
    }
}
