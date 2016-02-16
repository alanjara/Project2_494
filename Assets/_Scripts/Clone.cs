using UnityEngine;
using System.Collections;

public struct CloneData
{
    public Vector3 position;
    public State type;
}

public class Clone : MonoBehaviour {

    static public Clone clone;
    public CloneData[] playback;
    public int absOffset;
    public State type;

	// Use this for initialization
	void Start () {
        clone = this;
		playback = new CloneData[Main.MCU.rewindMax];
        int length = Main.MCU.rewindStart - Main.MCU.currentFrame;
		for (int i = 0; i < Main.MCU.rewindMax; i++)
        {
            if (i < length)
            {
                playback[i].position = PlayerControl.player.Moments[Main.MCU.currentFrame + i].position;
                playback[i].type = PlayerControl.player.Moments[Main.MCU.currentFrame + i].type;
            }
            else
            {   
                playback[i].position = PlayerControl.player.Moments[Main.MCU.rewindStart - 1].position;
                playback[i].type = PlayerControl.player.Moments[Main.MCU.rewindStart - 1].type;

            }
        }
        absOffset = Main.MCU.currentFrame;
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Rigidbody> ().WakeUp();
		this.tag = "Player";
        this.gameObject.transform.position = playback[Main.MCU.currentFrame - absOffset].position;
		type = playback [Main.MCU.currentFrame - absOffset].type;
		if (Main.MCU.currentFrame - absOffset >= Main.MCU.rewindMax - 1)
        {
            Destroy(this.gameObject);
            Main.MCU.inUse = false;
        }

		if (type == State.Throw) {
			GetComponent<MeshFilter> ().mesh = PlayerControl.player.basicSphere;
		} else {
			GetComponent<MeshFilter> ().mesh = PlayerControl.player.basicCube;
		}
	}
}
