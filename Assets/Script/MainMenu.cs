using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int selectedLevel;
    [SerializeField] Button startGameButton;
    [SerializeField] bool isStartGame;
    [SerializeField] GameObject TitleSceenPanel, chapterPanel;

    private void Start()
    {
        startGameButton.interactable = false;
    }

    private void Update()
    {
        if(isStartGame) return;
        if(Input.anyKeyDown)
            isStartGame = true;

        if(isStartGame){
            chapterPanel.SetActive(true);
            TitleSceenPanel.SetActive(false);
        }
    }

    public void SelectLevel(int level)
    {
        selectedLevel = level;
        EnableStartGame();
    }

    public void EnableStartGame(){
        startGameButton.interactable = true;
    }

    public void StartGame()
    {
        if(selectedLevel < 1) return;
        SceneManager.LoadScene(selectedLevel);
    }

    public void ChangeStartGame(bool isStartGame) => this.isStartGame = isStartGame;
}
