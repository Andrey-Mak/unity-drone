using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    void Start() {
        PlayerSettings.IS_GAME_OVER = false;
    }

    void Update() {
        if (PlayerSettings.IS_GAME_OVER) {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public static void onPlayerDamage(Collision target) {
        Debug.Log("onPlayerDamage: " + target.collider);
    }
}
