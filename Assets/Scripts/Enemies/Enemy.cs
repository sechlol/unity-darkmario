using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int lives = 1;
	public int pointsWhenKilled = 100;

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.tag == "DeathPit")
			Kill();
	}

	public virtual void OnMarioContact(Mario mario, Collision2D coll = null){
		mario.HitByEnemy();
	}

	public virtual int Stomped(Mario mario){
		lives--;
		if(lives == 0){
			Kill(mario);
			return pointsWhenKilled;
		}
		return 0;
	}

	public virtual void Kill(Mario mario = null){
		BoxCollider2D coll = GetComponent<BoxCollider2D>();
		AISimpleMovement move = GetComponent<AISimpleMovement>();
		Enemy enemy = GetComponent<Enemy>();

		Destroy(enemy);
		Destroy(move);
		Destroy(coll);
		rigidbody2D.AddForce(new Vector2(0.0f,23000.0f));

		Vector3 newScale = transform.localScale;
		newScale.y *= -1;
		transform.localScale = newScale;
		Destroy(gameObject,5.0f);
	}
}
