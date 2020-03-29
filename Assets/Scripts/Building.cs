using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Building : MonoBehaviour
{

    public string loadScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnTriggerEnter2D (Collider2D collision){

        if (collision.tag.Equals ("Player")){

            SceneManager.LoadScene(loadScene);
        }
    }
}
