using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerSpotlight : MonoBehaviour
{
    private Game game;
    private Light globalLight;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name == "World" && (game == null || globalLight == null))
        {


            game = Camera.main.GetComponent<Game>();
            globalLight = GameObject.Find("GlobalLight").GetComponent<Light>();
        }


        if (SceneManager.GetActiveScene().name == "World")
            this.GetComponent<Light>().intensity = (-(globalLight.intensity - 0.999f) * 3) - 2.2f;
    }
}
