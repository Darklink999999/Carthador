using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Save () {

        SaveSystem.Save (GameObject.FindGameObjectWithTag ("Player").GetComponent <MainCharacter>(), Camera.main.GetComponent <Game>());


    }
}
