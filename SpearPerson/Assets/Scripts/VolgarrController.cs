using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(RaycastCollisionChecks))]
public class VolgarrController : MonoBehaviour {

	//Key inputs
	public KeyCode jumpKey;
	public KeyCode leftKey;
	public KeyCode rightKey;

	//Unity components
	Rigidbody2D rb;
	RaycastCollisionChecks colInfo;

	//Gameplay variables
	private bool airJump = false;
	private bool jumpedOnce = false;
	[HideInInspector] public bool throwingSpear = false;

	//Physics variables - We set these
	private float maxJumpHeight = 2.0f;                 // If this could be in actual unity units somehow, that would be great
	private float minJumpHeight = 0.5f;                 // If this could be in actual unity units somehow, that would be great
	private float timeToJumpApex = 0.3f;                // This is in actual seconds
	private float maxMovespeed = 8;                     // If this could be in actual unity units per second somehow, that would be great
	private float maxAirspeed = 6;                      // If this could be in actual unity units per second somehow, that would be great
	private float accelerationTime = 0.1f;              // This is in actual seconds
	private float deccelerationTime = 0.1f;             // This is in actual seconds
	private float airDeccelerationTime = 100000.0f;		// This is effectively removes air-decceleration

	//Physics variables - These get set for us
	private float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float acceleration;
	private float decceleration;
	private float airDecceleration;

	//Physics variables - State variables
	[SerializeField] float leftSpeed = 0.0f;
	[SerializeField] float rightSpeed = 0.0f;
	[SerializeField] float upSpeed = 0.0f;
	private bool previousFrame = false;
	public float direction = 1.0f;						// 1.0f is right, -1.0 is left

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		colInfo = GetComponent<RaycastCollisionChecks>();
		SetupMoveAndJumpSpeed();
	}

	void Update() {
		if (Input.GetKey(leftKey)) {
			direction = -1.0f;
			transform.localRotation = Quaternion.Euler(0, 180, 0);
		}
		if (Input.GetKey(rightKey)) {
			direction = 1.0f;
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}

		HorizontalGroundSpeed();
		Jumping();
		previousFrame = colInfo.down;
	}

	void HorizontalGroundSpeed() {
		if (colInfo.down) {
			if (Input.GetKey(leftKey)) {
				direction = -1.0f;
				leftSpeed += acceleration * Time.deltaTime;
			} else {
				leftSpeed -= decceleration * Time.deltaTime;
			}
			leftSpeed = Mathf.Clamp(leftSpeed, 0, maxMovespeed);

			if (Input.GetKey(rightKey)) {
				direction = 1.0f;
				rightSpeed += acceleration * Time.deltaTime;
			} else {
				rightSpeed -= decceleration * Time.deltaTime;
			}
			rightSpeed = Mathf.Clamp(rightSpeed, 0, maxMovespeed);

			if(throwingSpear){
				rightSpeed = 0;
				leftSpeed = 0;
			}

			rb.velocity = new Vector2(rightSpeed + -leftSpeed, rb.velocity.y);
		}
	}

	void Jumping() {

		bool jumpPressed = Input.GetKey(jumpKey);
		bool leftPressed = Input.GetKey(leftKey);
		bool rightPressed = Input.GetKey(rightKey);

		//Ground jumping
		if (colInfo.down) {

			jumpedOnce = false;
			//airJump = true;

			if (jumpPressed && leftPressed && !rightPressed) {              //Left diagonal jump
				direction = -1.0f;
				upSpeed = maxJumpVelocity;
				leftSpeed = maxAirspeed;
				rightSpeed = 0;
				//Debug.Log("Right diagonal jump");

			} else if(jumpPressed && !leftPressed && rightPressed){         //Right diagonal jump
				direction = 1.0f;
				upSpeed = maxJumpVelocity;
				leftSpeed = 0;
				rightSpeed = maxAirspeed;
				//Debug.Log("Left diagonal jump");

			} else if(jumpPressed){                                         //Ordinary jump
				upSpeed = maxJumpVelocity;
				leftSpeed = 0;
				rightSpeed = 0;
				//Debug.Log("Ordinary jump");
			}
		}

		//Air jumping, we can only air jump if we are in the air, have already jumped once (let go of the button) and have an airjump left
		if (!colInfo.down && airJump && jumpedOnce) {
			if (jumpPressed && leftPressed && !rightPressed) {					//Left diagonal jump
				direction = -1.0f;
				upSpeed = maxJumpVelocity;
				leftSpeed = maxAirspeed;
				rightSpeed = 0;
				airJump = false;
				//Debug.Log("Air right diagonal jump");

			} else if (jumpPressed && !leftPressed && rightPressed) {			//Right diagonal jump
				direction = 1.0f;
				upSpeed = maxJumpVelocity;
				leftSpeed = 0;
				rightSpeed = maxAirspeed;
				airJump = false;
				//Debug.Log("Air left diagonal jump");

			} else if (jumpPressed) {											//Ordinary jump
				upSpeed = maxJumpVelocity;
				leftSpeed = 0;
				rightSpeed = 0;
				airJump = false;
				//Debug.Log("Air ordinary jump");
			}
		}

		//Can we air jump / have we let go of the button?
		if (!colInfo.down && !jumpPressed) {
			jumpedOnce = true;
		}

		//Air decceleration
		if (!colInfo.down) {
			if (leftSpeed > 0) {
				leftSpeed -= airDecceleration * Time.deltaTime;
			}
			leftSpeed = Mathf.Clamp(leftSpeed, 0, maxAirspeed);

			if (rightSpeed > 0) {
				rightSpeed -= airDecceleration * Time.deltaTime;
			}
			rightSpeed = Mathf.Clamp(rightSpeed, 0, maxAirspeed);
		}

		//Maybe still have jump height depend on button hold?
		if (upSpeed > minJumpVelocity && !jumpPressed) {
			upSpeed = minJumpVelocity;
		}

		//Applying gravity
		if (!colInfo.down){
			upSpeed -= gravity * Time.deltaTime;
		}

		//To prevent that gravity builds up on platforms
		if(previousFrame && !colInfo.down && !jumpPressed){
			upSpeed = 0;
		}

		rb.velocity = new Vector2(rightSpeed + -leftSpeed, upSpeed);
	}

	void SetupMoveAndJumpSpeed() {
		//Scale gravity and jump velocity to jumpHeights and timeToJumpApex
		gravity = (2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = gravity * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * gravity * minJumpHeight);

		//Scale acceleration values to the movespeed and wanted acceleration times
		acceleration = maxMovespeed / accelerationTime;
		decceleration = maxMovespeed / deccelerationTime;
		airDecceleration = maxMovespeed / airDeccelerationTime;
	}
}
