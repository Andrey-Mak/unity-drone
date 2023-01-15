using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPauseGame;
    public GameObject PauseGameMenu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPauseGame) {
                Resume();
            } else {
                PauseGame();
            }
        }
    }

    public void Resume() {
        PauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        isPauseGame = false;
    }

    public void PauseGame() {
        PauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        isPauseGame = true;
    }
}
