using UnityEngine;
using System.Collections;

public enum POWER{NONE,BIG,INVINCIBLE,FIRE};

public abstract class SuperPower : MonoBehaviour{

	void Start(){
		Activate();
	}

	abstract protected void Activate();
	abstract public void Remove();
}
