using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void startGame() {
        SceneManager.LoadSceneAsync(1);
    }

    public void exitGame() {
        Application.Quit();
    }
}
