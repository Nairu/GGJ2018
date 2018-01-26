using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    public bool canDoubleJump = false;
    public bool canWallJump = false;
    public bool canFallOffWalls = false;
    public bool canWallJumpOnSlopes = false;

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = .4f;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float moveSpeed = 6;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = .25f;
    public float wallDropoffVelocity = 0.1f;
	float timeToWallUnstick;

    [Range(1, 1000)]
    public float externalForceDragCoefficient = 10f;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	Vector2 directionalInput;
    Vector2 externalForce;
	bool wallSliding;
    bool doubleJumping;
	int wallDirX;

	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);
	}

	void Update() {
		CalculateVelocity ();        
		HandleWallSliding ();

		controller.Move (velocity * Time.deltaTime, directionalInput + externalForce);

		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

    public void AddForce(Vector2 externalForce)
    {
        this.externalForce = externalForce;
        doubleJumping = false;        
    }

	public void SetDirectionalInput (Vector2 input) {
		directionalInput = input;
	}

	public void OnJumpInputDown() {
		if (wallSliding && canWallJump) {
            if (wallDirX == directionalInput.x) {
				velocity.x = -wallDirX * wallJumpClimb.x;
				velocity.y = wallJumpClimb.y;
			}
			else if (directionalInput.x == 0) {
				velocity.x = -wallDirX * wallJumpOff.x;
				velocity.y = wallJumpOff.y;
			}
			else {
				velocity.x = -wallDirX * wallLeap.x;
				velocity.y = wallLeap.y;
			}
		}
        if (!controller.collisions.below && (canDoubleJump && !doubleJumping))
        {
            Debug.Log("Double jump!");
            //double jumping
            doubleJumping = true;
            velocity.y = maxJumpVelocity;
        }
		if (controller.collisions.below) {
            doubleJumping = false;
            if (controller.collisions.slidingDownMaxSlope && !canWallJumpOnSlopes) {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}
		
	void HandleWallSliding() {
		wallDirX = (controller.collisions.left) ? -1 : 1;
		wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;

			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;

                if (directionalInput.x != wallDirX && (!canFallOffWalls ? directionalInput.x != 0 : true)) {
					timeToWallUnstick -= Time.deltaTime;
				}
				else {                    
                    timeToWallUnstick = wallStickTime;
				}
			}
			else {
                //kick us off the wall and then reset the timer
                if(canFallOffWalls)
                    velocity.x -= (wallDropoffVelocity * wallDirX);
				timeToWallUnstick = wallStickTime;
			}

		}

	}

	void CalculateVelocity() {

        //we need to work out what the external force calculation is, and add it into our current calculation, then we need to make sure we decrement it appropriately as it isn't
        //going to be a constant force - just an impulse.

		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
        if (externalForce.x > 0 || externalForce.y > 0)
            velocity = externalForce;
        //velocity.x += externalForce.x;
        //velocity.y += externalForce.y;


        //now lets just reduce both of our external forces by some drag coefficient, lets say 5 for now.

        //externalForce.x = Mathf.Lerp(externalForce.x, 0, Time.deltaTime * externalForceDragCoefficient);
        //externalForce.y = Mathf.Lerp(externalForce.y, 0, Time.deltaTime * externalForceDragCoefficient);        
        externalForce = Vector2.zero;
    }
}
