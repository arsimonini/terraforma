using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public bool forDeselecting = false;

    public GameObject descriptionPage;
    public GameObject settingsPage;

    [SerializeField] private AudioClip[] openPauseMenu;
    [SerializeField] private AudioClip[] closePauseMenu;
    [SerializeField] private AudioClip[] flipPauseMenu;

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
        SFXController.instance.PlayRandomSFXClip(closePauseMenu, transform, 1f);
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        SFXController.instance.PlayRandomSFXClip(openPauseMenu, transform, 1f);
        descriptionPage.SetActive(true);
        settingsPage.SetActive(false);
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit() {
        
        SceneManager.LoadScene("Title");
        //Debug.Log("Quit Clicked");
        //Application.Quit();
    }

    public void Settings() {
        Debug.Log("Settings Clicked");
        SFXController.instance.PlayRandomSFXClip(flipPauseMenu, transform, 1f);
        descriptionPage.SetActive(false);
        settingsPage.SetActive(true);
    }

    public void setDeslection(bool b) {
        forDeselecting = b;
    }

    public void goBackButton(bool b) {
        descriptionPage.SetActive(true);
        settingsPage.SetActive(false);
        SFXController.instance.PlayRandomSFXClip(flipPauseMenu, transform, 1f);
    }
}
