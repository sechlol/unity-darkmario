using UnityEngine;
using System.Collections;

public class ObstacleCollision : MonoBehaviour 
{
	
	private enum COLLISIONPOINT { GROUNDED1, GROUNDED2, RIGHT,  LEFT, 
		CORNER1, CORNER2, CORNER3, CORNER4, BIG1, BIG2};

	private enum DIRECTION {UP, DOWN, LEFT, RIGHT};
	
	private Transform[] collisionPoints;
	private Collider2D[] hits;
	private SMBPhysicsBody body;

	private int layermask;

	private  Vector3 oldPosition;

	private bool collisionOccurred;

	public bool IsGrounded()
	{
		return ( GetColliderAt(COLLISIONPOINT.GROUNDED1) != null || 
		         GetColliderAt(COLLISIONPOINT.GROUNDED2) != null    );
	}



	void Awake () 
	{

		layermask = LayerMask.GetMask("Obstacles","Ground","CameraWall");//(1 << LayerMask.NameToLayer("Obstacles")) | (1 << LayerMask.NameToLayer("Ground"));


		// Initialize array of collision points
		collisionPoints = new Transform[10];

		collisionPoints[(int)COLLISIONPOINT.GROUNDED1]   = transform.Find ("groundedCheck1").transform;
		collisionPoints[(int)COLLISIONPOINT.GROUNDED2]   = transform.Find ("groundedCheck2").transform;
		collisionPoints[(int)COLLISIONPOINT.LEFT]   	 = transform.Find ("left").transform;
		collisionPoints[(int)COLLISIONPOINT.RIGHT]   	 = transform.Find ("right").transform;


		collisionPoints[(int)COLLISIONPOINT.CORNER1]  	 = transform.Find ("corner1").transform;
		collisionPoints[(int)COLLISIONPOINT.CORNER2]  	 = transform.Find ("corner2").transform;
		collisionPoints[(int)COLLISIONPOINT.CORNER3]  	 = transform.Find ("corner3").transform;
		collisionPoints[(int)COLLISIONPOINT.CORNER4]  	 = transform.Find ("corner4").transform;

		collisionPoints[(int)COLLISIONPOINT.BIG1]  	 = transform.Find ("big1").transform;
		collisionPoints[(int)COLLISIONPOINT.BIG2]  	 = transform.Find ("big2").transform;


		hits = new Collider2D[5];
		
		body = gameObject.GetComponent<SMBPhysicsBody>();
		
		if (body == null) 
		{
			Debug.LogError("Collisions need BasicMovement to work");
		}
		
	}
	
	void Update () 
	{
		collisionOccurred = false;

		ProcessCollisionAt (COLLISIONPOINT.CORNER1);
		ProcessCollisionAt (COLLISIONPOINT.CORNER2);
		ProcessCollisionAt (COLLISIONPOINT.CORNER3);
		ProcessCollisionAt (COLLISIONPOINT.CORNER4);
		ProcessCollisionAt (COLLISIONPOINT.LEFT);
		ProcessCollisionAt (COLLISIONPOINT.RIGHT);

		if (IsBig ()) 
		{
			ProcessCollisionAt (COLLISIONPOINT.BIG1);
			ProcessCollisionAt (COLLISIONPOINT.BIG2);
		}

		if (collisionOccurred == false)
   		    oldPosition = new Vector2 (transform.position.x, transform.position.y);


	
	}


	void ProcessCollisionAt( COLLISIONPOINT cp )
	{

				Collider2D coll = GetColliderAt (cp);
		
				if (coll == null) 
						return;

				collisionOccurred = true;

		

			
				Vector2 pointPos = collisionPoints [(int)cp].position;
				// Calculate where this point was in last frame
				Vector2 pointOldPos = new Vector2 (pointPos.x - (transform.position.x - oldPosition.x),
			                                   pointPos.y - (transform.position.y - oldPosition.y));
			
			
			
				DIRECTION dir = CollisionDirection (pointPos, pointOldPos, coll.bounds.min, coll.bounds.max);
			
				if (dir == DIRECTION.DOWN) {
						body.velocity.y = 0.0f;
						while ( GetColliderAt(cp) != null)
							transform.position = new Vector2 (transform.position.x, transform.position.y + 1.0f);
				}
			
				if (dir == DIRECTION.UP) {
						body.velocity.y = 0.0f;
						while ( GetColliderAt(cp) != null)
							transform.position = new Vector2 (transform.position.x, transform.position.y - 1.0f);
				}
			
				if (dir == DIRECTION.LEFT) {
						body.velocity.x = 0.0f;
						while ( GetColliderAt(cp) != null)
							transform.position = new Vector2 (transform.position.x + 1.0f, transform.position.y);
				}
			
				if (dir == DIRECTION.RIGHT) {
						body.velocity.x = 0.0f;
						while ( GetColliderAt(cp) != null)
								transform.position = new Vector2 (transform.position.x - 1.0f, transform.position.y);
				}

	}



	DIRECTION CollisionDirection( Vector2 pointIn, Vector2 pointOut, Vector2 boxMin, Vector2 boxMax)
	{ 
		Vector2 topLeft = new Vector2 (boxMin.x, boxMax.y) ;
		Vector2 topRight = new Vector2 (boxMax.x, boxMax.y) ;
		Vector2 bottomLeft = new Vector2 (boxMin.x, boxMin.y) ;
		Vector2 bottomRight = new Vector2 (boxMax.x, boxMin.y) ;

		if (DoLineSegmentsIntersect (pointIn, pointOut, topLeft, topRight))
			return DIRECTION.DOWN;

		if (DoLineSegmentsIntersect (pointIn, pointOut, bottomLeft, bottomRight))
			return DIRECTION.UP;

		if (DoLineSegmentsIntersect (pointIn, pointOut, topLeft, bottomLeft))
			return DIRECTION.RIGHT;

		if (DoLineSegmentsIntersect (pointIn, pointOut, topRight, bottomRight))
			return DIRECTION.LEFT;

		Debug.Log ("BUG in collision direction detection");
		Debug.Log (pointIn);
		Debug.Log (pointOut);
		Debug.Log (boxMin);
		Debug.Log (boxMax);



		return DIRECTION.DOWN;

	}

	private bool IsBig()
	{
		return gameObject.GetComponent<BigPower> () != null;
	}


	Collider2D GetColliderAt(COLLISIONPOINT cp)
	{
		Vector2 position = collisionPoints [(int)cp].position;
		if ( Physics2D.OverlapPointNonAlloc (position, hits, layermask) > 0)
			return hits[0];
		
		return null;
	}




	bool CounterClockWise(Vector2 a, Vector2 b, Vector2 c)
	{
		if ( (c.y - a.y) * (b.x - a.x) > 
		     (b.y - a.y) * (c.x - a.x)   )
			return true;
		return false;
	}	


	bool DoLineSegmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
	{
		if (CounterClockWise (a, c, d) != CounterClockWise (b, c, d) && 
			CounterClockWise (a, b, c) != CounterClockWise (a, b, d))
				return true;
		return false;
	}

	
	
}