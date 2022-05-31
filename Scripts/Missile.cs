using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
	[SerializeField] public Explosives ex;
	public Rigidbody2D rb;
	Quaternion rotat;
	public Vector2 shootpoint;
	public Vector2 missileRotat;
	
	
	private void Update()
	{
		
		Vector2 dir = rb.velocity;
		
		rotat = Quaternion.LookRotation(dir.normalized, missileRotat);
		
		rotat.x = 0;
		rotat.y = 0;
		transform.rotation = rotat;
		if (Vector2.Distance(shootpoint, gameObject.transform.position) >= 500)
			ex.DestroyObjectAndExplode(gameObject,1.5f,8,30);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		gameObject.SetActive(false);
		
		ex.DestroyObjectAndExplode(gameObject,1.5f,8,30);
	}

	
}
