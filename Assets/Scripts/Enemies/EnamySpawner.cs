using UnityEngine;
using System.Collections;

public class EnamySpawner : MonoBehaviour {

	public GameObject[] enemies;

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.GetComponent<Mario>() != null){
			for(int i=0;i<enemies.Length;i++)
				enemies[i].SetActive(true);
			Destroy(gameObject);
		}
	}
}
