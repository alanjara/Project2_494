using UnityEngine;
using System.Collections;

//easy to add some more skills that could be context sensitive/enemy based
public enum State { None, Throw }

public struct Moment {
    //possibly expand with sprite for animations
    public Vector3 position;
    public Vector3 velocity;
    public State type;
}

public class PlayerControl : MonoBehaviour {

    static public PlayerControl player;

    //stores every position and velocity for a given level
    public Moment[] Moments;
    //might want to set to make specific levels shorter or long
    public int levelLength;
    //prevents using points that werent used the last time the player was recorded
    public int lastFrame = 0;

    //speed and jump vel should be changed in the inspector
    public float speed = 10f;
    public float jumpVel = 5f;
    public bool inAir = false;
    public float rayOffset = 0.22f;
	public State myState;
    public bool dead = false;
    public int timeOfDeath = -1;
	public Mesh basicCube, basicSphere;
    public Vector3 velocity;

    public Vector3 throwSpeed = new Vector3(10f, 2f, 0f);
    public bool thrown;
    public int throwDelay;

    // Use this for initialization
    void Start() {
        player = this;
		dead = false;
		timeOfDeath = 250000;
		Moments = new Moment[2000];
    }

    // Update is called once per frame
    void FixedUpdate() {
        //only the selected player is active
		if (!Main.MCU.rewind) {
            Move();
        } else {
			if (Main.MCU.currentFrame < 5) {
				GetComponent<Rigidbody> ().useGravity = false;
				GetComponent<Rigidbody> ().velocity = Vector3.zero;
			} else {
				GetComponent<Rigidbody> ().useGravity = true;
			}
            Rewind();
        }

		//render different states
		if (myState == State.Throw) {
			GetComponent<MeshFilter> ().mesh = basicSphere;
		} else {
			GetComponent<MeshFilter> ().mesh = basicCube;
		}

        if (Main.MCU.currentFrame >= throwDelay){
        	thrown = false;
        }
		if (dead) {
			this.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.GetComponent<Rigidbody> ().useGravity = false;
			this.GetComponent<BoxCollider> ().enabled = false;
			//velocity = Vector3.zero;
		} else {
			this.GetComponent<BoxCollider> ().enabled = true;
			this.GetComponent<Rigidbody> ().useGravity = true;
		}
        if (Main.MCU.currentFrame >= throwDelay){
        	thrown = false;
        }

        if (Main.MCU.currentFrame < timeOfDeath) {
            dead = false;
        }

		if (Main.MCU.currentFrame > timeOfDeath + 0) {
			Main.MCU.reset ();
		}
    }

    bool GroundCheck() {
        //requires that any floor be tagged and in layer "Ground"
        return !(Physics.Raycast(transform.position, Vector3.down, rayOffset, Main.MCU.groundMask)
            || Physics.Raycast((transform.position + rayOffset * Vector3.left), Vector3.down, rayOffset, Main.MCU.groundMask)
            || Physics.Raycast((transform.position + rayOffset * Vector3.right), Vector3.down, rayOffset, Main.MCU.groundMask)
            || Physics.Raycast(transform.position, Vector3.down, rayOffset, Main.MCU.cloneMask)
            || Physics.Raycast((transform.position + rayOffset * Vector3.left), Vector3.down, rayOffset, Main.MCU.cloneMask)
            || Physics.Raycast((transform.position + rayOffset * Vector3.right), Vector3.down, rayOffset, Main.MCU.cloneMask));
    }

    void Move() {
		if (thrown)
        {
            Moments[Main.MCU.currentFrame].position = this.gameObject.transform.position;
            Moments[Main.MCU.currentFrame].velocity = velocity;
            Moments[Main.MCU.currentFrame].type = State.None;
            return;
        }
			
        if (dead)
            return;
        inAir = GroundCheck();

        float horizontal_input = Input.GetAxis("Horizontal");
        velocity.x = horizontal_input * speed;
        bool jump = Input.GetKey(KeyCode.Space);

        if (jump && !inAir) {
            velocity.y = jumpVel;
        } else {
            velocity.y = this.GetComponent<Rigidbody>().velocity.y;
        }
        //adjust speed
        this.GetComponent<Rigidbody>().velocity = velocity;

        Moments[Main.MCU.currentFrame].position = this.gameObject.transform.position;
        Moments[Main.MCU.currentFrame].velocity = velocity;

        Moments[Main.MCU.currentFrame].type = State.None;
        if (Input.GetKey(KeyCode.C) && velocity == Vector3.zero && !inAir){
        	Moments[Main.MCU.currentFrame].type = State.Throw;
        }
		myState = Moments [Main.MCU.currentFrame].type;
    }

    void Rewind() {
        this.gameObject.transform.position = Moments[Main.MCU.currentFrame].position;
        thrown = false;
		myState = Moments [Main.MCU.currentFrame].type;
    }

   public void reset() {
		thrown = false;
        transform.position = Moments[0].position;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void OnTriggerEnter(Collider coll){
		print ("collided with " + coll.gameObject.name);
    	if (coll.tag == "Player" && Clone.clone.type == State.Throw){
    		GetThrown();
    	}
    }

    void GetThrown(){
    	thrown = true;
    	throwDelay = Main.MCU.currentFrame + 45;
    	if (velocity.x > 0){
    		velocity = throwSpeed;
    		this.GetComponent<Rigidbody>().velocity = velocity;
    	}
    	else {
    		velocity = throwSpeed;
    		velocity.x = -velocity.x;
    		this.GetComponent<Rigidbody>().velocity = velocity;
    	}
    }
}
