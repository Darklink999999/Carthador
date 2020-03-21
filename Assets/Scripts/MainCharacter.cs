using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharacter : MonoBehaviour
{

    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;

    public int maxAether = 100;
    [HideInInspector]public int currentAether;

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

    private GameObject playerLight;

    [HideInInspector] public GameObject companion;

    // Start is called before the first frame update


    void Start()
    {
        
        currentHealth = maxHealth;
        currentAether = maxAether;

        game = Camera.main.GetComponent<Game>();
        inventory = game.GetComponent <Inventory> ();

        anim = this.GetComponent<Animator>();

        playerLight = this.transform.GetChild (0).gameObject;

        companion = GameObject.FindGameObjectWithTag ("Companion");

         DontDestroyOnLoad (this.gameObject);

         SceneManager.sceneLoaded += OnLevelChanged;
    }

    // Update is called once per frame
    void Update()
    {

        if (game.isGamePaused)
            return;
        else if (game.state == "None" || game.state == "Fighting" || game.state == "NearNPC"){
            Time.timeScale = 1;
        }


        if ((game.hourOfDay >= 19 || game.hourOfDay <= 7) && !playerLight.activeSelf)
            playerLight.SetActive (true);
        else if ((game.hourOfDay >= 8 && game.hourOfDay <= 18) && playerLight.activeSelf)
            playerLight.SetActive (false);


        /// <summary>
        /// ///////////////////////////// AETHER ATTACK ////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        

        if (Input.GetButtonDown("Fire1") && this.currentAether >= attackCost && game.state == "ReadyToFight")
        {
            this.currentAether -= attackCost;
            GameObject.Instantiate(Resources.Load("AetherBullet"), this.transform.position + Vector3.up +   Camera.main.transform.forward.normalized, Quaternion.identity);

        }

        // AETHER DEFENSE //
        if (Input.GetButtonDown("Fire2") && this.currentAether >= defenseCost)
        {
            this.isDefending = true;
            this.currentAether -= defenseCost;
            GameObject.Instantiate(Resources.Load("AetherDefense"), this.transform);

        }
        
        if (this.currentHealth <= 0)
            SceneManager.LoadScene("China");

        if (this.currentAether < 0)
            this.currentAether = 0;
    }




    public void FixedUpdate()
    {
    }


    public void OnCollisionEnter(Collision collision)
    {
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

        this.gameJustLoaded = true;
        this.StartCoroutine (this.disableGameJustLoaded ());


        SceneManager.LoadScene (Camera.main.GetComponent<Game>().lastLevel);


    }
}
