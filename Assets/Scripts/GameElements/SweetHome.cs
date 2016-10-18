using UnityEngine;
using System.Collections;

public class SweetHome : MonoBehaviour {

	public GameObject FireWorks;
	public AudioClip HowlClip;
	public int HowManyFires = 10;
	public float Interval = 0.5f;

	private bool isExploded = false;

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.GetComponent<Mario>() && !isExploded){
			isExploded = true;
			audio.PlayOneShot(HowlClip);

			Invoke("PreKaboom", 1.5f);

			// Put Mario behind the cabin and disable movement. Causes a glitch in camera position.
			coll.gameObject.transform.position = transform.position + new Vector3(0.0f,0.0f,1000.0f);
			coll.GetComponent<SMBPhysicsBody>().enabled = false;
		}
	}

	private void PreKaboom(){
		StartCoroutine(KABOOM());
	}

	private IEnumerator KABOOM(){
		GameState.gameEnded = true;
		Bounds homebounds = GetComponent<SpriteRenderer> ().sprite.bounds;

		float extentY = homebounds.extents.y;
		float extentX = homebounds.extents.x;

		Vector3 randPos = new Vector3(0,0,800);
		Vector2 center = transform.position;
		center.y += extentY;


		for(int i=0;i<HowManyFires;i++){
			Vector2 rand = Random.insideUnitCircle;
			randPos.x = (rand.x * extentX * 1.5f) + center.x;
			randPos.y = (Mathf.Abs(rand.y) * extentY * 1.5f) + center.y;
			Instantiate(FireWorks,randPos,Quaternion.identity);
			yield return new WaitForSeconds(Interval);
		}
	}
}
