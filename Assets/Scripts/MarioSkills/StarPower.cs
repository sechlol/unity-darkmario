using UnityEngine;
using System.Collections;

public class StarPower : SuperPower {

	public float Duration = 5.0f;
	private float startTime = 0;
	private Animator _animator;

	void Start(){
		_animator = GetComponent<Animator>();
		Activate();
	}

	void Update(){
		if(startTime != 0){
			if(Time.time - startTime >= Duration)
				Remove();
		}
	}

	override protected void Activate(){
		_animator.SetBool("StarPower",true);
		startTime = Time.time;
	}
	
	override public void Remove(){
		_animator.SetBool("StarPower",false);
		Destroy(this);
	}

	public void Reload(){
		startTime = Time.time;
	}
}
