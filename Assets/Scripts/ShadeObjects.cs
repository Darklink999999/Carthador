using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadeObjects : MonoBehaviour
{


    private Game game;

    private int timeOfDay;
    private int previousTimeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent <Game> ();
        this.timeOfDay = game.hourOfDay;
        
    }

    // Update is called once per frame
    void Update()
    {

        this.timeOfDay = game.hourOfDay;

        if (previousTimeOfDay != this.timeOfDay){
        
            if (this.timeOfDay >= 0 && this.timeOfDay <= 5) 
                    foreach (Material m in this.GetComponent <MeshRenderer> ().materials)
                        m.color = new Color (0.25f, 0.25f, 0.25f);
            else if (this.timeOfDay >= 6 && this.timeOfDay <= 8)
                foreach (Material m in this.GetComponent <MeshRenderer> ().materials)
                    m.color = new Color (0.25f * (this.timeOfDay % 5),  0.25f * (this.timeOfDay % 5), 0.25f * (this.timeOfDay % 5) );
            else if (this.timeOfDay >= 9 && this.timeOfDay <= 17)
                foreach (Material m in this.GetComponent <MeshRenderer> ().materials)
                    m.color = Color.white;
            else if (this.timeOfDay >= 18 && this.timeOfDay <= 23)
                foreach (Material m in this.GetComponent <MeshRenderer> ().materials)
                m.color = new Color (1f - (0.15f * (this.timeOfDay % 18f)),  1f - (0.15f * (this.timeOfDay % 18f)), 1f - (0.15f * (this.timeOfDay % 18f)) );
        }


        previousTimeOfDay = this.timeOfDay;
    }
}
