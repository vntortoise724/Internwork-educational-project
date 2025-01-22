using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu, optionMenu;

    public void GoToOption()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void GoToMenu()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
