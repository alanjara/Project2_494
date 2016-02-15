using UnityEngine;
using System.Collections;

public class Button_ray : MonoBehaviour {
	public GameObject				doorPrefab;
	public float					door_x = 0f;
	public float 					door_y = 0f;
	
	GameObject						go;

	public float					door_lift_y;
	public float 					castDis = 0.2f;
	public bool						leftCast;
	public bool						rightCast;
	public bool						upCast;
	Vector3 		 				original_location;

	// Use this for initialization
	void Start () {
		door_lift_y = door_y + 2f;

		go = Instantiate<GameObject> (doorPrefab);
		original_location = transform.position;
		original_location.x = door_x;
		original_location.y = door_y;
		original_location.z = 0f;
		go.transform.position = original_location;
	}

	/*
	// Update is called once per frame
	void Update () {
		Vector3 check = this.transform.position;
		upCast = Physics.Raycast (check, Vector3.up, castDis);
		leftCast = Physics.Raycast (check, Vector3.left, castDis);
		rightCast = Physics.Raycast (check, Vector3.right, castDis);
		if (upCast || leftCast || rightCast) {
			//Vector3 temp = go.transform.position;
			//temp.y = door_lift_y;
			//go.transform.position = temp;
			go.active = false;
		} else {
			go.active = true;
			go.transform.position = original_location;

		}

	}
	*/

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			go.active = false;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			go.active = true;
		}
	}

}
