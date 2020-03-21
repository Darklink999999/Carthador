using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private float speed = 5f;

    private Vector3 attackDir;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroySelf());
        attackDir = attackDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameObject.FindGameObjectWithTag("Player").transform.position;

        float aimH = Input.GetAxis("Horizontal2");
        float aimV = Input.GetAxis("Vertical2");

        if (aimH != 0 || aimV != 0)
        {
            attackDir = new Vector3(aimH, aimV, 0);
            speed = 3f;
        }
        else
            speed = 20f;
    }

    // Update is called once per frame
    void Update()
    { 

    }

    void FixedUpdate()
    {
        this.GetComponent<Rigidbody2D>().MovePosition(this.transform.position + attackDir.normalized * speed * Time.deltaTime);
    }


    public IEnumerator destroySelf ()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);

    }
}
