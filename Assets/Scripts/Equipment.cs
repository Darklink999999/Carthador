using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{

    [HideInInspector] public Dictionary <string, List <string>> items;
    [HideInInspector] public Dictionary <string, string> currentlyEquipped;


    [HideInInspector] public GameObject equipmentPanel;
    [HideInInspector] public Transform equipmentContent;


    // Start is called before the first frame update
    void Start()
    {

        items = new Dictionary <string, List <string>> ();
        currentlyEquipped = new Dictionary <string, string> ();
        equipmentPanel = GameObject.Find ("EquipmentPanel");
        equipmentContent = GameObject.Find ("EquipmentContent").transform;

        this.equipmentPanel.SetActive (false);

        currentlyEquipped.Add("helmet", "Leather Helmet");
        currentlyEquipped.Add("breastplate", "Leather Armor");
        currentlyEquipped.Add("boots", "Leather Boot");

        items.Add("helmet", new List <string> ());
        items.Add("breastplate", new List <string> ());
        items.Add("boots", new List <string> ());

        items ["helmet"].Add ("Iron Helmet");
        items ["breastplate"].Add ("Iron Armor");
        items ["boots"].Add ("Iron Boot");

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
