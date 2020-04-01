using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Cameras;

public class Game : MonoBehaviour
{

    public int gamePhase = 0;
    public string mainQuest;
    public string currentQuest;

    public List <string> completedQuests;
    public int currentQuestPhase;

    public Text messages;
    public GameObject messagesParent;

    [HideInInspector] public GameObject inventory;
    [HideInInspector] public Inventory inventoryScript;
    [HideInInspector]public List <GameObject> inventoryItems;



    [HideInInspector] public GameObject equipmentPanel;
    [HideInInspector] public Transform equipmentContent;
    [HideInInspector] public Equipment equipmentScript;
    [HideInInspector] public Dictionary <string, List <string>> equipmentItems;
    [HideInInspector] public Dictionary <string, string> currentlyEquipped;


    [HideInInspector]public GameObject mainMenu;
    [HideInInspector]public GameObject innMenu;


    [HideInInspector] public bool isGamePaused = false;

    

    [HideInInspector]public GameObject player;
    private MainCharacter playerController;

    [HideInInspector] public string lastLevel = "World";
    
    [HideInInspector] public string state = "None";
     [HideInInspector] public string previousState = "None";

    [HideInInspector] public Image healthBar;
    [HideInInspector] public Image aetherBar;
    [HideInInspector] public Text timeText;


    public int hourOfDay = 12;
    public int minuteOfDay = 0;
    
    public GameObject globalLight;

    [HideInInspector] public GameObject settingsMenu;



    [HideInInspector] public List <GameObject> party;



    [HideInInspector] public string [] spawnedEnemies;

    [HideInInspector] public int [] spawnedEnemiesHeath;
    [HideInInspector] public int [] spawnedEnemiesAether;
    [HideInInspector] public int [] spawnedEnemiesAttacks;
    [HideInInspector] public int [] spawnedEnemiesDefenses;
    [HideInInspector] public int [] spawnedEnemiesSpeeds;
    [HideInInspector] public int [] spawnedEnemiesIntelligence;


    [HideInInspector] public List <GameObject> spawnedEnemiesGameObjects;
    [HideInInspector] public List <GameObject> allFightingObjects;
    [HideInInspector] public List <GameObject> attackOrderGameObjects;

    [HideInInspector] public GameObject currentGameObjectFighting;
    [HideInInspector] public GameObject currentlyTargetedObjectInBattle;
    [HideInInspector] public GameObject objectIndicator;

