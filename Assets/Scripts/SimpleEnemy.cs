using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{

    public float moveRadius = 5;
    public float waitTime = 3;
    public float detectionRadius = 10;

    public int damage = 10;

    public int maxHealth = 30;
    [HideInInspector] public int currentHealth;

    private Vector3 startPosition;
    private Vector3 nextPosition;
    private bool nextPositionReached = false;

    private Game game;

    private GameObject player;
    private bool nearPlayer = false;
    private bool previousNearPlayer;
    private bool attacked = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        game = Camera.main.GetComponent<Game>();
        player = game.player;


        startPosition = this.transform.position;
        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), this.transform.position.y, Random.Range(startPosition.z - moveRadius, startPosition.z + moveRadius));
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(this.transform.position, nextPosition) < 0.5f && !nextPositionReached && !nearPlayer && !attacked)
        {
            StartCoroutine(changeNextPosition());
            nextPositionReached = true;
        }


        if (Vector3.Distance(player.transform.position, this.transform.position) < detectionRadius * 2 && game.state == "None")
            game.state = "ReadyToFight";

        if (Vector3.Distance(player.transform.position, this.transform.position) < detectionRadius || attacked)
        {

            if (!nearPlayer || game.state != "Fighting")
            {
                game.state = "Fighting";

            }

            nextPositionReached = false;
            nextPosition = player.transform.position;
            nearPlayer = true;
        }
        else if (!attacked)
        {
            nearPlayer = false;
        }


        if (previousNearPlayer == true && nearPlayer == false)
        {
            game.state = "None";
            this.attacked = false;
        }




        previousNearPlayer = nearPlayer;



        if (this.currentHealth <= 0) {
            game.state = "None";
            Destroy(this.gameObject);
        }
        
    }

    public void FixedUpdate() {
        
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



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PlayerAttack")
        {
            Destroy(collision.collider.gameObject);
            this.currentHealth -= 10;
            this.attacked = true;

            StartCoroutine (disableAttacked());
        }
        else if (collision.collider.tag == "Player")
        {
            player.GetComponent<MainCharacter>().currentHealth -= damage;
            player.GetComponent<MainCharacter>().attacked = true;
            player.GetComponent<Rigidbody>().AddForce(-collision.contacts [0].normal * 5f, ForceMode.Impulse);
        }
    }

    public IEnumerator changeNextPosition()
    {
        yield return new WaitForSeconds(waitTime);



        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), this.transform.position.y, Random.Range(startPosition.z - moveRadius, startPosition.z + moveRadius));
        nextPositionReached = false;
    }

     public IEnumerator disableAttacked()
    {
        yield return new WaitForSeconds(2);



        attacked = false;
    }
}
