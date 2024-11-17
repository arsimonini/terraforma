using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    public float fixedDeltaTime;

    void Start() 
    {
        fixedDeltaTime = Time.fixedDeltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(GameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
        
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }

    void Pause() {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;

            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }

    public void Quit() {
        Debug.Log("Quit Clicked");
        Application.Quit();
    }

    public void Settings() {
        Debug.Log("Settings Clicked");
    }
}
