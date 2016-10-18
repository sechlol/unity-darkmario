using UnityEngine;
using System.Collections;

public class ItemCollector : MonoBehaviour {
	[HideInInspector]
	public delegate void ItemCollision(Item item);

	[HideInInspector]
	public ItemCollision OnItemCollision;

	private LayerMask _itemLayer;

	void Awake(){
		_itemLayer = LayerMask.NameToLayer("Items");
	}

	void OnCollisionEnter2D(Collision2D coll){
		if(coll != null)
			callCollision(coll.collider);
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(coll != null)
			callCollision(coll);
	}

	private void callCollision(Collider2D coll){
		if(coll.gameObject.layer == _itemLayer){
			Item item = coll.gameObject.GetComponent<Item>() as Item;
			OnItemCollision(item);
		}
	}
}
