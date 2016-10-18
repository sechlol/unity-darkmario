using UnityEngine;
using System.Collections;

public class Scrolling : MonoBehaviour 
{
	public Transform Maincamera;
	
	public float scrollFactor = 1.0f;
	
	
	
	// Update is called once per frame
	//public float scrollSpeed = 0.5F;
	void Update() 
	{
		//	float offset = Time.time * scrollSpeed;
		renderer.material.SetTextureOffset("_MainTex", new Vector2( Maincamera.position.x/2048.0F * scrollFactor, 0.0f));
	}
}
