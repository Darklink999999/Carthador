using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpotlight : MonoBehaviour
{
	private Game game;
	
    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<Light>().intensity = (game.globalLight.GetComponent<Light>().intensity + 1) * -1;
    }
}
