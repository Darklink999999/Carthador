using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginCombat : MonoBehaviour
{

    private Game game;
    public string [] spawnedEnemies;
    public int [] health;
    public int [] aether;
    public int []   attacks;
    public int []   defenses;
    public int []   speeds;
    public int []   intelligence;
    public string combatScene = "World";

    public float moveRadius = 5;
    public float waitTime = 3;
    public float detectionRadius = 10;

    private Vector3 startPosition;
    private Vector3 nextPosition;
    private bool nextPositionReached = false;


    private GameObject player;
    private bool nearPlayer = false;
    private bool previousNearPlayer;

    


    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent <Game>();

        game = Camera.main.GetComponent<Game>();
        player = game.player;


        startPosition = this.transform.position;
        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), this.transform.position.y, Random.Range(startPosition.z - moveRadius, startPosition.z + moveRadius));
        
    }


    public void Update () {


        if (game.state == "Fighting" || game.isGamePaused)
            return;


        if (Vector3.Distance(this.transform.position, nextPosition) < 0.5f && !nextPositionReached && !nearPlayer)
        {
            StartCoroutine(changeNextPosition());
            nextPositionReached = true;
        }


        if (Vector3.Distance(player.transform.position, this.transform.position) <= detectionRadius)
        {
            nextPositionReached = false;
            nextPosition = player.transform.position;
            nearPlayer = true;
        }
        else if (Vector3.Distance(player.transform.position, this.transform.position) > detectionRadius && previousNearPlayer == true) {

            StartCoroutine(changeNextPosition());
            nearPlayer = false;
        }


        previousNearPlayer = nearPlayer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (game.state == "Fighting" || game.isGamePaused)
            return;

         if (!nextPositionReached)
        {

            Vector3 dir = nextPosition - this.transform.position;

            this.transform.LookAt (nextPosition);
            this.transform.rotation =  Quaternion.Euler (0, this.transform.rotation.eulerAngles.y ,0);


            if (player.GetComponent<MainCharacter>().isDefending && nearPlayer)
                this.GetComponent<Rigidbody>().MovePosition(this.transform.position - dir.normalized * 10f * Time.deltaTime);
            else if (!nearPlayer)
                this.GetComponent<Rigidbody>().MovePosition(this.transform.position + dir.normalized * 2f * Time.deltaTime);
            else
                this.GetComponent<Rigidbody>().MovePosition(this.transform.position + dir.normalized * 3f * Time.deltaTime);
        }
        
    }


    public IEnumerator changeNextPosition()
    {
        yield return new WaitForSeconds(waitTime);

        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), this.transform.position.y, Random.Range(startPosition.z - moveRadius, startPosition.z + moveRadius));
        nextPositionReached = false;
    }


	
	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Player") {

            game.spawnedEnemies = this.spawnedEnemies;
            game.spawnedEnemiesHeath = this.health;
            game.spawnedEnemiesAether = this.aether;
            game.spawnedEnemiesAttacks = this.attacks;
            game.spawnedEnemiesDefenses = this.defenses;
            game.spawnedEnemiesSpeeds = this.speeds;
            game.spawnedEnemiesIntelligence = this.intelligence;

            SceneManager.LoadScene ("Combat_"+ this.combatScene);

            game.state = "Fighting";
            
			Destroy(this.gameObject);
		}
	}
}
