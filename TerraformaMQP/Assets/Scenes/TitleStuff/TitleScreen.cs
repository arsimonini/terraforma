using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //public String name;
    // Start is called before the first frame update

    public GameObject option1;
    public GameObject option2;

    public void LevelButton(string name) {
        SceneManager.LoadScene(name);
    }

    public void QuitButton() {
        Debug.Log("Quit Clicked");
        Application.Quit();
    }

    public void PlayButton() {
        option1.SetActive(false);
        option2.SetActive(true);
    }

    public void BackButton() {
        option1.SetActive(true);
        option2.SetActive(false);
    }
}
