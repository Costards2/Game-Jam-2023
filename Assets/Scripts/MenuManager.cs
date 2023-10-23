using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialHud;
    [SerializeField] private GameObject basicHud;

    public void GameStart()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void Turorial()
    {
        basicHud.SetActive(false);
        tutorialHud.SetActive(true);

    }

    public void Voltar()
    {
        basicHud.SetActive(true);
        tutorialHud.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Sair()
    {
        Debug.Log("Sair");
        Application.Quit();
    }

}

