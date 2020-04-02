using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [HideInInspector] public bool finishedTurn= false;

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

        if (this.currentHealth <= 0) {               

            game.spawnedEnemiesGameObjects.Remove(this.gameObject);
            game.allFightingObjects.Remove(this.gameObject);
            game.attackOrderGameObjects.Remove(this.gameObject);
            // FINISH BATTLE ////////////////////////
            if (game.spawnedEnemiesGameObjects.Count == 0) {

                game.state = "FinishedFight";
                game.spawnedEnemies = null;
                game.spawnedEnemiesGameObjects.Clear();
                game.allFightingObjects.Clear();
                game.attackOrderGameObjects.Clear();

                SceneManager.LoadScene ("World");
            }
            else
                game.advanceBattle ();

            Destroy (this.gameObject);
        }

    }

    public void attackFunc () {

        finishedTurn = true;
        
        int defenseFactor =  game.currentlyTargetedObjectInBattle.GetComponent<MainCharacter>().defense;
        if (game.currentlyTargetedObjectInBattle.GetComponent<MainCharacter> ().defending) {
           defenseFactor = (int) (defenseFactor * 1.5f);
           game.currentlyTargetedObjectInBattle.GetComponent<MainCharacter>().defending = false;
        }
            
        
        int damage = this.attack - defenseFactor;

        if (damage > 0)
            game.currentlyTargetedObjectInBattle.GetComponent<MainCharacter> ().currentHealth -= damage;
        
        game.advanceBattle ();
        finishedTurn = false;
    }


    public void defendFunc () {
        



    }
}
