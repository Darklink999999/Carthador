using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 20f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        private bool levelJustLoaded = false;

        // Use this for initialization
        private void Start()
        {
            target = GameObject.FindGameObjectWithTag ("Player").transform;

            m_LastTargetPosition = target.position;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;

            SceneManager.sceneLoaded += OnLevelChanged;
        }


        // Update is called once per frame
        private void Update()
        {
            if (levelJustLoaded) {
                this.transform.position = new Vector3 (target.transform.position.x, target.transform.position.y, this.transform.position.z);
            }

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
        }




        public void OnLevelChanged (Scene scene, LoadSceneMode mode) {

            this.levelJustLoaded = true;

            this.StartCoroutine (disableLevelJustLoaded ());

        }

        private IEnumerator disableLevelJustLoaded () {

            yield return new WaitForSeconds (0.5f);

            this.levelJustLoaded = false;
        }
    }
}
