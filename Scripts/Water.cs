using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Water : MonoBehaviour
{
	public RoundManager rm;
	
	

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.transform.TryGetComponent<Worm>(out Worm w))
		{
			Camera.main.GetComponent<CameraSC>().StopFollow_Drag();
			w.Death();


		}
	}

	

	
}

