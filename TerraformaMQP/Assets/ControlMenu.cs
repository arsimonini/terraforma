using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    //public String name;
    // Start is called before the first frame update

    public void LevelButton(string name) {
        SceneManager.LoadScene(name);
    }
}
