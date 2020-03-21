using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Seller : MonoBehaviour
{

    public string[] dialogues;
    public string[] questDialogues;


	
    private Game game;

    private GameObject player;

	private bool firstMeeting = false;
	
    private bool nearPlayer;

    private GameObject innMenu;


    // Start is called before the first frame update
    void Start()
    {

        game = Camera.main.GetComponent<Game>();
        player = game.player;

        innMenu = game.innMenu;
        innMenu.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {

        if (game.isGamePaused)
            return;


        
        if (game.state != "NearNPC" && Vector3.Distance (player.transform.position, this.transform.position) < 1.3f){

            this.nearPlayer = true;

            game.messagesParent.SetActive(true);
            game.messages.text = "Talk";
            game.state = "NearNPC";
        }

        if (Input.GetButtonDown ("Fire1") && nearPlayer && game.state == "NearNPC") 
        {

            game.state = "Talking";
            game.messages.text = "";
            game.isGamePaused = true;
            Time.timeScale = 0;

            game.messages.GetComponent<Messages>().followingMenu = innMenu;

            if (game.completedQuests.Contains ("First InnKeeper Meeting")) {

                game.messages.GetComponent<Messages>().message = dialogues[game.gamePhase];
            }
            else if (this.name == "InnKeeper") {
                
                game.messages.GetComponent<Messages>().message = questDialogues[0];
                game.inventoryScript.items [game.inventoryScript.items.IndexOf ("Empty")] = "Aether Potion";
                game.inventoryScript.items [game.inventoryScript.items.IndexOf ("Empty")] = "Aether Potion";

                game.completedQuests.Add ("First InnKeeper Meeting");
            }
        } 

        
    }

    public void OnTriggerExit2D (Collider2D collider) {

        if (collider.tag.Equals ("Player")){

            this.nearPlayer = false;

            game.messages.GetComponent<Messages>().followingMenu = null;
            game.messages.GetComponent<Messages>().message = "";
            game.messages.text = "";
            game.state = "None";
            game.messagesParent.SetActive(false);
        }
    }
}
