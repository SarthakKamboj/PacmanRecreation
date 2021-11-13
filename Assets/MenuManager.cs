using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
