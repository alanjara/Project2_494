using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	public GameObject				doorPrefab;
	public float					door_x = 0f;
	public float 					door_y = 0f;

	GameObject				go;
	// Use this for initialization
	void Start () {
		go = Instantiate<GameObject> (doorPrefab);
		Vector3 temp = transform.position;
		temp.x = door_x;
		temp.y = door_y;
		temp.z = 0f;
		go.transform.position = temp;

	}
	
	// Update is called once per frame
	void Update () {
	}


	void OnTriggerEnter(Collider other){
		if (other.tag == "Player") {
			Destroy(go);
		}
	}

}
