using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

public class MainCharacter : MonoBehaviour
{

    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;

    public int maxAether = 100;
    [HideInInspector]public int currentAether;


    public int attack = 10;

    public int defense = 10;
    [HideInInspector] public bool defending;
    public int speed = 10;  

    public int intelligence = 10;


    public int attackCost = 1;
    public int defenseCost = 5;


    private Inventory inventory;

    private Animator anim;
    private Vector3 scale;

    private float h;
    private float v;
    private float previousH;
    private float previousV;

    private GameObject arrow;

    private Game game;
    [HideInInspector] public bool gameJustLoaded = false;

    [HideInInspector] public bool attacked = false;
    [HideInInspector] public bool nearEnemy = false;

    [HideInInspector] public bool isDefending = false;
    
    [HideInInspector] public Vector3  spawnPosition;

    [HideInInspector] public GameObject companion;

    [HideInInspector] public List <GameObject> spawnedEnemies;
    [HideInInspector] public GameObject selectedEnemy;


    public void Start ()
    {
        currentHealth = maxHealth;
        currentAether = maxAether;

        game = Camera.main.GetComponent<Game>();
        game.party.Add (this.gameObject);

        inventory = game.GetComponent<Inventory>();

        anim = this.GetComponent<Animator>();

        companion = GameObject.FindGameObjectWithTag("Companion");

        selectedEnemy = new GameObject ();

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnLevelChanged;
    }


    private float selectedEnemyButtonTimeout = 0;
    // Update is called once per frame
    void Update()
    {

        if (game.isGamePaused)
            return;
        else if (game.state == "None" || game.state == "Fighting" || game.state == "NearNPC"){
            Time.timeScale = 1;
        }

        if (game.state == "Fighting" && game.currentGameObjectFighting != null){

            if (Input.GetAxisRaw ("Horizontal") > 0 && Time.time > selectedEnemyButtonTimeout) {

                game.currentlyTargetedObjectInBattle = game.allFightingObjects [game.allFightingObjects.IndexOf(game.currentlyTargetedObjectInBattle)+1];
                selectedEnemyButtonTimeout = Time.time + 0.25f;
            }
            else if (Input.GetAxisRaw ("Horizontal") < 0 && Time.time > selectedEnemyButtonTimeout) {
                game.currentlyTargetedObjectInBattle = game.allFightingObjects [game.allFightingObjects.IndexOf(game.currentlyTargetedObjectInBattle)-1];
                selectedEnemyButtonTimeout = Time.time + 0.25f;
            }

            selectedEnemy = game.currentlyTargetedObjectInBattle;

            if (selectedEnemy != this.gameObject) {

                this.transform.LookAt (selectedEnemy.transform.position);
                this.transform.rotation =  Quaternion.Euler (0, this.transform.rotation.eulerAngles.y ,0);
            }


            if (Input.GetButtonDown ("Attack") && game.currentGameObjectFighting == this.gameObject){

                
                SimpleEnemy c;

                if (selectedEnemy.TryGetComponent <SimpleEnemy> (out c)){
                    int damage = this.attack - c.defense;

                    if (damage > 0)
                        c.currentHealth -= damage;
                  
                  c.currentHealth -= damage;  
                 }
                
                game.advanceBattle ();
            }
            else if (Input.GetButtonDown ("Defend") && game.currentGameObjectFighting == this.gameObject) {

                this.defending = true;
                game.advanceBattle ();
            }
            // else if (Input.GetButtonDown ("Magic") && game.currentGameObjectFighting == this.gameObject)
            // else if (Input.GetButtonDown ("Items") && game.currentGameObjectFighting == this.gameObject)
            // else if (Input.GetButtonDown ("Flee") && game.currentGameObjectFighting == this.gameObject)

        }

        

        if (this.currentAether < 0)
            this.currentAether = 0;      

        
        if (this.currentHealth <= 0)
            this.loadGame ();
    }


    public void OnLevelChanged (Scene scene, LoadSceneMode mode) {


        if (game != null && GameObject.Find ("Player Spawn " + game.lastLevel) != null)
            spawnPosition = GameObject.Find ("Player Spawn " + game.lastLevel).transform.position;
        else if (!gameJustLoaded)
            spawnPosition = GameObject.Find ("Player Spawn").transform.position;
        
        this.transform.position = this.spawnPosition;
        Camera.main.transform.parent.parent.position = this.transform.position;
        companion.transform.position = this.transform.position + new Vector3 (1 ,1 ,1);
    }


    public IEnumerator disableGameJustLoaded()
    {
        yield return new WaitForSeconds(0.25f);

        this.gameJustLoaded = false;
    }

    public void loadGame () {

        SaveData data = SaveSystem.Load ();

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

        this.spawnPosition = new Vector3 (data.playerPosition [0], data.playerPosition [1], data.playerPosition [2]);

        this.maxHealth = data.playerMaxHealth;
        this.currentHealth = data.playerCurrentHealth;
        this.maxAether = data.playerMaxAether;
        this.currentAether = data.playerCurrentAether;


        SettingsSaveData settingsData = SettingsSaveSystem.Load();
        if (settingsData != null) {
            Screen.SetResolution(settingsData.resolution[0], settingsData.resolution[1], settingsData.fullscreen, settingsData.resolution[2]);
            QualitySettings.SetQualityLevel(settingsData.quality);
            AudioListener.volume = settingsData.volume;
            this.transform.parent.parent.GetComponent<FreeLookCam>().m_TurnSpeed = settingsData.cameraSensitivity;
        }



        this.gameJustLoaded = true;
        this.StartCoroutine (this.disableGameJustLoaded ());

        game.StopAllCoroutines();

        SceneManager.LoadScene (data.scene);


    }
}
