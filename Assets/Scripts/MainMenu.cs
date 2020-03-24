using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private Game game;
    private MainCharacter player;
    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent<Game>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>();
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


    public void Load()
    {

        SaveData data = SaveSystem.Load();

        if (game == null)
            game = Camera.main.GetComponent<Game>();

        game.lastLevel = data.scene;
        game.hourOfDay = data.hourOfDay;
        game.minuteOfDay = data.minuteOfDay;
        game.currentQuest = data.currentQuest;
        game.mainQuest = data.mainQuest;
        game.gamePhase = data.gamePhase;
        game.completedQuests = data.completedQuests;
        game.equipmentScript.items = data.equipmentItems;
        game.equipmentScript.currentlyEquipped = data.currentlyEquipped;

        Camera.main.GetComponent<Inventory>().items = data.inventoryItems;

        player.spawnPosition = new Vector3(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);

        player.maxHealth = data.playerMaxHealth;
        player.currentHealth = data.playerCurrentHealth;
        player.maxAether = data.playerMaxAether;
        player.currentAether = data.playerCurrentAether;

        player.gameJustLoaded = true;
        player.StartCoroutine(player.disableGameJustLoaded());

        game.StopAllCoroutines();


        UnityEngine.SceneManagement.SceneManager.LoadScene(data.scene);
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
