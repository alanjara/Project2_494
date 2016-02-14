using UnityEngine;
using System.Collections;

public class RewindEffect : MonoBehaviour {
    public MeshRenderer rend;
    public AudioSource rewindSoundMaker;

	// Use this for initialization
	void Start () {
	    rend = this.gameObject.GetComponent<MeshRenderer>();
        rewindSoundMaker = this.gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        rend.enabled = Camera.main.GetComponent<Main>().rewind;
        rewindSoundMaker.mute = !Camera.main.GetComponent<Main>().rewind;
	
	}
}
