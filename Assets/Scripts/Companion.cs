using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Companion : MonoBehaviour
{

    private GameObject player;
    private Game game;

    private Rigidbody rigid;
    public float moveSpeed = 5f;

    private Animator anim;
    private float previousAnimSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        game = Camera.main.GetComponent <Game> ();
        rigid = this.GetComponent <Rigidbody> ();
        anim = this.GetComponent <Animator> ();

        DontDestroyOnLoad (this.gameObject);

        SceneManager.sceneLoaded += OnLevelChanged;

    }

    // Update is called once per frame
    void Update()
    {
        if (game.isGamePaused)
            return;        
    }

    public void FixedUpdate() {
        
        if(game.isGamePaused)
            return;

        followPlayer ();
    }


    private void followPlayer () {


        /// <summary>
        /// /////////////////////////////////////////// MOVEMENT ///////////////////////////////////////////////////////
        /// </summary>

        Vector3 dir = player.transform.position - this.transform.position;

        this.transform.LookAt (player.transform.position);
        this.transform.rotation =  Quaternion.Euler (0, this.transform.rotation.eulerAngles.y ,0);

        RaycastHit hit;
        float groundY = 0;    

        if (Physics.Raycast(transform.position + new Vector3 (0, 1, 0), transform.TransformDirection(-Vector3.up), out hit, 4f)){

           groundY= hit.point.y;
        }
        else {

            this.transform.position = player.transform.position + new Vector3 (1, 0 ,1);
        }


        if (Vector3.Distance (this.transform.position, player.transform.position) >= 7f) 
            rigid.MovePosition (new Vector3 (this.transform.position.x, groundY, this.transform.position.z) + dir.normalized * moveSpeed * Time.fixedDeltaTime);
        else if (Vector3.Distance (this.transform.position, player.transform.position) > 3f && Vector3.Distance (this.transform.position, player.transform.position) < 7f)
            rigid.MovePosition (new Vector3 (this.transform.position.x, groundY, this.transform.position.z) + dir.normalized * moveSpeed / 2 * Time.fixedDeltaTime);
        else
            rigid.MovePosition (new Vector3 (this.transform.position.x, groundY, this.transform.position.z));


        /// ////////////////////////// ANIMATION //////////////////////////////////////////


        float animSpeed = Vector3.Distance (this.transform.position, player.transform.position) - 3;

        if (animSpeed + 3f > 3f && animSpeed + 3f < 7f)
            animSpeed /= 4f;

        animSpeed = Mathf.Lerp (previousAnimSpeed, animSpeed, 0.5f);

        anim.SetFloat("Forward", animSpeed);
        anim.SetFloat("Turn",rigid.angularVelocity.normalized.y);

        previousAnimSpeed = animSpeed;
    }

    public void OnLevelChanged (Scene scene, LoadSceneMode mode) {

        ;

    }

}
