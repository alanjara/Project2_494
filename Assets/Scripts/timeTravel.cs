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
    public int index = 0;
  public  Vector3 last_velocity=Vector3.zero;
    public bool on_rails = false;
    Rigidbody body;
   public bool grounded = false;
   public bool rewind = false;
   public bool pause = false;
   public bool main = false;
    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody>();
        movementController.moveControl = movementController.moveControl;
    }
    void Awake() {
        //   movementController.moveControl.addTraveller(gameObject);
    }
    void FixedUpdate() {
        if (index == 0) {
            movementController.moveControl.time_travellers.Add(gameObject);
//            movementController.moveControl.addTraveller(gameObject);
        }
        stopSpasm();
        Vector3 rayOffset = new Vector3(0.22f, 0, 0);
        grounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y * .5f + 0.01f)
            || Physics.Raycast(transform.position + rayOffset, Vector3.down, transform.localScale.y * .5f + 0.01f)
            || Physics.Raycast(transform.position - rayOffset, Vector3.down, transform.localScale.y * .5f + 0.01f);
        if (!on_rails && !rewind) {
            moments[index].position = transform.position;
            moments[index].rotation = transform.rotation;
            rail_stop = index;
            if(index==0 || moments[index].position != moments[index-1].position) index++;
         //   index++;
            last_velocity = body.velocity;

        } else {
            if (index >= rail_stop)
                derail();
            transform.position = moments[index].position;
            transform.rotation = moments[index].rotation;
            body.velocity = Vector3.zero;
            if (!pause && rewind && index > 0)
                index--; 
            else if(!pause)
            index++;
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
        movementController.moveControl.enableCollision(number);
    }
    public void goBack(int target_index) {
        putOnRail();
        index = target_index;
    }
    public void putOnRail() {
        GetComponent<Rigidbody>().useGravity = false;
        body.velocity = Vector3.zero;
        on_rails = true;
        body.isKinematic = true;
        movementController.moveControl.disableCollision(number);
    }
    void stopSpasm() {
        if (body.velocity.magnitude > 10) {
            Vector3 new_vel = body.velocity;
            while(new_vel.magnitude> 30)
            new_vel = new_vel * 0.9f;
            body.velocity = new_vel;
        }
    }

    /*
    void OnCollisionStay(Collision collisionInfo) {

        if (collisionInfo.gameObject.tag == "Ground")
            grounded = true;
    }
    */
}
