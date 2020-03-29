using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnImportantObjects : MonoBehaviour
{

    [HideInInspector] public GameObject player;
    [HideInInspector]public  GameObject camera;
	private GameObject enemy;

    private GameObject canvas;

    private GameObject companion;

    [HideInInspector] public GameObject startMenu;
    private GameObject settingstMenu;


    public void Awake () {

        camera = GameObject.Instantiate (Resources.Load <GameObject> ("Main Camera"),new Vector3 (0, 0, -10), Quaternion.identity);
        player = GameObject.Instantiate (Resources.Load <GameObject> ("Player"), new Vector3 (0, 0, 0), Quaternion.identity);
        canvas = GameObject.Instantiate (Resources.Load <GameObject> ("Canvas"));
        companion = GameObject.Instantiate (Resources.Load <GameObject> ("Companions/Sidekick"), player.transform.position, Quaternion.identity);
		//enemy = GameObject.Instantiate (Resources.Load <GameObject> ("EnemyBall"));

        startMenu = GameObject.Find("StartMenu");

        settingstMenu = new GameObject();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Camera.main.GetComponent <Game> ().isGamePaused = true;
        settingstMenu = GameObject.Find("Settings");
        settingstMenu.SetActive(false);

        Camera.main.GetComponent<Game>().settingsMenu = this.settingstMenu;



        StartCoroutine (deactivateMainObjets());       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartMenu_NewGame () {

        player.SetActive (true);
        camera.SetActive(true);
        canvas.SetActive (true);
        companion.SetActive (true);

        Camera.main.GetComponent <Game> ().isGamePaused = false;

        SceneManager.LoadScene ("World");
    }

    public void OnStartMenu_LoadGame () {

        player.SetActive (true);
        camera.SetActive(true);
        canvas.SetActive (true);
        companion.SetActive (true);

        Camera.main.GetComponent <Game> ().isGamePaused = false;

        this.StartCoroutine (this.loadGame());
    }


    public void OnStartMenu_Settings()
    {

        startMenu.SetActive(false);
        settingstMenu.SetActive (true);
    }




    public void OnStartMenu_ExitGame () {
        
        Application.Quit ();

    }


    public IEnumerator loadGame () {

        yield return new WaitForSeconds (0.1f);

        player.GetComponent <MainCharacter>().loadGame ();

    }

    public IEnumerator deactivateMainObjets () {

        yield return new WaitForSeconds (0.1f);

        player.SetActive (false);
        camera.SetActive (false);
        canvas.SetActive (false);
        companion.SetActive (false);

    }

}
