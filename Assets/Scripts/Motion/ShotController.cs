using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {

	private int layermask;


	// Use this for initialization
	void Start () 
	{
		layermask = LayerMask.GetMask("Obstacles","Ground","CameraWall");
//		rb = gameObject.GetComponent<Rigidbody2D>();
	//	rb.velocity = new Vector2( 800.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsInGroundContact()) 
		{
			rigidbody2D.velocity = new Vector2( rigidbody2D.velocity.x, 500.0f);
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{

		// If shot hits an enemy, it dies
		if (coll.gameObject.GetComponent<Enemy> () != null)
			coll.gameObject.GetComponent<Enemy> ().Kill ();
		
		// Shot is destroyed on all collisions
		Destroy (gameObject);
	}


	// Checks if shot is close to ground
	bool IsInGroundContact()
	{
		// 20 pixels y-distance is minimum
		Vector2 checkPoint = transform.position + new Vector3 (0.0f, -20.0f, 0.0f);

		Collider2D[] collidersHit = Physics2D.OverlapPointAll (checkPoint, layermask);

		return (collidersHit.Length > 0);
	}

}
