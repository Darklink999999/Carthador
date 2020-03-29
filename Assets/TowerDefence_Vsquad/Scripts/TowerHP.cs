using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHP : MonoBehaviour {

    public int CastleHp = 20;

    

    public void Dmg_2(int DMG_2count)
    {
        CastleHp -= DMG_2count;
    }

    private void Update()
    {
        if (CastleHp <= 0)
        {
            gameObject.tag = "Castle_Destroyed"; // send it to TowerTrigger to stop the shooting
            Destroy(gameObject);

        }
    }

   
}
