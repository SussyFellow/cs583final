using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager instance;

    [Header("UI Panels")]
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject victoryMenu;
    public GameObject hudPanel;

    public bool isGamePaused = false;
    public bool isGameActive = false; //False until start is pressed

    void Awake()
    {
        //Singleton setup
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        //Freeze time immediately
        Time.timeScale = 0f; 
        
        //Show the start menu
        ShowMenu(startMenu);
        
        //Unlock the cursor so we can click buttons
        UnlockCursor();
    }

    void Update()
    {
        //Handle Pause Input
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            if (isGamePaused) ResumeGame();
            else PauseGame();
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        isGamePaused = false;
        ShowMenu(hudPanel);
        Time.timeScale = 1f; // Unfreeze time
        LockCursor();
    }

    public void PauseGame()
    {
        isGamePaused = true;
        ShowMenu(pauseMenu);
        Time.timeScale = 0f; //Freeze time
        UnlockCursor();
    }

    public void ResumeGame()
    {
        isGamePaused = false;
        ShowMenu(hudPanel);
        Time.timeScale = 1f;
        LockCursor();
    }

    public void Victory()
    {
        isGameActive = false;
        ShowMenu(victoryMenu);
        Time.timeScale = 0f;
        UnlockCursor();
    }

    public void GameOver()
    {
        isGameActive = false;
        ShowMenu(gameOverMenu);
        Time.timeScale = 0f;
        UnlockCursor();
    }

    public void RestartLevel()
    {
        //Reloads the current scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game...");
    }

    void ShowMenu(GameObject menuToShow)
    {
        //Hide all menys
        if(startMenu) startMenu.SetActive(false);
        if(pauseMenu) pauseMenu.SetActive(false);
        if(gameOverMenu) gameOverMenu.SetActive(false);
        if(victoryMenu) victoryMenu.SetActive(false);
        if(hudPanel) hudPanel.SetActive(false);

        //Show the one we want
        if (menuToShow != null)
            menuToShow.SetActive(true);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}