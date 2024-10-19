using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Button startButton;
    public Button continueButton;
    public Button pauseButton;
    public Button quitButton;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        pauseButton.onClick.AddListener(OnContinueButtonClicked);
        pauseButton.onClick.AddListener(OnPauseButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);

    }

    void OnStartButtonClicked()
    {
        GameManager.instance.StartGame(); // ���� GameManager ��ʼ��Ϸ�ķ���
    }

    void OnContinueButtonClicked()
    {
        GameManager.instance.PauseGame(); // ���� GameManager ��ͣ��Ϸ�ķ���
    }

    void OnPauseButtonClicked()
    {
        GameManager.instance.PauseGame(); // ���� GameManager ��ͣ��Ϸ�ķ���
    }



    void OnQuitButtonClicked()
    {
        GameManager.instance.EndGame(); // ���� GameManager ������Ϸ�ķ���
    }


    void UpdateUIForCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        Debug.Log(currentScene);
        if (currentScene == "MainMenu")
        {
            startButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
        else if (currentScene == "MainGame")
        {
            startButton.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateUIForCurrentScene();
    }
}
