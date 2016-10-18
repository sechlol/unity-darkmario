using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour 
{


	static public bool Left()
	{
		if (Input.GetAxis("Horizontal") < 0 )
			return true;

		return false;
	}

	static public bool Right()
	{
		if (Input.GetAxis("Horizontal") > 0)
			return true;

		return false;
	}

	static public bool Jump()
	{
		if (Input.GetButton("Fire2") )
			return true;

		if (Input.GetKey(KeyCode.LeftAlt))
			return true;

		return false;
	}

	static public bool RunOrFire()
	{
		if (Input.GetButton("Fire1") )
			return true;

		if (Input.GetKey(KeyCode.LeftShift))
			return true;
		
		return false;
	}



	static public bool JumpDown()
	{
//		if (Input.GetButtonDown("Vertical"))
//			return true;

		if (Input.GetButtonDown ("Fire2"))
			return true;
				
		if (Input.GetKeyDown(KeyCode.LeftAlt))
			return true;

		return false;
	}

	static public bool Exit()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			return true;

		return false;
	}


}
