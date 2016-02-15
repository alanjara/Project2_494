using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        print("Hit a trigger with " + gameObject.name);
        if (other.gameObject.tag == "Player" && other.gameObject.name.Contains("Clone"))
        {
            Main.MCU.inUse = false;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Player" && other.gameObject.name.Contains("Player"))
        {
            Main.MCU.reset();
        }
    }
}
