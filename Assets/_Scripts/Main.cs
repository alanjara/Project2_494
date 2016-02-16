using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour {

    public static Main MCU;

    //this is the current "time step" for storing or getting positions
    public int currentFrame;
	public string nextLevelName;
    public GameObject playerPrefab;
    public GameObject clonePrefab;
    public bool inUse;
    bool resetting = false;

    public float levelLength = 30;
    public int frameRate = 60;
    public bool rewind = false;
    public int rewindStart;
    public int rewindMax;

    public int groundMask;
    public int cloneMask;

    void Awake() {
        MCU = this;
        Application.targetFrameRate = frameRate;
    }

    // Use this for initialization
    void Start() {

        groundMask = LayerMask.GetMask("Ground");
        cloneMask = LayerMask.GetMask("Clone");
		rewindStart = 1;
        inUse = false;

    }

    // Update is called once per frame
    void Update() {
        if (!resetting) {
            //pressing "V" acts to reset the current playback and go to character select
            if (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.JoystickButton2)) {
                rewind = true;
				currentFrame--; //force one rewind
                rewindStart = currentFrame;

                if (inUse) {
                    Destroy(Clone.clone.gameObject);
                    inUse = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.V) || Input.GetKeyUp(KeyCode.JoystickButton2)) {
                rewind = false;
                GameObject cgo = Instantiate(clonePrefab, PlayerControl.player.transform.position, Quaternion.identity) as GameObject;
                inUse = true;
            }
            checkWin();

            

        } else
            resetting = false;
    }

    void FixedUpdate() {
        if (!rewind) {
            currentFrame++;
        } else if ((rewind && currentFrame <= 0) || (rewind && (rewindStart - currentFrame >= rewindMax))) {

        } else {
            currentFrame--;
        }

    }

    //Call this to reset the player, anything that needs to be reset should have its reset function be called from here
    public void reset() {
        resetting = true;
        currentFrame = 0;
		PlayerControl.player.reset ();
		foreach (GameObject playerLike in GameObject.FindGameObjectsWithTag("Player")) {
			if (!playerLike.name.Contains ("Player")) { //it's a clone
				Destroy(playerLike.gameObject);
				inUse = false;
			}
		}
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
			enemy.GetComponent<Thwomp>().reset();
        }
		if (inUse || Clone.clone != null) {
			Destroy (Clone.clone.gameObject);
			Clone.clone.playback = new CloneData[rewindMax];
			inUse = false;
		}

    }
    //If this function causes an error then add a goal from prefab to the scene
    void checkWin() {
        Vector3 Player_pos= PlayerControl.player.transform.position;
        Vector3 winBoxPos=Goal.winBox.transform.position;
        float winBoxHeight = Goal.winBox.transform.localScale.y/2;
        float winBoxWidth = Goal.winBox.transform.localScale.x/2;
        if (winBoxPos.x - winBoxWidth < Player_pos.x && winBoxPos.x + winBoxWidth > Player_pos.x) {
            if (winBoxPos.y - winBoxHeight < Player_pos.y && winBoxPos.y + winBoxHeight > Player_pos.y) {
                win();
            }
        }

    }
    void win() {
        //What should happen when the player wins
        //reset();
		Application.LoadLevel(nextLevelName);
    }
}
