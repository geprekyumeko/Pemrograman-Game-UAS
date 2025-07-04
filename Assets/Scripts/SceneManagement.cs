using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    //Tombol Scene Menu
    public void PlayButton()
    {
        SceneManager.LoadScene("PilihanLevel");
    }
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }

    //Tombol Scene Pilihan Level
    public void Level1Button()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void Level2Button()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void Level3Button()
    {
        SceneManager.LoadScene("Level 3");
    }


    //Tombol Scene Level 1
    public void RetryButton()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void MenuButton()
    {
        SceneManager.LoadScene("Menu");
    }
}
