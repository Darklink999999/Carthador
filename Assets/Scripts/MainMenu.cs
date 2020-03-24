using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private Game game;
    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Resume ()
    {
        Screen.lockCursor = true;

        game.mainMenu.SetActive(false);
        game.isGamePaused = false;
        game.state = "None";

        Time.timeScale = 1;
    }


    public void Save () {

        SaveSystem.Save (GameObject.FindGameObjectWithTag ("Player").GetComponent <MainCharacter>(), Camera.main.GetComponent <Game>());
    }


    public void Settings ()
    {

        game.settingsMenu.SetActive(true);
        this.gameObject.SetActive(false);

    }


    public void Exit ()
    {

        Application.Quit();
    }

}
