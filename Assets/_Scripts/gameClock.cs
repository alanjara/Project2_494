using UnityEngine;
using System.Collections;
using System;

public class gameClock : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        if (Main.MCU.levelLength - Main.MCU.currentFrame / Main.MCU.frameRate <= 0)
            reset();
        GetComponent<TextMesh>().text = Convert.ToString(Main.MCU.levelLength - Main.MCU.currentFrame / Main.MCU.frameRate);

    }

    void reset() {
        Main.MCU.reset();
    }

}
