using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour {

    public int EnemyHP = 30;

    public void Dmg(int DMGcount)
    {
        EnemyHP -= DMGcount;
    }

    private void Update()
    {        

        if (EnemyHP <= 0)
        {
            gameObject.tag = "Dead"; // send it to TowerTrigger to stop the shooting
           
        }
    }
    
}
