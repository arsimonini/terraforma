using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreditsScreen : MonoBehaviour
{
    public void LevelButton(string name) {
        SceneManager.LoadScene(name);
    }
}
