using UnityEngine;
using System.Collections;

public class Destructible : Brick {

	public AudioClip BounceAudio;
	public AudioClip BreakAudio;

	override public void SpecialEffect(GameObject hitter){
		if(hitter.GetComponent<BigPower>() != null){
			AudioSource.PlayClipAtPoint(BreakAudio,transform.position);
			_animator.SetTrigger("Break");
			Destroy(GetComponent<SpriteRenderer>());
			Destroy(gameObject,0.2f);
		}
		else{
			AudioSource.PlayClipAtPoint(BounceAudio,transform.position);
			_animator.SetTrigger("Bounce");
		}
	}

}
