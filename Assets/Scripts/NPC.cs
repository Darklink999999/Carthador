using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NPC : MonoBehaviour
{

    public float moveRadius = 5;
    public float waitTime = 3;
    public string[] dialogues;
    public string[] questDialogues;
    

    private Vector3 startPosition;
    private Vector3 nextPosition;
    private bool nextPositionReached = false;

    private Game game;

    private GameObject player;
    private bool nearPlayer = false;
    private bool previousNearPlayer;
    private string previousGameState;

    // Start is called before the first frame update
    void Start()
    {

        game = Camera.main.GetComponent<Game>();
        player = game.player;

        startPosition = this.transform.position;
        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), Random.Range(startPosition.y - moveRadius, startPosition.y + moveRadius), 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (game.isGamePaused)
            return;


        if (Vector3.Distance(this.transform.position, nextPosition) < 0.5f && !nextPositionReached)
        {
            StartCoroutine(changeNextPosition());
            nextPositionReached = true;
            this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }


        if (Vector3.Distance(player.transform.position, this.transform.position) < 1f)
        {
            if (!nearPlayer || (previousGameState == "Talking" && game.state != "Talking"))
            {
                game.messagesParent.SetActive(true);
                game.messages.text = "Talk";
                game.state = "NearNPC";
            }

            if (Input.GetButtonDown("Fire1") && game.state == "NearNPC")
            {
                game.state = "Talking";
                game.messages.text = "";
                game.messages.GetComponent<Messages>().message = dialogues[game.gamePhase];
            }

            nearPlayer = true;
        }
        else
            nearPlayer = false;


        if (previousNearPlayer == true && nearPlayer == false)
        {
            game.messages.GetComponent<Messages>().message = "";
            game.messages.text = "";
            game.state = "None";
            game.messagesParent.SetActive(false);

        }




       previousNearPlayer = nearPlayer;
       previousGameState = game.state;




    }


    public void FixedUpdate() {

        if (!nextPositionReached) {

            Vector3 dir = nextPosition - this.transform.position;
            this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + dir.normalized * 2f * Time.deltaTime);
        }
        
    }

    public IEnumerator changeNextPosition ()
    {
        yield return new WaitForSeconds(waitTime);
        
            
            
        nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), Random.Range(startPosition.y - moveRadius, startPosition.y + moveRadius), 0);
        nextPositionReached = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != "Player")
            nextPosition = new Vector3(Random.Range(startPosition.x - moveRadius, startPosition.x + moveRadius), Random.Range(startPosition.y - moveRadius, startPosition.y + moveRadius), 0);
    }
}
