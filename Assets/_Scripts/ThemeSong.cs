using UnityEngine;
using System.Collections;

public class ThemeSong : MonoBehaviour {
    public AudioSource speaker;
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
        speaker = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((Application.loadedLevelName != "Menu" && Application.loadedLevelName != "ThanksForPlaying"))
        {
			if(Main.MCU.rewind){
          	  speaker.Pause();
			}
			else
				speaker.UnPause();
        }
        
	
	}
}
