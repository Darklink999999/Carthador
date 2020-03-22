using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginCombat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Time.timeScale = 0;
			
			SceneManager.LoadScene(2);
			Destroy(this.gameObject);
		}
	}
}
