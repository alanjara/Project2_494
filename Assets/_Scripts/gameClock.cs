using UnityEngine;
using System.Collections;
using System;

public class gameClock : MonoBehaviour {

	void Start() {
		GetComponent<TextMesh>().transform.position = new Vector3 (0, 5.05f, 0);
	}
    // Update is called once per frame
    void Update() {
        if (Main.MCU.levelLength - Main.MCU.currentFrame / Main.MCU.frameRate <= 0)
            reset();
        GetComponent<TextMesh>().text = "Time: " + Convert.ToString(Main.MCU.levelLength - Main.MCU.currentFrame / Main.MCU.frameRate);

    }

    void reset() {
        Main.MCU.reset();
    }

}
