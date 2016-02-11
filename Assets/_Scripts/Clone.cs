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

	// Use this for initialization
	void Start () {
        clone = this;
        playback = new CloneData[180];
        int length = Main.MCU.rewindStart - Main.MCU.currentFrame;
        for (int i = 0; i < 180; i++)
        {
            if (i < length)
            {
                playback[i].position = PlayerControl.player.Moments[Main.MCU.currentFrame + i].position;
            }
            else
            {
                playback[i].position = PlayerControl.player.Moments[Main.MCU.rewindStart - 1].position;
            }
        }
        absOffset = Main.MCU.currentFrame;
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.position = playback[Main.MCU.currentFrame - absOffset].position;
        if (Main.MCU.currentFrame - absOffset >= 179)
        {
            Destroy(this.gameObject);
            Main.MCU.inUse = false;
        }
	}
}
