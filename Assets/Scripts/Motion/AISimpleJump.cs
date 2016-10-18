using UnityEngine;
using System.Collections;

public class AISimpleJump : MonoBehaviour {

	public float everySeconds = 1;
	public float force = 100.0f;

	private float nextJump = 0.0f;
	private bool onGround = true;


	void FixedUpdate(){
		if(onGround){
			if (Time.time > nextJump ) {
				nextJump += everySeconds;
				rigidbody2D.AddForce(new Vector2(0.0f,force*100));

			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (!onGround && collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Obstacle") 
			onGround = true;
	}

	void OnCollisionExit2D(Collision2D collision){
		if (onGround && collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Obstacle") 
			onGround = false;
	}
}
