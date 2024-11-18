using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public bool forDeselecting = false;

    public void Update() {
        if(GameIsPaused == true && Input.GetKeyUp(KeyCode.R)) {
            RestartLevel();
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        setDeslection(true);
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        Debug.Log("Quit Clicked");
        Application.Quit();
    }

    public void Settings() {
        Debug.Log("Settings Clicked");
    }

    public void setDeslection(bool b) {
        forDeselecting = b;
    }
}
