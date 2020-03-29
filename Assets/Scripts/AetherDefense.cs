using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AetherDefense : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutine(destroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Rigidbody2D r = collision.collider.GetComponent<Rigidbody2D>();
            //r.AddForce(new Vector2(-r.velocity.x, -r.velocity.y) * 50f, ForceMode2D.Impulse);
            r.velocity = -r.velocity * 50f;

        }
    }


    public IEnumerator destroySelf ()
    {
        yield return  new WaitForSeconds(1);

        GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>().isDefending = false;

        Destroy(this.gameObject);
    }

}
