using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [HideInInspector] public List <string> items;
    public int maxItems;

    // Start is called before the first frame update
    void Start()
    {

        items = new List <string> ();

        maxItems = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
