using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    public int damage = 10;

    public int maxHealth = 30;
    [HideInInspector] public int currentHealth;

    public int maxAether = 30;
    [HideInInspector] public int currentAether;

    private Game game;

    private GameObject player;
    private bool attacked = false;

    public int attack;

    public int defense;

    public int speed;

    public int intelligence;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        game = Camera.main.GetComponent<Game>();
        player = game.player;

        this.transform.LookAt (player.transform.position);
        this.transform.rotation =  Quaternion.Euler (0, this.transform.rotation.eulerAngles.y ,0);
    }


    public void Update () {

        ;

    }

    public void attackFunc () {
        int damage = this.attack - player.GetComponent<MainCharacter>().defense;

        if (damage > 0)
            player.GetComponent<MainCharacter> ().currentHealth -= damage;
        
        game.advanceBattle ();
    }


    public void defendFunc () {
        



    }
}
