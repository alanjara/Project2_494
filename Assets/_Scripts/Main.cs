using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public static Main MCU;

    //this is the current "time step" for storing or getting positions
    public int 			currentFrame;
    public GameObject 	playerPrefab;
    public GameObject 	clonePrefab;
    public bool 		inUse;

	public int			levelLength;

    public bool 		rewind = false;
    public int 			rewindStart;
    public int			rewindMax;

    public int 			groundMask;
    public int 			cloneMask;

    void Awake()
    {
        MCU = this;
        Application.targetFrameRate = 60;
    }

	// Use this for initialization
	void Start () {

        groundMask = LayerMask.GetMask("Ground");
        cloneMask = LayerMask.GetMask("Clone");

        inUse = false;

    }
	
	// Update is called once per frame
	void Update () {
        //pressing "V" acts to reset the current playback and go to character select
        if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.JoystickButton2))
        {
            rewind = true;
            rewindStart = currentFrame;

            if (inUse)
            {
                Destroy(Clone.clone.gameObject);
                inUse = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.V) || Input.GetKeyUp(KeyCode.JoystickButton2))
        {
            rewind = false;
            GameObject cgo = Instantiate(clonePrefab, PlayerControl.player.transform.position, Quaternion.identity) as GameObject;
            inUse = true;
        }
    }

    void FixedUpdate()
    {
        if (!rewind)
        {
            currentFrame++;
        }
        else if ((rewind && currentFrame <= 0) || (rewind && (rewindStart - currentFrame >= rewindMax)))
        {

        }
        else
        {
            currentFrame--;
        }
        
    }
}
