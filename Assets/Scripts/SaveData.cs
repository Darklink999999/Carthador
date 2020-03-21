using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string scene;

    [HideInInspector] public int hourOfDay;
    [HideInInspector] public int minuteOfDay;
    [HideInInspector] public string currentQuest;
    [HideInInspector] public string mainQuest;
    [HideInInspector] public int gamePhase;
    [HideInInspector] public List <string> completedQuests;


    public float [] playerPosition;
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public int playerMaxAether;
    public int playerCurrentAether;
    public List <string> inventoryItems;

    public Dictionary <string, string> currentlyEquipped;
    public Dictionary <string, List <string>> equipmentItems;

    public SaveData (MainCharacter player, Game game) {

        this.scene = game.lastLevel;
        this.hourOfDay = game.hourOfDay;
        this.minuteOfDay = game.minuteOfDay;
        this.currentQuest = game.currentQuest;
        this.mainQuest = game.mainQuest;
        this.gamePhase = game.gamePhase;
        this.completedQuests = game.completedQuests;
        this.equipmentItems = game.equipmentScript.items;
        this.currentlyEquipped = game.equipmentScript.currentlyEquipped;


        this.playerPosition = new float [3];
        this.playerPosition [0] = player.transform.position.x;
        this.playerPosition [1] = player.transform.position.y;
        this.playerPosition [2] = player.transform.position.z;
        this.playerMaxHealth = player.maxHealth;
        this.playerCurrentHealth = player.currentHealth;
        this.playerMaxAether = player.maxAether;
        this.playerCurrentAether = player.currentAether;

        this.inventoryItems = game.inventoryScript.items;

    }



}
