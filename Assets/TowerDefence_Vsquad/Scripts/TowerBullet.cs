using UnityEngine;
using System.Collections;

public class TowerBullet : MonoBehaviour {

    public float Speed;
    public Transform target;
    public GameObject impactParticle; // bullet impact
    
    public Vector3 impactNormal; 
    Vector3 lastBulletPosition; 
    public Tower twr;    
    float i = 0.05f; // delay time of bullet destruction

    
    void Update() {

        // Bullet move

        if (target) 
        {        
            
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * Speed); 
            lastBulletPosition = target.transform.position; 

        }

        // Move bullet ( enemy was disapeared )

        else          {
                        
                transform.position = Vector3.MoveTowards(transform.position, lastBulletPosition, Time.deltaTime * Speed); 

                if (transform.position == lastBulletPosition) 
                {
                    Destroy(gameObject,i);

                // Bullet hit ( enemy was disapeared )

                if (impactParticle != null) 
                {
                    impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;  // Tower`s hit
                    Destroy(impactParticle, 3);
                    return;
                }

                 }           
        }     
    }

    // Bullet hit

    void OnTriggerEnter (Collider other) // tower`s hit if bullet reached the enemy
    {
        if(other.gameObject.transform == target)
        {
            target.GetComponent<EnemyHp>().Dmg(twr.dmg); 
            Destroy(gameObject, i); // destroy bullet
            impactParticle = Instantiate(impactParticle, target.transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            impactParticle.transform.parent = target.transform;
            Destroy(impactParticle, 3);
            return;  
        }
    }
 
}



