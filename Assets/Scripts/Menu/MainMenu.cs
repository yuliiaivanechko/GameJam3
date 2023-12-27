using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button primaryMainButton;
    [SerializeField] private Button primaryOptionsButton;
    [SerializeField] private Button sliderBackground;
    [SerializeField] private Button backButton;

    void Start()
    {
        primaryMainButton.Select();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnNewGameClicked()
    {
        Debug.Log(DataPersistanceManager.instance);
        DataPersistanceManager.instance.NewGame();
        PlayGame();
    }

    public void OnLoadGameClicked()
    {
        DataPersistanceManager.instance.LoadGame();
        PlayGame();
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
