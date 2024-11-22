using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    //public String name;
    // Start is called before the first frame update

    public void LevelButton(string name) {
        SceneManager.LoadScene(name);
    }

    public void QuitButton() {
        Debug.Log("Quit Clicked");
        Application.Quit();
    }
}
