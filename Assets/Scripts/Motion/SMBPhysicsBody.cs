using UnityEngine;
using System.Collections;


// This script does basic Super Mario Bros physics:
// It stores objects position and velocity and does
// basic integration between them and applies gravity.


public class SMBPhysicsBody : MonoBehaviour 
{
	
	// Default gravity
	public float marioGravity = 7875.0F; // pixels / sec ^ 2
//	private float gravity = 2250.0F; // pixels / sec ^ 2

	public Vector2 velocity;

	private MarioMovement movement;

	void Awake()
	{	
		movement = gameObject.GetComponent<MarioMovement>();
		velocity = new Vector2 (0.0f, 0.0f);
	}	


	void Update () 
	{
		// Integrate velocity into position
		transform.position = new Vector2( transform.position.x + velocity.x * Time.deltaTime, 
		                                 transform.position.y + velocity.y * Time.deltaTime);
		

		// Gravity pulls object down
		if(velocity.y > -2000 && !movement.IsGrounded())
			velocity.y -= marioGravity * Time.deltaTime;

		// Prevents physics engine moving Mario
		rigidbody2D.velocity = new Vector2 (0.0f, 0.0f);

	}
	
	
	
	
}