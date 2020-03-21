using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    
        public string state = "None";

    [HideInInspector] public Image healthBar;
    [HideInInspector] public Image aetherBar;
    [HideInInspector] public Text timeText;


    public int timeOfDay = 12;


    // Start is called before the first frame update


    void Start()
    {

        Physics2D.IgnoreLayerCollision(8, 9);

        state = "None";
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<MainCharacter>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        aetherBar = GameObject.Find("AetherBar").GetComponent<Image>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();


        this.inventory = GameObject.Find ("Inventory");
        this.inventory.SetActive(false);
        this.inventoryScript = this.GetComponent<Inventory>();
        this.inventoryItems = new List <GameObject> ();


        this.equipmentScript = this.GetComponent <Equipment>();
        this.equipmentPanel = equipmentScript.equipmentPanel;
        this.equipmentContent = equipmentScript.equipmentContent;
        this.equipmentItems = equipmentScript.items;
        this.currentlyEquipped = equipmentScript.currentlyEquipped;



        messages = GameObject.Find("MessagesText").GetComponent<Text>();
        messagesParent = messages.transform.parent.gameObject;
        messagesParent.SetActive(false);
        

        mainMenu = GameObject.Find ("MainMenu");
        mainMenu.SetActive(false);

        innMenu = GameObject.Find ("InnMenu");
        innMenu.SetActive(false);



        completedQuests = new List <string> ();

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

        if (this.timeOfDay != 0 && this.timeOfDay != 12)
            timeText.text = (this.timeOfDay % 12).ToString() + ":00";
        else if (this.timeOfDay == 0)
            timeText.text = "OO:00";
        else
            timeText.text = "12:00";

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

        shadeObjects ();

        //StartCoroutine (changeLastLevelnName(scene));

    }

    private IEnumerator changeLastLevelnName (Scene scene) {

        yield return new WaitForSeconds (0.1f);

        this.lastLevel = scene.name;

        if (this.lastLevel == "World")
            StartCoroutine (dayNightCycle ());
        else 
            StopCoroutine (dayNightCycle ());
    }


    private IEnumerator dayNightCycle () {
        
        yield return new WaitForSeconds ((20f*60)/24);

        this.timeOfDay += 1;

        if (this.timeOfDay >= 24)
            this.timeOfDay = 0;

        shadeObjects ();

        this.StartCoroutine (dayNightCycle());

    }

    private void shadeObjects (){

        if (SceneManager.GetActiveScene ().name == "World") {
           
            GameObject globalLight = GameObject.Find ("GlobalLight");
            Material skyboxMat = this.GetComponent <Skybox> ().material;

            if (this.timeOfDay >= 0 && this.timeOfDay <= 5) {
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (0, 0 ,0 ));
            }
            else if (this.timeOfDay >= 6 && this.timeOfDay <= 8) {
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (25 * (this.timeOfDay % 5),  0,0 ));
            }
            else if (this.timeOfDay >= 9 && this.timeOfDay <= 17) {
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (90, 0,0 ));
            }
            else if (this.timeOfDay >= 18 && this.timeOfDay <= 23) {
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (90 - this.timeOfDay * 4f, 0 ,0 ));
            }



            if (this.timeOfDay >= 0 && this.timeOfDay <= 5) {
                skyboxMat.SetColor("_Tint", new Color (0.05f, 0.05f, 0.05f));
            }
            else if (this.timeOfDay >= 6 && this.timeOfDay <= 8) {
                skyboxMat.SetColor("_Tint", new Color (0.5f,  0.25f + (0.15f * (this.timeOfDay % 5f))/2f , 0.25f + (0.15f * (this.timeOfDay % 5f))/2f));
            }
            else if (this.timeOfDay >= 9 && this.timeOfDay <= 17) {
                skyboxMat.SetColor("_Tint", Color.white);
            }
            else if (this.timeOfDay >= 18 && this.timeOfDay <= 20) {
                skyboxMat.SetColor("_Tint", new Color (0.5f,  0.5f - (0.15f * (this.timeOfDay % 18f))/2f, 0.5f - (0.15f * (this.timeOfDay % 18f))/2f));
            }
            else if (this.timeOfDay >= 21 && this.timeOfDay <= 23) {
                skyboxMat.SetColor("_Tint", new Color (0.15f - (0.05f * (this.timeOfDay % 21f)),  0.15f - (0.05f * (this.timeOfDay % 21f)), 0.15f - (0.05f * (this.timeOfDay % 21f))));
            }
        }
    }
}