    // Start is called before the first frame update
    public void Start ()
    {
        Physics2D.IgnoreLayerCollision(8, 9);

        state = "None";
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<MainCharacter>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        aetherBar = GameObject.Find("AetherBar").GetComponent<Image>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();


        this.inventory = GameObject.Find("Inventory");
        this.inventory.SetActive(false);
        this.inventoryScript = this.GetComponent<Inventory>();
        this.inventoryItems = new List<GameObject>();


        this.equipmentScript = this.GetComponent<Equipment>();
        this.equipmentPanel = equipmentScript.equipmentPanel;
        this.equipmentContent = equipmentScript.equipmentContent;
        this.equipmentItems = equipmentScript.items;
        this.currentlyEquipped = equipmentScript.currentlyEquipped;



        messages = GameObject.Find("MessagesText").GetComponent<Text>();
        messagesParent = messages.transform.parent.gameObject;
        messagesParent.SetActive(false);


        mainMenu = GameObject.Find("MainMenu");
        mainMenu.SetActive(false);

        innMenu = GameObject.Find("InnMenu");
        innMenu.SetActive(false);

        completedQuests = new List<string>();


        party = new List <GameObject> ();
        spawnedEnemiesGameObjects = new List <GameObject> ();
        allFightingObjects = new List <GameObject> ();
        attackOrderGameObjects = new List <GameObject> ();


        SceneManager.sceneLoaded += OnLevelChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        
        if (this.equipmentPanel == null) {

            this.equipmentScript = this.GetComponent <Equipment>();
            this.equipmentPanel = equipmentScript.equipmentPanel;
            this.equipmentContent = equipmentScript.equipmentContent;
            this.equipmentItems = equipmentScript.items;
            this.currentlyEquipped = equipmentScript.currentlyEquipped;


        }

        healthBar.fillAmount = (float) playerController.currentHealth / playerController.maxHealth;
        aetherBar.fillAmount = (float) playerController.currentAether / playerController.maxAether;




        /////////////////////////////////////////////// TIME DISPLAY ///////////////////////////////////////////////////////

        if ((this.hourOfDay != 0 && this.hourOfDay != 12) || (this.hourOfDay == 0 && this.minuteOfDay != 0) || (this.hourOfDay == 12 && this.minuteOfDay != 0))
            if (this.hourOfDay == 12)
                timeText.text = "12:" + (this.minuteOfDay.ToString().Length == 1 ? "0" + this.minuteOfDay : this.minuteOfDay.ToString());
            else
                timeText.text = ((this.hourOfDay % 12).ToString ().Length == 1 ? "0" + (this.hourOfDay % 12).ToString() : (this.hourOfDay % 12).ToString ()) + ":" +  (this.minuteOfDay.ToString().Length == 1 ? "0" + this.minuteOfDay.ToString () : this.minuteOfDay.ToString ());
        else if (this.hourOfDay == 0 && this.minuteOfDay == 0)
            timeText.text = "OO:00";
        else if (this.hourOfDay == 12 && this.minuteOfDay == 0)
            timeText.text = "12:00";


        /////////////////////////////////////////////////////// MENUS /////////////////////////////////////////////////////

        if (Input.GetButtonDown ("Inventory") && this.state != "Talking" && this.state != "InMenu" && this.state != "InEquipment"){
            
            if (!inventory.activeSelf) {

                Screen.lockCursor = false;
                
                inventory.SetActive (true);
                this.isGamePaused = true;
                this.fillInventory ();

                this.state = "InInventory";

                Time.timeScale = 0;
                
            }
            else if (inventory.activeSelf) {

                Screen.lockCursor = true;

                inventory.SetActive (false);
                this.isGamePaused = false;
                this.clearInventory();
                this.state = "None";

                Time.timeScale = 1;
            }
        }



         if (Input.GetButtonDown ("Cancel") && this.state != "Talking" && this.state != "InInventory" && this.state != "InEquipment"){
            
            if (!mainMenu.activeSelf) {

                Screen.lockCursor = false;

                mainMenu.SetActive (true);
                this.isGamePaused = true;
                this.state = "InMenu";

                Time.timeScale = 0;
                
            }
            else if (mainMenu.activeSelf) {

                Screen.lockCursor = true;

                mainMenu.SetActive (false);
                this.isGamePaused = false;
                this.state = "None";

                Time.timeScale = 1;
            }
        }

        if (Input.GetButtonDown ("Equipment") && this.state != "Talking" && this.state != "InInventory" && this.state != "InMenu"){
            
            if (!equipmentPanel.activeSelf) {

                Screen.lockCursor = false;

                equipmentPanel.SetActive (true);
                this.isGamePaused = true;
                this.state = "InEquipment";
                fillEquipment ();

                Time.timeScale = 0;
                
            }
            else if (equipmentPanel.activeSelf) {

                Screen.lockCursor = true;

                equipmentPanel.SetActive (false);
                this.isGamePaused = false;
                this.state = "None";
                clearEquipment ();

                Time.timeScale = 1;
            }
        }


        ////////////////////////////////////////////////////// FIGHTING /////////////////////////////////////////////////////////////////////////////////
        if (this.state == "Fighting" && currentGameObjectFighting != null){

            if (spawnedEnemiesGameObjects.Contains (currentGameObjectFighting) && !currentGameObjectFighting.GetComponent<SimpleEnemy> ().finishedTurn)
                currentGameObjectFighting.GetComponent <SimpleEnemy> ().attackFunc ();
                
            objectIndicator.transform.position = currentlyTargetedObjectInBattle.transform.position + Vector3.up * 2;
        }




        ///////////////////////////////////////////////////// DAY / NIGHT UPDATES ////////////////////////////////////////////////////////////////////////

        if (SceneManager.GetActiveScene().name == "World" && skyboxMat != null && globalLight != null)
        {
            skyboxMat.SetFloat("_Rotation", Mathf.LerpAngle(skyboxMat.GetFloat("_Rotation"), degrees, Time.deltaTime));
            globalLight.GetComponent<Light>().intensity = Mathf.Lerp(globalLight.GetComponent<Light>().intensity, Mathf.Cos(Mathf.Deg2Rad * finalDegrees) + 0.2f, Time.deltaTime);
            skyboxMat.SetFloat("_Exposure", Mathf.Lerp(skyboxMat.GetFloat("_Exposure"), Mathf.Cos(Mathf.Deg2Rad * finalDegrees), Time.deltaTime));
            skyboxMat.SetColor("_Tint", Color.Lerp(skyboxMat.GetColor("_Tint"), new Color(globalLight.GetComponent<Light>().intensity * (1.8f - Mathf.Cos(Mathf.Deg2Rad * finalDegrees)), globalLight.GetComponent<Light>().intensity, globalLight.GetComponent<Light>().intensity), Time.deltaTime));
            //this.GetComponent<Camera>().backgroundColor = new Color(Mathf.Cos(Mathf.Deg2Rad * finalDegrees), Mathf.Cos(Mathf.Deg2Rad * finalDegrees) + 0.1f, Mathf.Cos(Mathf.Deg2Rad * finalDegrees) + 0.2f);
        }




        previousState = state;
    }

