using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadeObjects : MonoBehaviour
{


    private Game game;
    private Light globalLight;

    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent <Game> ();
        
    }

    // Update is called once per frame
    void Update()
    {


        if (SceneManager.GetActiveScene().name == "World" && globalLight == null)
        {
            globalLight = game.globalLight.GetComponent<Light>();
        }

        if (SceneManager.GetActiveScene().name == "World" && globalLight != null)
        {
<<<<<<< Updated upstream
            float finalIntensity = globalLight.intensity + 0.5f;
=======
            float finalIntensity = globalLight.intensity + 0.2f;
>>>>>>> Stashed changes
            foreach (Material m in this.GetComponent<MeshRenderer>().materials)
                m.color = new Color(finalIntensity, finalIntensity, finalIntensity);
        }
    }
}
