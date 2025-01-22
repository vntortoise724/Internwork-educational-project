using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;

    public GameObject canvasUI;
    public GameObject pauseMenuUI;
    public GameObject optionMenuUI;

    private void Awake()
    {
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        canvasUI.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    void Pause()
    {
        canvasUI.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void GoToOption(int index)
    {
        if (index == 0)
        {
            optionMenuUI.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
        else 
        {
            optionMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
        
    }
}