    public void advanceBattle () {

        int nextIndexOfFightingGameObject = attackOrderGameObjects.IndexOf (currentGameObjectFighting)+1;
        if (nextIndexOfFightingGameObject >= attackOrderGameObjects.Count)
            nextIndexOfFightingGameObject = 0;

        currentGameObjectFighting = attackOrderGameObjects [nextIndexOfFightingGameObject];
        if (party.Contains (currentlyTargetedObjectInBattle))
            currentlyTargetedObjectInBattle = spawnedEnemiesGameObjects [0];
        else
            currentlyTargetedObjectInBattle = party [0];
    }



///////////////////////////////////////////////// INVENTORY ////////////////////////////////////////////

    private void fillInventory () {

        if (inventoryScript.items.Count == 0)
            return;

        float panelWidth = Screen.width;
        float panelHeight = Screen.height;

        float squareRoot = Mathf.Sqrt (inventoryScript.maxItems);

        float itemSize = panelHeight / Mathf.Ceil (squareRoot) / 2;

        int columnsCounter = 1;
        int rowsCounter = 0;


        for (int i = 0;i < inventoryScript.items.Count; i++) {

            float posX = 0;
            float posY = 0;

            
            if (i == 0) {

                posX = (panelWidth / 2) - (panelWidth / 8);
                posY = (panelHeight / 2) - (panelHeight / 8);
            }
            else if (rowsCounter == 0) {
                posX = inventoryItems[i - 1].transform.position.x + itemSize;
                posY = inventoryItems [i - 1].transform.position.y;
            }
            else {
                posX = inventoryItems[i - (int) squareRoot].transform.position.x;
                posY = inventoryItems[i - (int) squareRoot].transform.position.y + itemSize;
            }
            


            string path = "Inventory/Empty";

            if ( i < inventoryScript.items.Count) {
                if (inventoryScript.items[i].Contains ("(Clone)"))
                    path = "Inventory/" + inventoryScript.items[i].Substring(0, inventoryScript.items[i].IndexOf ("(Clone)"));
                else
                 path = "Inventory/" + inventoryScript.items[i];
            }
            
            inventoryItems.Add ( GameObject.Instantiate (Resources.Load <GameObject> (path), inventory.transform) );
            inventoryItems[i].name = inventoryItems [i].name + " " + i;

            inventoryScript.items [i] = inventoryItems[i].name;
            
            if (inventoryScript.items [i].Contains ("Empty"))
                inventoryScript.items [i] = "Empty";

            inventoryItems[i].transform.position = new Vector3 (posX, posY, 0);
            
            
            inventoryItems[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemSize, itemSize);


            columnsCounter++;
            if (columnsCounter > squareRoot) {
                columnsCounter = 1;
                rowsCounter++;
            }
        }
    }


    private void clearInventory () {

        foreach (GameObject item in inventoryItems) {

            Destroy (item.gameObject);
        }

        inventoryItems.Clear();

    }



    ///////////////////////////////////////////////// EQUIPMENT ////////////////////////////////////////////
    
