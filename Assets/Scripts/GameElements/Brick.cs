using UnityEngine;
using System.Collections;

public abstract class Brick : MonoBehaviour {

	private float _force = 3f;
	private Collider2D _collider;
	protected Animator _animator;


	void Awake(){
		_collider = GetComponent<BoxCollider2D>();
		_animator = GetComponent<Animator>();
	}

	public virtual void OnHit(GameObject hitter){
		KillAllEnemies();
		SpecialEffect(hitter);
	}

	public abstract void SpecialEffect(GameObject hitter);

	private void KillAllEnemies(){
		Vector2 topR, botL;

		topR = new Vector2(
			transform.position.x + _collider.bounds.extents.x,
			transform.position.y + _collider.bounds.extents.y*3
		);
		botL = new Vector2(
			transform.position.x - _collider.bounds.extents.x,
			transform.position.y + _collider.bounds.extents.y
		);
		LayerMask mask = LayerMask.GetMask("Enemies","Items","RunningEnemies");
		Collider2D[] colls = Physics2D.OverlapAreaAll(topR,botL,mask);

		for(int i=0;i<colls.Length;i++){
			//if enemy, kill the enemy
			Enemy enemy = colls[i].GetComponent<Enemy>();
			if(enemy != null)
				enemy.Kill();
			//if item, make it jump
			else{
				colls[i].rigidbody2D.AddForce(new Vector2(0.0f,_force));

				//flip the item according to their position relative to the block
				if(transform.position.x > colls[i].transform.position.x){
					AISimpleMovement movable = colls[i].GetComponent<AISimpleMovement>();
					if(movable.direction == DIRECTION.RIGHT)
						movable.Flip();
				}



			}
		}
	}

	/*void OnCollisionEnter2D(Collision2D coll){
		Debug.Log("collision enter "+coll.gameObject.name);
		if(coll != null && coll.gameObject.tag == "MarioHead")
			OnHit();
	}*/

}
