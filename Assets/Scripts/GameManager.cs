using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerMan;
    public GameObject playerWoman;

    void Awake()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
    
    }

    public void PlayMan()
    {
        Time.timeScale = 1f;
        playerMan.SetActive(true);
        Cursor.visible = false;
    }

    public void PlayWoman()
    {
        Time.timeScale = 1f;
        playerWoman.SetActive(true);
        Cursor.visible = false;
    }
}

