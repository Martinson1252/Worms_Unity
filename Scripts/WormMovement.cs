using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WormMovement : MonoBehaviour
{
	public Worm w;
	public Action Movement;
	InputManager inp;
	private void Start()
	{
		inp = FindObjectOfType<InputManager>();
		inp.InputJumpKeyEvent.AddListener(Jump);
		inp.InputAKeyEvent.AddListener(MoveLeft);
		inp.InputDKeyEvent.AddListener(MoveRight);
		inp.InputDKeyUPEvent.AddListener(StopRight);
		inp.InputAKeyUPEvent.AddListener(StopLeft);
	}

	public void RemoveAllMoveEvents()
	{
		inp.InputJumpKeyEvent.RemoveListener(Jump);
		inp.InputAKeyEvent.RemoveListener(MoveLeft);
		inp.InputDKeyEvent.RemoveListener(MoveRight);
		inp.InputDKeyUPEvent.RemoveListener(StopRight);
		inp.InputAKeyUPEvent.RemoveListener(StopLeft);
	}

	void StopLeft()
	{
		w.anim.SetBool("move", false);
	}

	void StopRight()
	{
		w.anim.SetBool("move", false);
	}

	void MoveLeft()
	{
		w.anim.SetBool("move", true);
		w.rb.velocity = new Vector2( Input.GetAxisRaw("Horizontal")* 33 * Time.fixedDeltaTime, w.rb.velocity.y);
		transform.localRotation = Quaternion.Euler(0, 180, 0);
		gameObject.transform.GetChild(0).gameObject.SetActive(false);
	}

	void MoveRight()
	{
		w.anim.SetBool("move", true);
		w.rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * 33 * Time.fixedDeltaTime, w.rb.velocity.y);
		transform.localRotation = Quaternion.Euler(0, 0, 0);
		gameObject.transform.GetChild(0).gameObject.SetActive(false);
	}
	
	private void Jump()
	{
		if ( w.anim.GetBool("OnGround"))  //Input.GetKeyDown(KeyCode.W) &&
		{
			w.anim.SetBool("jump", true);

			w.rb.AddForce(new Vector2(w.rb.velocity.x, 3.3f), ForceMode2D.Impulse);

		}


	}


	private void OnTriggerStay2D(Collider2D collision)
	{

		w.anim.SetBool("OnGround", true);

		gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{

		w.anim.SetBool("OnGround", false);

		gameObject.transform.GetChild(0).gameObject.SetActive(false);


	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		w.anim.SetBool("jump", false);
	}


}
