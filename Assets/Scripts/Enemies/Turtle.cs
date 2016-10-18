using UnityEngine;
using System.Collections;

public class Turtle : Enemy {

	public override int Stomped (Mario mario)
	{
		GameObject shell = Resources.Load("Shell", typeof(GameObject)) as GameObject;
		Vector3 pos = transform.position;
		Quaternion rot = transform.rotation;
		Kill (mario);
		Instantiate(shell,pos,rot);
		return 0;
	}

	public override void Kill (Mario mario = null)
	{
		if(mario == null)
			base.Kill();
		else{
			Destroy(gameObject);
		}
	}


}
