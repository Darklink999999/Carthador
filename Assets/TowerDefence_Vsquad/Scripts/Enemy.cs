using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    public Transform shootElement;
    public GameObject bullet;
    public GameObject Enemybug;
    public int Creature_Damage = 10;    
    public float Speed;
    // 
    public Transform[] waypoints;
    int curWaypointIndex = 0;
    public float previous_Speed;
    public Animator anim;
    public EnemyHp Enemy_Hp;
    public Transform target;
    public GameObject EnemyTarget;
    

    void Start()
    {            
        anim = GetComponent<Animator>();
        Enemy_Hp = Enemybug.GetComponent<EnemyHp>();
        previous_Speed = Speed;        
    }

    // Attack

    void OnTriggerEnter(Collider other)

    {
        if (other.tag == "Castle")
        {
            
            Speed = 0;
            EnemyTarget = other.gameObject;
            target = other.gameObject.transform;
            Vector3 targetPosition = new Vector3(EnemyTarget.transform.position.x, transform.position.y, EnemyTarget.transform.position.z);            
            transform.LookAt(targetPosition);
            anim.SetBool("RUN", false);
            anim.SetBool("Attack", true);
            
        }

    }

    // Attack
    void Shooting ()
    {
        //if (EnemyTarget)
       // {           
            GameObject с = GameObject.Instantiate(bullet, shootElement.position, Quaternion.identity) as GameObject;
            с.GetComponent<EnemyBullet>().target = target;
            с.GetComponent<EnemyBullet>().twr = this;
       // }  

    }

    

    void GetDamage ()

    {        
            EnemyTarget.GetComponent<TowerHP>().Dmg_2(Creature_Damage);       
    }

    


    void Update () 
	{

        
        //Debug.Log("Animator  " + anim);


        // MOVING

        if (curWaypointIndex < waypoints.Length){
	transform.position = Vector3.MoveTowards(transform.position,waypoints[curWaypointIndex].position,Time.deltaTime*Speed);
            
            if (!EnemyTarget)
            {
                transform.LookAt(waypoints[curWaypointIndex].position);
            }
	
	if(Vector3.Distance(transform.position,waypoints[curWaypointIndex].position) < 0.5f)
	{
		curWaypointIndex++;
	}    
	}          

        else
        {
            anim.SetBool("Victory", true);  // Victory
        }

        // DEATH

        if (Enemy_Hp.EnemyHP <= 0)
        {
            Speed = 0;
            Destroy(gameObject, 5f);
            anim.SetBool("Death", true);            
        }

        // Attack to Run
                

        if (EnemyTarget)        {

          
            if (EnemyTarget.CompareTag("Castle_Destroyed")) // get it from BuildingHp
            {
                anim.SetBool("Attack", false);
                anim.SetBool("RUN", true);
                Speed = previous_Speed;               
                EnemyTarget = null;                
            }
        }


    }
       
   
}