        [HideInInspector] public Transform helmet;
        [HideInInspector] public Transform shoulders;
        [HideInInspector] public Transform breastplate;
        [HideInInspector] public Transform gauntlets;
        [HideInInspector] public Transform ringLeft;
        [HideInInspector] public Transform ringRight;
        [HideInInspector] public Transform legs;
        [HideInInspector] public Transform greaves;
        [HideInInspector] public Transform boots;
        [HideInInspector] public Transform weaponLeft;
        [HideInInspector] public Transform weaponRight;

    private void fillEquipment () {

        equipmentItems = equipmentScript.items;
        currentlyEquipped = equipmentScript.currentlyEquipped;

        helmet =  equipmentPanel.transform.GetChild (0);
        shoulders =  equipmentPanel.transform.GetChild (1);
        breastplate =  equipmentPanel.transform.GetChild (2);
        gauntlets =  equipmentPanel.transform.GetChild (3);
        ringLeft =  equipmentPanel.transform.GetChild (4);
        ringRight =  equipmentPanel.transform.GetChild (5);
        legs =  equipmentPanel.transform.GetChild (6);
        greaves =  equipmentPanel.transform.GetChild (7);
        boots =  equipmentPanel.transform.GetChild (8);
        weaponLeft =  equipmentPanel.transform.GetChild (9);
        weaponRight =  equipmentPanel.transform.GetChild (10);

        helmet.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Helmet/" + currentlyEquipped ["helmet"]);
        helmet.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["helmet"];
        helmet.GetComponent <EquipmentItem> ().category = "helmet";
        

        // shoulders.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Shoulders/" + currentlyEquipped ["shoulders"]);
        // shoulders.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["shoulders"];
        // shoulders.GetComponent <EquipmentItem> ().category = "shoulders";

        breastplate.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Breastplate/" + currentlyEquipped ["breastplate"]);
        breastplate.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["breastplate"];
        breastplate.GetComponent <EquipmentItem> ().category = "breastplate";

        // gauntlets.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Gauntlets/" + currentlyEquipped ["gauntlets"]);
        // gauntlets.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["gauntlets"];
        // gauntlets.GetComponent <EquipmentItem> ().category = "gauntlets";

        // ringLeft.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Rings/" + currentlyEquipped ["ringLeft"]);
        // ringLeft.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["ringLeft"];
        // ringLeft.GetComponent <EquipmentItem> ().category = "ringLeft";

        // ringRight.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Rings/" + currentlyEquipped ["ringRight"]);
        // ringRight.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["ringRight"];
        // ringRight.GetComponent <EquipmentItem> ().category = "ringRight";

        // legs.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Legs/" + currentlyEquipped ["legs"]);
        // helmet.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["legs"];
        // helmet.GetComponent <EquipmentItem> ().category = "legs";

        // greaves.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Greaves/" + currentlyEquipped ["greaves"]);
        // greaves.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["greaves"];
        // greaves.GetComponent <EquipmentItem> ().category = "greaves";

        boots.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Boots/" + currentlyEquipped ["boots"]);
        boots.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["boots"];
        boots.GetComponent <EquipmentItem> ().category = "boots";

        // weaponLeft.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Weapons/" + currentlyEquipped ["weaponLeft"]);
        // weaponLeft.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["weaponLeft"];
        // weaponLeft.GetComponent <EquipmentItem> ().category = "weaponLeft";

        // weaponRight.GetComponent <Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/Weapons/" + currentlyEquipped ["weaponRight"]);
        // weaponRight.GetComponent <EquipmentItem> ().itemName = currentlyEquipped ["weaponRight"];
        // weaponRight.GetComponent <EquipmentItem> ().category = "weaponRight";
        
    }


    private void clearEquipment () {

        equipmentPanel.GetComponent <EquipmentPanel> ().clearScrollView ();
    }

















    public void OnLevelChanged (Scene scene, LoadSceneMode mode) {

        
        this.transform.position = player.transform.position;

        Screen.lockCursor = true;

        if (scene.name == "World")
            globalLight = GameObject.Find("GlobalLight");

        shadeObjects ();

        StartCoroutine (changeLastLevelnName(scene));

    }

