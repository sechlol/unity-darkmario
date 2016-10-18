using UnityEngine;
using System.Collections;

public enum ITEM{MUSHROOM,STAR,FLOWER,COIN};

public class Item : MonoBehaviour {

	public int points = 200;
	public ITEM ItemType;
	public POWER superpower;
	
}
