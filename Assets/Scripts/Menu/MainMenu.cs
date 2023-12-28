using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button primaryMainButton;
    [SerializeField] private Button primaryOptionsButton;
    [SerializeField] private Button sliderBackground;
    [SerializeField] private Button backButton;
    [SerializeField] private Button continueButton;

    void Start()
    {
        primaryMainButton.Select();
        if (!DataPersistanceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
            TMP_Text text = continueButton.GetComponentInChildren<TMP_Text>();
            text.color = Color.grey;
        }
    }

    public void OnNewGameClicked()
    {
        DataPersistanceManager.instance.NewGame();
        SceneManager.LoadSceneAsync(1);
    }

    public void OnLoadGameClicked()
    {
        Debug.Log("Loading");
        SceneManager.LoadSceneAsync(DataPersistanceManager.instance.gameData.sceneName);
    }


    public void SelectPrimaryOptionsButton()
    {
        primaryOptionsButton.Select();
    }

    public void SelectPrimaryBackButton()
    {
        backButton.Select();
    }

    public void SelectPrimaryMainButton()
    {
        primaryMainButton.Select();
    }

    public void QuitGame()
    {
        DataPersistanceManager.instance.SaveGame();
        Application.Quit();
    }

}
