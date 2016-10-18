using UnityEngine;
using System.Collections;

public enum STATE { STANDING, RUNNING, JUMPING, SKIDDING, DIED };
public class MarioMovement : MonoBehaviour 
{
	// pixels / s
	float maxWalkSpeed 			= 468.75F;
	float minWalkSpeed			= 50.0f;
	float maxRunSpeed			= 768.75F;
	float jumpSpeed 			= 1200.0f;

	// pixels / s^2
	float walkAccelleration 	= 668.0F;
	float runAccelleration 		= 1002.0F;
	float releaseDecelleration 	= 703.0F;
	float skiddingDecelleration = 1828.0F;
	float jumpXDecelleration 	= 1002.0F;

	private float nextFire = 0.0f;

	public float fireRate = 0.5f; 

	public AudioClip JumpClip;
	public AudioClip WalkClip;
	public AudioClip LandClip;
	public AudioClip FireClip;
	private AudioSource _audio;
	

	SMBPhysicsBody body;
	ObstacleCollision obsColls;

	Animator anim;
	
	private STATE state;
	private enum DIRECTION {RIGHT, LEFT};
	private DIRECTION direction = DIRECTION.RIGHT;

	public GameObject shotPrefab;

	public bool Grounded{get{return IsGrounded();}}


	void Start () 
	{
		state = STATE.STANDING;
		anim = GetComponent<Animator> (); 
		_audio = gameObject.AddComponent<AudioSource>();
	}
	
	void Awake()
	{
		body = gameObject.GetComponent<SMBPhysicsBody>();
		obsColls = gameObject.GetComponent<ObstacleCollision>();
	}

	public void SetState( STATE newState )
	{
		if (state == newState)
			return;

		state = newState;

		switch (state) 
		{
				case STATE.STANDING: 
						anim.SetTrigger ("Standing");
						break;
				case STATE.RUNNING: 
						anim.SetTrigger ("Running");
						break;
				case STATE.JUMPING: 
						anim.SetTrigger ("Jumping");
						break;
				case STATE.SKIDDING: 
						anim.SetTrigger ("Skidding");
						break;
				case STATE.DIED:
						anim.SetTrigger ("Died");
						break;

		}
	}


	public void Jump(){
		body.velocity.y = jumpSpeed;
		SetState(STATE.JUMPING);
		_audio.PlayOneShot(JumpClip);
	}
	// Update is called once per frame
	void Update () 
	{

		// If Jump-button is just pressed down and Mario is standing on ground
		if (UserInput.JumpDown() && IsGrounded() ) 
		{
			Jump();
		}

		if ( IsGrounded() && state == STATE.JUMPING)
			_audio.PlayOneShot(LandClip);

		if ( IsGrounded() )
		{
			//if(WalkClip){

				// If Mario is moving other way and player steeres other way, Mario skids and decellerates faster
				if ( IsBraking () )
				{
					Decellerate( skiddingDecelleration * Time.deltaTime );
					SetState(STATE.SKIDDING);
				}
				else if (Mathf.Abs (body.velocity.x) < minWalkSpeed)
				{
					SetState(STATE.STANDING);
				}
				else
				{
					SetState(STATE.RUNNING);
					_audio.clip = WalkClip;
					if(!_audio.isPlaying)
						_audio.Play();
				}
			//}
		}

		if (!IsGrounded() && IsBraking () ) 
		{
			Decellerate( jumpXDecelleration * Time.deltaTime );
		}



		


		if ( UserInput.Right() && body.velocity.x >= 0.0f )
		{
			if (UserInput.RunOrFire())
			{
				body.velocity.x += runAccelleration * Time.deltaTime;
			
				if (body.velocity.x > maxRunSpeed)
					body.velocity.x = maxRunSpeed;
			}
			else
			{
				body.velocity.x += walkAccelleration * Time.deltaTime;
				
				if (body.velocity.x > maxWalkSpeed)
					body.velocity.x = maxWalkSpeed;
			}
		}
		
		if ( UserInput.Left() && body.velocity.x <= 0.0f)
		{
			if (UserInput.RunOrFire())
			{
				body.velocity.x -= runAccelleration * Time.deltaTime;
			
				if (body.velocity.x < -maxRunSpeed)
					body.velocity.x = -maxRunSpeed;
			}
			else
			{
				body.velocity.x -= walkAccelleration * Time.deltaTime;
				
				if (body.velocity.x < -maxWalkSpeed)
					body.velocity.x = -maxWalkSpeed;
			}

		}
		
		// If player let's go of controller, Mario decellerates slowly if grounded
		if (!UserInput.Left () && !UserInput.Right () && IsGrounded() ) 
		{
			Decellerate( releaseDecelleration * Time.deltaTime );
		}


		// Mario is facing the direction it's moving
		if (body.velocity.x > 1.0f)
		{
			direction = DIRECTION.RIGHT;
			// The sprite is flipped using x-scale of tranformation
			Vector3 scale = transform.localScale;
			scale.x = 1;
			transform.localScale = scale;
		}

		if (body.velocity.x < -1.0f)
		{
			direction = DIRECTION.LEFT;
			Vector3 scale = transform.localScale;
			scale.x = -1;
			transform.localScale = scale;
		}

		// FIRING

		if (UserInput.RunOrFire() && Time.time > nextFire && gameObject.GetComponent<FirePower>()!=null ) 
		{
			GameObject shot = Instantiate (shotPrefab, transform.position,transform.rotation) as GameObject;
			_audio.PlayOneShot(FireClip);
			Rigidbody2D shotRB = shot.GetComponent<Rigidbody2D>();
			if (direction == DIRECTION.RIGHT)
			{
				shotRB.velocity = new Vector2( 800.0f, 0.0f );
				shot.transform.position = transform.position + new Vector3(60.0f, 50.0f, 0.0f);
			}
			else
			{
				shotRB.velocity = new Vector2( -800.0f, 0.0f );
				shot.transform.position = transform.position + new Vector3(-60.0f, 50.0f, 0.0f);
			}

			_audio.PlayOneShot(FireClip);
			SetState(STATE.JUMPING);	
			nextFire = Time.time + fireRate;
			
		}


		// If jump-button is being held down, gravity is lower making jump higher
		if (UserInput.Jump() ) 
		{
			body.marioGravity = 2250.0f;
		}
		else
		{
			body.marioGravity = 7875.0f;
		}

		if (UserInput.Exit ()) {
			Application.LoadLevel("StartScreen");		
		}


	}


	private void Decellerate( float deltaV )
	{
		if ( Mathf.Abs(body.velocity.x) < deltaV )
		{
			body.velocity.x = 0;
		}
		else
		{
			if (body.velocity.x < 0.0f)
			{
				body.velocity.x += deltaV;
			}
			else
			{
				body.velocity.x -= deltaV;
			}	
		}
	}

	// Mario is grounded if he's touching ground and not moving upwards
	public bool IsGrounded()
	{
		return (body.velocity.y < jumpSpeed / 2.0F && obsColls.IsGrounded ());
	}

	private bool IsBraking()
	{
		return (UserInput.Right () && body.velocity.x < 0.0f ||
						UserInput.Left () && body.velocity.x > 0.0f);
	}
	
	private bool IsAccelerating()
	{
		return (UserInput.Right () && body.velocity.x >= 0.0f ||
		        UserInput.Left () && body.velocity.x <= 0.0f);
	}



	
}