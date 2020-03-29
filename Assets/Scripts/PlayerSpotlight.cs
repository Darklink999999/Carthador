using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerSpotlight : MonoBehaviour
{
    private Game game;
    private Light globalLight;
    public float lightMultiplier = 3;
    private Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = this.GetComponent<Light>();
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
        {
           light.intensity = -globalLight.intensity + 0.5f;
           light.intensity *= lightMultiplier * 2;
        }
    }
}