    private IEnumerator changeLastLevelnName (Scene scene) {

        yield return new WaitForSeconds (0.1f);

        this.lastLevel = scene.name;

        if (this.lastLevel == "World")
            StartCoroutine(dayNightCycle());
        else 
            StopCoroutine(dayNightCycle());

        if (this.lastLevel.Length >= 6 && this.lastLevel.Substring(0,6) == "Combat"){
            
            int i = 0;
            Vector3 position = new Vector3 (0 ,0 ,0);
            Dictionary <int, GameObject> speeds = new Dictionary<int, GameObject> ();

            foreach (string enemy in spawnedEnemies) {
                
                int rightOffset = (i % 2) * -1 * 10;

                spawnedEnemiesGameObjects.Add (GameObject.Instantiate (Resources.Load <GameObject> ("Enemies/" + enemy), player.transform.position + player.transform.forward * 10 + player.transform.right * i, Quaternion.identity));
                allFightingObjects.Add (spawnedEnemiesGameObjects[i]);
                // SET STATS ////
                spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy> ().maxHealth = spawnedEnemiesHeath[i];
                spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy> ().maxAether = spawnedEnemiesAether[i];
                spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy> ().attack = spawnedEnemiesAttacks[i];
                spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy> ().defense = spawnedEnemiesDefenses[i];
                spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy> ().speed = spawnedEnemiesSpeeds[i];
                spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy> ().intelligence = spawnedEnemiesIntelligence[i];


                speeds.Add (spawnedEnemiesGameObjects[i].GetComponent <SimpleEnemy>().speed, spawnedEnemiesGameObjects[i]);

                i++;
            }

            foreach (GameObject player in party) {
                allFightingObjects.Add (player);
                speeds.Add (player.GetComponent<MainCharacter>().speed, player);
            }

            foreach (KeyValuePair <int,GameObject> s1 in speeds) {
                GameObject temp = new GameObject ();
                foreach (KeyValuePair <int,GameObject> s2 in speeds) {
                    if (s1.Key >= s2.Key && s1.Value != s2.Value && !attackOrderGameObjects.Contains (s1.Value))
                        temp = s1.Value;
                    else if (s1.Value != s2.Value && !attackOrderGameObjects.Contains (s2.Value))
                        temp = s2.Value;
                }

                if (!attackOrderGameObjects.Contains(temp))
                    attackOrderGameObjects.Add (temp);
            }

            player.GetComponent <MainCharacter>().spawnedEnemies = this.spawnedEnemiesGameObjects;
            currentGameObjectFighting = attackOrderGameObjects[0];
            
            if (party.Contains(currentGameObjectFighting))
                currentlyTargetedObjectInBattle = spawnedEnemiesGameObjects[(int) Random.Range(0, spawnedEnemiesGameObjects.Count - 1)];
            else
                currentlyTargetedObjectInBattle = party[(int) Random.Range(0, party.Count - 1)];

            objectIndicator = GameObject.Instantiate (Resources.Load <GameObject> ("ObjectIndicator"),currentlyTargetedObjectInBattle.transform.position + Vector3.up * 2, Quaternion.identity);

            Camera.main.transform.parent.parent.position = player.transform.position;
            Camera.main.transform.parent.localPosition = new Vector3 (0, 2, 0);
            Camera.main.transform.localPosition = new Vector3 (0, 0, -6);
            Camera.main.transform.LookAt (currentGameObjectFighting.transform.position);
        }   
    }



    private int seconds = 12 * 60 * 60 ;
    private IEnumerator dayNightCycle () {
        
        yield return new WaitForSeconds (20f/24f);

        this.minuteOfDay += 1;
        this.seconds += 60;

        if (this.seconds >= 3600 * 24)
            this.seconds = 0;
        
        
        if (minuteOfDay % 60 == 0)
        {
            this.minuteOfDay = 0;
            this.hourOfDay += 1;        }

        if (this.hourOfDay >= 24)
            this.hourOfDay = 0;

        shadeObjects ();

        this.StartCoroutine (dayNightCycle());

    }

    private float degrees = 0;
    private Material skyboxMat;
    
    float finalDegrees;
    private void shadeObjects (){

        if (SceneManager.GetActiveScene ().name == "World") {
           
            skyboxMat = this.GetComponent <Skybox> ().material;

            degrees = (((float)this.seconds) / 24f / 10f);

            float offset = (Mathf.Sign(degrees - 180) * 30);

            finalDegrees = degrees - (180 + offset);
        }
    }
}
