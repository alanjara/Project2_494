using UnityEngine;
using System.Collections;

public struct moment {
  public  Vector3 position;
  public  Quaternion rotation;
}
public class timeTravel : MonoBehaviour {
    public int number;
    moment[] moments = new moment[10000];
    int rail_stop = 0;
    int index = 0;
  public  Vector3 last_velocity=Vector3.zero;
    public bool on_rails = false;
    Rigidbody body;
   public bool grounded = false;
    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody>();
    }
    void Awake() {
        //   movementController.moveControl.addTraveller(gameObject);
    }
    void FixedUpdate() {
        if (index == 0) {
            movementController.moveControl.time_travellers.Add(gameObject);

//            movementController.moveControl.addTraveller(gameObject);
        }

        Vector3 rayOffset = new Vector3(0.22f, 0, 0);
        grounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * .5f + 0.1f)
            || Physics.Raycast(transform.position + rayOffset, Vector3.down, transform.localScale.y * .5f + 0.1f)
            || Physics.Raycast(transform.position - rayOffset, Vector3.down, transform.localScale.y * .5f + 0.1f);
        if (!on_rails) {
            moments[index].position = transform.position;
            moments[index].rotation = transform.rotation;
            rail_stop = index;
            index++;
            last_velocity = body.velocity;

        } else {
            transform.position = moments[index].position;
            transform.rotation = moments[index].rotation;
            body.velocity = Vector3.zero;
            index++;
            if (index >= rail_stop)
                derail();
        }
    }
    public void derail() {
        if (!on_rails)
            return;
        on_rails = false;
        GetComponent<Rigidbody>().useGravity = true;
        rail_stop = index;
        body.velocity = last_velocity;
        body.isKinematic = false;
    }
    public void goBack(int target_index) {
        GetComponent<Rigidbody>().useGravity = false;
        body.velocity = Vector3.zero;
        on_rails = true;
        body.isKinematic = true;
        index = target_index;
    }
    /*
    void OnCollisionStay(Collision collisionInfo) {

        if (collisionInfo.gameObject.tag == "Ground")
            grounded = true;
    }
    */
}
