using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{

    public float moveRadius = 5;
    public float waitTime = 3;
    public float detectionRadius = 2;

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
        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), Random.Range(startPosition.y - moveRadius, startPosition.y + moveRadius), 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(this.transform.position, nextPosition) < 0.5f && !nextPositionReached && !nearPlayer && !attacked)
        {
            StartCoroutine(changeNextPosition());
            nextPositionReached = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
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


            if (player.GetComponent<MainCharacter>().isDefending && nearPlayer)
                this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position - dir.normalized * 10f * Time.deltaTime);
            else if (!nearPlayer)
                this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + dir.normalized * 2f * Time.deltaTime);
            else if (this.GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic)
                this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + dir.normalized * 3f * Time.deltaTime);
        }

    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Arrow")
        {
            Destroy(collision.collider.gameObject);
            this.currentHealth -= 10;
            this.attacked = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            StartCoroutine (disableAttacked());
        }
        else if (collision.collider.tag == "Player")
        {
            player.GetComponent<MainCharacter>().currentHealth -= damage;
            player.GetComponent<MainCharacter>().attacked = true;
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2 (-collision.contacts [0].normal.x, -collision.contacts [0].normal.y) * 5f, ForceMode2D.Impulse);
        }
    }

    public IEnumerator changeNextPosition()
    {
        yield return new WaitForSeconds(waitTime);



        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), Random.Range(startPosition.y - moveRadius, startPosition.y + moveRadius), 0);
        nextPositionReached = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

     public IEnumerator disableAttacked()
    {
        yield return new WaitForSeconds(2);



        attacked = false;
    }
}
