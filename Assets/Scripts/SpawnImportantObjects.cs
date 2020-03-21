using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnImportantObjects : MonoBehaviour
{

    private GameObject player;
    private  GameObject camera;

    private GameObject canvas;


    public void Awake () {

        camera = GameObject.Instantiate (Resources.Load <GameObject> ("Main Camera"),new Vector3 (0, 0, -10), Quaternion.identity);
        player = GameObject.Instantiate (Resources.Load <GameObject> ("Player"), new Vector3 (0, 0, 0), Quaternion.identity);
        canvas = GameObject.Instantiate (Resources.Load <GameObject> ("Canvas"));
    }
    // Start is called before the first frame update
    void Start()
    {

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

        SceneManager.LoadScene ("China");
    }

    public void OnStartMenu_LoadGame () {

        player.SetActive (true);
        camera.SetActive(true);
        canvas.SetActive (true);

        this.StartCoroutine (this.loadGame());
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

    }

}
