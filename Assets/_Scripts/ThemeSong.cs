using UnityEngine;
using System.Collections;

public class ThemeSong : MonoBehaviour {
    public AudioSource speaker;
	// Use this for initialization
	void Start () {
        speaker = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Main.MCU.rewind)
        {
            speaker.Pause();
        }
        else
            speaker.UnPause();
	
	}
}
