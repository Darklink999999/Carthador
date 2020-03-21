using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string scene;

    public int timeOfDay;
    public string currentQuest;
    public string mainQuest;
    public int gamePhase;
    public List <string> completedQuests;


    public float [] playerPosition;
    public int playerMaxHealth;
    public int playerCurrentHealth;
    public int playerMaxAether;
    public int playerCurrentAether;
    public List <string> inventoryItems;
    public SaveData (MainCharacter player, Game game) {

        this.scene = game.lastLevel;
        this.timeOfDay = game.timeOfDay;
        this.currentQuest = game.currentQuest;
        this.mainQuest = game.mainQuest;
        this.gamePhase = game.gamePhase;
        this.completedQuests = game.completedQuests;

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
