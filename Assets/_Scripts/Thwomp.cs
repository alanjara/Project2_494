using UnityEngine;
using System.Collections;

public class Thwomp : MonoBehaviour {
    public float fallspeed = 10f;
    public float raisespeed = 2f;
    public int stayLength = 20;
    public float aggroWidth = 2f;
    public float rayOffset = .9f;
    public bool grounded;
    int groundedCount;
    Vector3[] Moments;
    bool[] AIStates;
    Vector3 startingPos;
    Rigidbody rigid;
    RigidbodyConstraints standardConstraints;
    bool fall;

	// Use this for initialization
	void Start () {
        fall = false;
        
        groundedCount = 0;
        startingPos = transform.position;
        Moments = new Vector3[2000];
        AIStates = new bool[2000];
        Moments[0] = transform.position;
        rigid = GetComponent<Rigidbody>();
        standardConstraints = rigid.constraints;
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            grounded = true;
    }

    public void reset()
    {
        transform.position = startingPos;
        rigid.constraints = standardConstraints;
        fall = false;
    }

    bool aggro()
    {
        bool triggered = false;
        GameObject[] playerLikeObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in playerLikeObjects)
        {
            Vector3 pos = player.transform.position;
            Vector3 thwompPos = transform.position;
            if (Mathf.Abs(thwompPos.x - pos.x) < aggroWidth && thwompPos.y > pos.y + 1f){
                triggered = true;
            }
        }
        return triggered;
    }

    // Update is called once per frame
    void Update () {
        if (Camera.main.GetComponent<Main>().rewind)
        {
            transform.position = Moments[Camera.main.GetComponent<Main>().currentFrame];
            fall = AIStates[Main.MCU.currentFrame];
            if(groundedCount > 0)
            {
                groundedCount -= 1;
            }
        }
        else
        {
            
            if (grounded)
            {
                groundedCount += 1;
            }
            if (groundedCount > stayLength)
            {
               groundedCount = 0;
               fall = false;
               grounded = false;
            }
            if (aggro())
            {
                fall = true;
            }

            if (fall)
            {
                rigid.constraints = standardConstraints;
                rigid.velocity = Vector3.down * fallspeed;
            }
            else if (transform.position.y < startingPos.y)
            {
                rigid.constraints = standardConstraints;
                rigid.velocity = Vector3.up * raisespeed;
            }
            else
            {
                rigid.constraints = RigidbodyConstraints.FreezeAll;
                rigid.velocity = Vector3.zero;
            }


            Moments[Camera.main.GetComponent<Main>().currentFrame] = transform.position;
            AIStates[Main.MCU.currentFrame] = fall;
        }
	
	}
}
