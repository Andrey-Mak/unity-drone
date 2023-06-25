using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public TextMeshProUGUI killedTroopsText;

    void Start() {
        PlayerSettings.IS_GAME_OVER = false;
        PlayerSettings.BOOLET_COUNT = 10;
        PlayerSettings.KILLED_TROOPS = 0;
    }

    void Update() {
        killedTroopsText.text = "Killed: " + PlayerSettings.KILLED_TROOPS;
        if (PlayerSettings.BOOLET_COUNT <= 0) {
            Invoke(nameof(GameOver), 5);
        }
        if (PlayerSettings.IS_GAME_OVER) {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public static void onPlayerDamage(Collision target) {
        Debug.Log("onPlayerDamage: " + target.collider);
    }

    private void GameOver() {
        PlayerSettings.IS_GAME_OVER = true;
    }
}
