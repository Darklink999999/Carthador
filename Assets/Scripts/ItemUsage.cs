using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUsage : MonoBehaviour
{


    private MainCharacter player;
    public int healthPotionRecoveryAmount = 10;
    public int aetherPotionRecoveryAmount = 10;

    private Inventory inventory;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnLeftClick () {

        player =  GameObject.FindGameObjectWithTag ("Player").GetComponent <MainCharacter> ();
        inventory = Camera.main.GetComponent <Inventory>();
        GameObject currentItem = EventSystem.current.currentSelectedGameObject; 

        if (currentItem.name.Contains ("Health Potion") && !currentItem.GetComponent <Item> ().used) {
            
            if (player.currentHealth + healthPotionRecoveryAmount > player.maxHealth)
                player.currentHealth = player.maxHealth;
            else
                player.currentHealth += healthPotionRecoveryAmount;
            
            currentItem.GetComponent <Item> ().used = true;
        }
        else if (currentItem.name.Contains ("Aether Potion") && !currentItem.GetComponent <Item> ().used) {
            
            if (player.currentAether + aetherPotionRecoveryAmount > player.maxAether)
                player.currentAether = player.maxAether;
            else
                player.currentAether += aetherPotionRecoveryAmount;
            
             currentItem.GetComponent <Item> ().used = true;
        }


        int i;
        Int32.TryParse(currentItem.name.Substring (currentItem.name.Length - 1), out i);

        inventory.items [i] = "Empty";

       currentItem.GetComponent <Image> ().sprite = Resources.Load <Sprite> ("Inventory/EmptyImage");
    }


}
