using UnityEngine;
using System.Collections;

public enum DIRECTION {RIGHT, LEFT};
public class AISimpleMovement : MonoBehaviour {

	public DIRECTION direction = DIRECTION.RIGHT;
	public float speed = 15.0f;
	public LayerMask layermask;

	private Transform frontCheck;
	private Collider2D[] hits;


	void Awake(){
		hits = new Collider2D[1];
		frontCheck = transform.Find("frontCheck").transform;

		//We assume that the frontCheck is always on the right, in the first instance
		if(direction == DIRECTION.LEFT)
			Flip ();
	}

	void FixedUpdate ()
	{
		int collisions = Physics2D.OverlapPointNonAlloc(frontCheck.position,hits,layermask); 
		if(collisions > 0){
			Flip();
		}
			
		
		// Set the enemy's velocity to moveSpeed in the x direction.
		rigidbody2D.velocity = new Vector2(transform.localScale.x * speed, rigidbody2D.velocity.y);	

	}
	
	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 currentScale = transform.localScale;
		currentScale.x *= -1;
		transform.localScale = currentScale;
		direction = direction == DIRECTION.RIGHT ? DIRECTION.LEFT : DIRECTION.RIGHT;
	}


}
