using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
 
    // Main Menu Buttons
    public Button newGameButton;
    public Button continueButton;
    public Button quitButton;
    public Button stopButton;
    public Button returnToMenuButton;
    public Button newGameFromOverButton;
    public Button quitFromOverButton;
    public Button closeButton;
    public Button NextLevelButton;
    public Button iaButton;

    public GameObject settingsPanel;
    public GameObject NextLevelPanel;
    public GameObject mainMenuUI;  // Main Menu UI
    public GameObject gameUI;       // Main Game UI
    public GameObject gameOverUI;   // Game Over UI
    public Padding padding;

    private ScreenOrientation lastOrientation;


    void Start()
    {
        // Add listeners for buttons
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        else
            Debug.LogWarning("New Game Button is not assigned!");

        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        else
            Debug.LogWarning("Continue Button is not assigned!");

        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        else
            Debug.LogWarning("Quit Button is not assigned!");

        if (stopButton != null)
            stopButton.onClick.AddListener(OnStopButtonClicked);
        else
            Debug.LogWarning("Stop Button is not assigned!");

        if (returnToMenuButton != null)
            returnToMenuButton.onClick.AddListener(OnReturnToMenuClicked);
        else
            Debug.LogWarning("Return to Menu Button is not assigned!");

        if (newGameFromOverButton != null)
            newGameFromOverButton.onClick.AddListener(OnNewGameFromOverClicked);
        else
            Debug.LogWarning("New Game From Over Button is not assigned!");

        if (quitFromOverButton != null)
            quitFromOverButton.onClick.AddListener(OnQuitFromOverClicked);
        else
            Debug.LogWarning("Quit From Over Button is not assigned!");

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseSettingsPanel);
        else
            Debug.LogWarning("Quit From Close Button is not assigned!");

        if (NextLevelButton != null)
            NextLevelButton.onClick.AddListener(CloseNextLevelPanel);
        else
            Debug.LogWarning("Quit From Next Button is not assigned!");

        if (iaButton != null)
            iaButton.onClick.AddListener(IAPlayer);
        else
            Debug.LogWarning("Quit From IA Button is not assigned!");

        // Set UI based on the current scene
        UpdateUIForCurrentScene();
    }

    
    void UpdateUIForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Log the current scene for debugging
        Debug.Log(currentScene);

        // Activate/deactivate UI elements based on the current scene
        if (currentScene == "MainMenu")
        {
            Debug.Log("Activating Main Menu UI");
            mainMenuUI.SetActive(true);

            if (gameUI != null)
                gameUI.SetActive(false);
            if (gameOverUI != null)
                gameOverUI.SetActive(false);
        }
        else if (currentScene == "MainGame")
        {
            Debug.Log("Activating Main Game UI");
            mainMenuUI.SetActive(false);
            settingsPanel.SetActive(false);
            if (gameUI != null)
                gameUI.SetActive(true);
            if (gameOverUI != null)
                gameOverUI.SetActive(false);
        }
        else if (currentScene == "GameOver")
        {
            Debug.Log("Activating Game Over UI");
            mainMenuUI.SetActive(false);

            if (gameUI != null)
                gameUI.SetActive(false);
            if (gameOverUI != null)
                gameOverUI.SetActive(true);
        }
        else
        {
            Debug.Log("No UI to activate for this scene");
        }
    }

    void OnNewGameButtonClicked()
    {
        // Start a new game and load the main game scene
        GameManager.instance.StartGame();
        SceneManager.LoadScene("MainGame");
    }

    void OnContinueButtonClicked()
    {

        string relativePath = "\\Script\\SaveData\\savegame.json";

        
        string savePath = Application.dataPath + relativePath;

        
        if (File.Exists(savePath))
        {
            // Continue the game
            GameManager.instance.ContinueGame();
            if (gameUI != null)
            {
                gameUI.SetActive(true);
            }
            mainMenuUI.SetActive(false);
        }

       
    }

    void OnQuitButtonClicked()
    {
        // Quit the application
        Application.Quit();
    }

    void OnStopButtonClicked()
    {
        //PauseGame
        settingsPanel.SetActive(true);
        GameManager.instance.PauseGame();
    }

    void OnReturnToMenuClicked()
    {
        // Return to the main menu
        GameManager.instance.ReturnToMainMenu();
        SceneManager.LoadScene("MainMenu");
    }

    void OnNewGameFromOverClicked()
    {
        // Start a new game from the Game Over screen
        GameManager.instance.StartGame();
        SceneManager.LoadScene("MainGame");
    }

    void OnQuitFromOverClicked()
    {
        // Quit the application from Game Over screen
        Application.Quit();
    }

    public void CloseSettingsPanel()
    {
        //Close pausa panel
        Debug.Log("ClosePanel");
        settingsPanel.SetActive(false);
        GameManager.instance.ResumeGame();
    }

    public void CloseNextLevelPanel()
    {
        //close next level panel
        Debug.Log("ClosePanel");
        NextLevelPanel.SetActive(false);
        GameManager.instance.ResumeGame();
    }

    public void OpenNextLevelPanel()
    {
        //open next level panel
        Debug.Log("ClosePanel");
        NextLevelPanel.SetActive(true);
        GameManager.instance.PauseGame();
    }

    public void IAPlayer()
    {
        //Active iaPlayer
        GameObject picture = GameObject.Find("IAButton/Button/ActiveAI");
        if (padding.isAutoMode) {
            padding.isAutoMode = false;
            picture.SetActive(false);


        }
        else
        {
            padding.isAutoMode = true;
            picture.SetActive(true);
        }
    }

    void Update()
    {
    }
}
