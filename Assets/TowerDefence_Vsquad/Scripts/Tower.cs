using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour {


    public bool Catcher = false;
	public Transform shootElement;
    public GameObject Towerbug;
    public Transform LookAtObj;    
    public GameObject bullet;
    public GameObject DestroyParticle;
    public Vector3 impactNormal_2;
    public Transform target;
    public int dmg = 10;
    public float shootDelay;
    bool isShoot;
    public Animator anim_2;
    public TowerHP TowerHp;    
    private float homeY;

    // for Catcher tower 

    void Start()
    {
        anim_2 = GetComponent<Animator>();
        homeY = LookAtObj.transform.localRotation.eulerAngles.y;
        TowerHp = Towerbug.GetComponent<TowerHP>();
    }
           

    
    // for Catcher tower attack animation

    void GetDamage()

    {
        if (target)
        {
            target.GetComponent<EnemyHp>().Dmg(dmg);
        }
    }




    void Update () {

        
        // Tower`s rotate

        if (target)
        {  
            
            Vector3 dir = target.transform.position - LookAtObj.transform.position;
                dir.y = 0; 
                Quaternion rot = Quaternion.LookRotation(dir);                
                LookAtObj.transform.rotation = Quaternion.Slerp( LookAtObj.transform.rotation, rot, 5 * Time.deltaTime);

        }
      
        else
        {
            
            Quaternion home = new Quaternion (0, homeY, 0, 1);
            
            LookAtObj.transform.rotation = Quaternion.Slerp(LookAtObj.transform.rotation, home, Time.deltaTime);                        
        }


        // Shooting
               

            if (!isShoot)
            {
                StartCoroutine(shoot());

            }

        
        if (Catcher == true)
        {
            if (!target || target.CompareTag("Dead"))
            {

                StopCatcherAttack();
            }

        }

        // Destroy

        if (TowerHp.CastleHp <= 0)        {
            
            Destroy(gameObject);
            DestroyParticle = Instantiate(DestroyParticle, Towerbug.transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal_2)) as GameObject;            
            Destroy(DestroyParticle, 3);
        }



    }

	IEnumerator shoot()
	{
		isShoot = true;
		yield return new WaitForSeconds(shootDelay);


        if (target && Catcher == false)
        {
            GameObject b = GameObject.Instantiate(bullet, shootElement.position, Quaternion.identity) as GameObject;
            b.GetComponent<TowerBullet>().target = target;
            b.GetComponent<TowerBullet>().twr = this;
          
        }

        if (target && Catcher == true)
        {
            anim_2.SetBool("Attack", true);
            anim_2.SetBool("T_pose", false);
        }


        isShoot = false;
	}



        void StopCatcherAttack()

        {                
            target = null;
            anim_2.SetBool("Attack", false);
            anim_2.SetBool("T_pose", true);        
        } 
          

}



