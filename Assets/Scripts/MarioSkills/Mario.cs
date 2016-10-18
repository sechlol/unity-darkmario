using UnityEngine;
using System.Collections;


public class Mario : MonoBehaviour {

	public Callback OnMarioDie;

	private int _score;
	private int _coins;
	private int _lives = 3;

	private Collider2D _marioCollider;
	private ItemCollector _collector = null;
	private MarioMovement _motion = null;
	private SuperPower big, fire, star;
	private LayerMask bricksLayer, enemyLayer,runningEnemyLayer;
	private bool _alreadyHitBrick = false;
	private Vector2 _topR, _botL;
	private float _lastY, _currentY=0;
	private AudioSource _audio;
	private Animator _animator;
	private bool _HitByEnemyCooldown = false;

	public int Score{get{return _score;}}
	public int Coins{get{return _coins;}}
	public int Lives{get{return _lives;}}


	public AudioClip CollectCoinClip;
	public AudioClip EatMushroomClip;
	public AudioClip HitByEnemyClip;
	public AudioClip KillEnemyClip;
	public AudioClip MarioDiesClip;


	public float DelayAfterHit = 3.0f;

	void Start(){
		big = star = fire = null;
		bricksLayer = LayerMask.NameToLayer("Obstacles");
		enemyLayer  = LayerMask.NameToLayer("Enemies");
		runningEnemyLayer = LayerMask.NameToLayer("RunningEnemies");

		_motion = GetComponent<MarioMovement>();
		_collector = GetComponent<ItemCollector>();
		_collector.OnItemCollision += pickItem;

		_marioCollider = GetComponent<BoxCollider2D>();
		_currentY = transform.position.y;
		_audio = gameObject.AddComponent<AudioSource>();
		_animator = GetComponent<Animator>();
	}


	void LateUpdate(){
		if(_currentY != 0)    
			_lastY = _currentY;
		_currentY = transform.position.y;
//		Debug.Log(_lastY+" - "+ _currentY);

		if(_alreadyHitBrick && _motion.Grounded)
			_alreadyHitBrick = false;
	}


	void OnCollisionEnter2D(Collision2D coll){
		if(coll == null)
			return;

		//Hit a brick 
		if(coll.gameObject.layer == bricksLayer && _alreadyHitBrick != true){

			Brick brick = HitBrickWithHead();
			//Debug.Log("hit "+coll.gameObject.name+" "+(brick==null));
			if(brick != null){
				brick.OnHit(gameObject);
				_alreadyHitBrick = true;
			}
		}

		//Hit an enemy
		else if(coll.gameObject.layer == enemyLayer || coll.gameObject.layer == runningEnemyLayer){
			Enemy enemy = coll.contacts[0].collider.gameObject.GetComponent<Enemy>();
			if(enemy != null){
				//mario stomped the enemy
				if(HasStomped(coll)){
					_motion.Jump();
					_score += enemy.Stomped(this);
					_audio.PlayOneShot(KillEnemyClip);
				}
				//mario collided with the enemy 
				else if(star != null){
					_score += enemy.pointsWhenKilled;
					enemy.Kill();
				}
				else
					enemy.OnMarioContact(this,coll);
			}
		}
	}

	private bool HasStomped(Collision2D coll){

		//if moving straight or jumping up

		if(_lastY <= _currentY){
			Debug.Log("Not jumping down.. prev: "+_lastY+" curr: "+_currentY);
			return false;
		}

		//HACK: correct previous Y if OnCollision was called before Update
		/*if(_currentY != transform.position.y){
			_lastY = _currentY;
			_currentY = transform.position.y;
			Debug.Log("correct. Prev: "+_lastY+" Curr: "+_currentY);
		}*/

		float feetBefore = _lastY - _marioCollider.bounds.extents.y;
		float enemyHead  = coll.gameObject.transform.position.y + coll.collider.bounds.extents.y; 

		//Before, mario's feet were below enemy's head
		if(feetBefore+40 <= enemyHead){
			Debug.Log("feetBefore "+feetBefore+" enemy: "+enemyHead);
			return false;
		}
		/*/
		//Check Collision normal vector. If vertical facing down, we stomped the enemy
		Debug.Log("**** Enemy! Contacts: "+coll.contacts.Length);
		bool isStomped = false;
		for(int i=0; i< coll.contacts.Length && isStomped == false; i++){

			if(coll.contacts[i].normal.y == 1.0f)
				isStomped = true;
		}
		/*/

		return true;
		//*/
	}

