using UnityEngine;
using System.Collections;

public class Goomba : Enemy {

	public override void Kill (Mario mario = null)
	{
		if(mario == null)
			base.Kill();
		else{
			Destroy(GetComponent<AISimpleMovement>());
			GetComponent<Animator>().SetBool("IsDied",true);
			Destroy(gameObject,2.0f);
		}
	}

}
