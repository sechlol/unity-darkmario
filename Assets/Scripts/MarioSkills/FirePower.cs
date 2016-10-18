using UnityEngine;
using System.Collections;

public class FirePower : SuperPower {
		
	void Start(){
		Activate();
	}

	override protected void Activate(){
		GetComponent<SpriteRenderer>().color = Color.red;
	}
	
	override public void Remove(){
		GetComponent<SpriteRenderer>().color = Color.white;
		Destroy(this);
	}

}