	private Brick HitBrickWithHead(){
		_marioCollider = GetComponent<BoxCollider2D>();

		//shrink width by 20% so it doesn't collide on sides
		float shrinkAmount = _marioCollider.bounds.extents.x * 0.2f;
		
		//Calculate triggering area for the head
		/*_botL.x = transform.position.x - _marioCollider.bounds.extents.x + shrinkAmount;
		_botL.y = transform.position.y + _marioCollider.bounds.extents.y-1;
		
		_topR.x = transform.position.x + _marioCollider.bounds.extents.x - shrinkAmount;
		_topR.y = _botL.y + 20;
*/
		_botL.x = _marioCollider.bounds.center.x - _marioCollider.bounds.extents.x + shrinkAmount;
		_botL.y = _marioCollider.bounds.center.y + _marioCollider.bounds.extents.y-1;
		
		_topR.x = _marioCollider.bounds.center.x + _marioCollider.bounds.extents.x - shrinkAmount;
		_topR.y = _botL.y + 20;


	;


		//if(coll.contacts[0].otherCollider.tag == "MarioHead"){
		Collider2D[] colliders = Physics2D.OverlapAreaAll(_topR,_botL,LayerMask.GetMask("Obstacles"));

//		Debug.Log("collisions "+colliders.Length+": "+_topR+" "+_botL);

		if(colliders.Length == 0)
			return null;

		if(colliders.Length == 1){
			return colliders[0].GetComponent<Brick>();
		}



		//hit more than one brick. take the one closer to the center of the head
		float distance;
		float minDistance = Mathf.Abs(colliders[0].transform.position.x - transform.position.x);
		int theBrick = 0;
		for(int i=1; i<colliders.Length;i++){
			distance = Mathf.Abs(colliders[i].transform.position.x - transform.position.x);
			if(distance < minDistance){
				minDistance = distance;
				theBrick = i;
			}
		}
		return colliders[theBrick].GetComponent<Brick>();
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(coll != null)
			if(coll.tag == "DeathPit")
				Die();
	}

	public void Die(){
		_lives--;
		_audio.PlayOneShot(MarioDiesClip);
		_motion.SetState(STATE.DIED);
		rigidbody2D.isKinematic = true;
		Destroy(_marioCollider);


		OnMarioDie();
	}

	public void HitByEnemy(){
		if(star == null && !_HitByEnemyCooldown){
			if(fire != null){
				fire.Remove();
				fire = null;
			}

			if(big != null){
				big.Remove();
				_audio.PlayOneShot(HitByEnemyClip);
				big = null;
			}
			else{
				Die();
				return;
			}
			StartCoroutine(ApplyInvulnerability());
		}
	}

	private void pickItem(Item item){

		if(item.ItemType == ITEM.COIN){
			_coins++;
			_audio.PlayOneShot(CollectCoinClip);
		}
		else{
			_audio.PlayOneShot(EatMushroomClip);
			switch(item.superpower){
				case POWER.BIG:
					if(big == null)
						big = gameObject.AddComponent<BigPower>();
					break;
				case POWER.FIRE:
					if(big == null)
						big = gameObject.AddComponent<BigPower>();
					else if(fire == null)
						fire = gameObject.AddComponent<FirePower>();
					break;
				case POWER.INVINCIBLE:
					
					if(star == null)
						star = gameObject.AddComponent<StarPower>();
					else
						(star as StarPower).Reload();
					break;
			}
		}

		_score += item.points;
		Destroy(item.gameObject);
	}

	//Needed when hitting a brick and gets a coin from it
	//HACK: Coin's score must not be hardcoded like this....
	public void GetCoin(){
		_coins++;
		_score+=200;
	}
	
	IEnumerator ApplyInvulnerability()
	{
		//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),true);
		_animator.SetBool("Invulnerable",true);
		_HitByEnemyCooldown = true;
		yield return new WaitForSeconds(DelayAfterHit);
		_HitByEnemyCooldown = false;
		_animator.SetBool("Invulnerable",false);
		//Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),false);
	}
}
