using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanel : MonoBehaviour
{

    private Game game;

    private List <GameObject> scrollViewItems;

    [HideInInspector] public GameObject currentlyEquippedSelected;

    // Start is called before the first frame update
    void Start()
    {

        currentlyEquippedSelected = new GameObject ();
        game = Camera.main.GetComponent <Game>();
        scrollViewItems = new List <GameObject> ();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void helmetButton () {

        populateScrollView ("helmet");

    }

    public void shouldersButton () {

        populateScrollView ("shoulders");


    }

    public void breastplateButton () {

        populateScrollView ("breastplate");


    }

    public void gauntletsButton () {

        populateScrollView ("gauntlets");


    }

    public void ringLeftButton () {

        populateScrollView ("ringLeft");


    }

    public void ringRightButton () {

        populateScrollView ("ringRight");


    }

    public void legsButton () {

        populateScrollView ("legs");


    }

    public void greavesButton () {

        populateScrollView ("greaves");


    }

    public void bootsButton () {

        populateScrollView ("boots");


    }

    public void weaponLeftButton () {

        populateScrollView ("weaponLeft");


    }

    public void weaponRightButton () {

        populateScrollView ("weaponRight");


    }


    private void populateScrollView (string category){

        currentlyEquippedSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        this.clearScrollView ();

        string generalCategory = category;

        if (category == "ringLeft" || category == "ringRight")
            generalCategory = "rings";
        else if (category == "weaponLeft" || category == "weaponRight") 
            generalCategory = "weapons";


        List <string> l = game.equipmentItems [category];
            
        foreach (string s in l){

            GameObject g = GameObject.Instantiate (Resources.Load <GameObject> ("Equipment/EquipmentButton"), game.equipmentContent);
            scrollViewItems.Add (g);

            g.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ s);
            g.GetComponent<Button> ().onClick.AddListener (changeEquipment);
            g.GetComponent<EquipmentItem> ().category = category;
            g.GetComponent<EquipmentItem> ().itemName = s;
        }        
    }



    public void changeEquipment () {

        GameObject selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        string category = selected.GetComponent<EquipmentItem>().category;
        string name = selected.GetComponent<EquipmentItem>().itemName;


        string generalCategory = category;

        if (category == "ringLeft" || category == "ringRight")
            generalCategory = "rings";
        else if (category == "weaponLeft" || category == "weaponRight") 
            generalCategory = "weapons";

        game.currentlyEquipped [name] = category;
        game.equipmentScript.currentlyEquipped = game.currentlyEquipped;

        string tempName = currentlyEquippedSelected.GetComponent <EquipmentItem> ().itemName;
        game.currentlyEquipped [category] = name;
        print (game.equipmentItems [category] [game.equipmentItems [category].IndexOf (name)]);
        game.equipmentItems [category] [game.equipmentItems [category].IndexOf (name)] = tempName;

        if (selected.GetComponent <EquipmentItem> ().category == "helmet") {

            selected.GetComponent<Image>().overrideSprite = game.helmet.GetComponent<Image> ().overrideSprite;

            game.helmet.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.helmet.GetComponent<EquipmentItem>().itemName;
            game.helmet.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "shoulders") {

            selected.GetComponent<Image>().overrideSprite = game.shoulders.GetComponent<Image> ().overrideSprite;

            game.shoulders.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.shoulders.GetComponent<EquipmentItem>().itemName;
            game.shoulders.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "breastplate") {

            selected.GetComponent<Image>().overrideSprite = game.breastplate.GetComponent<Image> ().overrideSprite;

            game.breastplate.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.breastplate.GetComponent<EquipmentItem>().itemName;
            game.breastplate.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "gauntlets") {

            selected.GetComponent<Image>().overrideSprite = game.gauntlets.GetComponent<Image> ().overrideSprite;

            game.gauntlets.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.gauntlets.GetComponent<EquipmentItem>().itemName;
            game.gauntlets.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "ringLeft") {

            selected.GetComponent<Image>().overrideSprite = game.ringLeft.GetComponent<Image> ().overrideSprite;

            game.ringLeft.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.ringLeft.GetComponent<EquipmentItem>().itemName;
            game.ringLeft.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "ringRight") {

            selected.GetComponent<Image>().overrideSprite = game.ringRight.GetComponent<Image> ().overrideSprite;

            game.ringRight.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.ringRight.GetComponent<EquipmentItem>().itemName;
            game.ringRight.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "legs") {

            selected.GetComponent<Image>().overrideSprite = game.legs.GetComponent<Image> ().overrideSprite;

            game.legs.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.legs.GetComponent<EquipmentItem>().itemName;
            game.legs.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "greaves") {

            selected.GetComponent<Image>().overrideSprite = game.greaves.GetComponent<Image> ().overrideSprite;

            game.greaves.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.greaves.GetComponent<EquipmentItem>().itemName;
            game.greaves.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "boots") {

            selected.GetComponent<Image>().overrideSprite = game.boots.GetComponent<Image> ().overrideSprite;

            game.boots.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.boots.GetComponent<EquipmentItem>().itemName;
            game.boots.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "weaponLeft") {

            selected.GetComponent<Image>().overrideSprite = game.weaponLeft.GetComponent<Image> ().overrideSprite;

            game.weaponLeft.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.weaponLeft.GetComponent<EquipmentItem>().itemName;
            game.weaponLeft.GetComponent<EquipmentItem>().itemName = tempName;
        }
        else if (selected.GetComponent <EquipmentItem> ().category == "weaponRight") {

            selected.GetComponent<Image>().overrideSprite = game.weaponRight.GetComponent<Image> ().overrideSprite;

            game.weaponRight.GetComponent<Image> ().overrideSprite = Resources.Load <Sprite> ("Equipment/" + char.ToUpper(generalCategory [0]) + generalCategory.Substring (1) +"/"+ name);

            tempName = name;
            selected.GetComponent<EquipmentItem>().itemName = game.weaponRight.GetComponent<EquipmentItem>().itemName;
            game.weaponRight.GetComponent<EquipmentItem>().itemName = tempName;
        }


        game.equipmentScript.items = game.equipmentItems;
        game.equipmentScript.currentlyEquipped = game.currentlyEquipped;
    }

    public void clearScrollView () {
        
        if (scrollViewItems.Count > 0) {
            foreach (GameObject g in scrollViewItems)
                Destroy (g);
            
            scrollViewItems.Clear();
        }
    }




}
