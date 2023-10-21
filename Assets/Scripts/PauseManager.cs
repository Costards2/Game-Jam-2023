using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]private bool isPaused = false;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            PauseGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            ResumeGame();
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Time.timeScale = 0f; 
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
    }
}
