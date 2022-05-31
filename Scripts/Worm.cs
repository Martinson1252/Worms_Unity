using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using TMPro;

public class Worm : MonoBehaviour
{
	public WormManager wm;
    public Rigidbody2D rb;
    public BoxCollider2D groundCheck;
	public Animator anim;
	public GameObject cam;
	public Image healthBarFill;
	public new TextMeshProUGUI name;
	public Canvas upperUI;
	public float health,maxHealth;
	public bool ActiveWorm;
	public Team team;
	Vector3 lScale;
	public enum Team
	{
		red,blue
	}
	// Start is called before the first frame update
	private void Start()
	{
		maxHealth = health;
		wm = FindObjectOfType<WormManager>();
		
	}

	
	public void SetHealth(float value)
	{
		healthBarFill.fillAmount = this.health = value;
	}
	

	public void AddHealth(float value)
	{
		if (this.health + value <= 0) Death();
		else
		{
		this.health += value;
			wm.ui.SetBar(value, team);
		healthBarFill.fillAmount = this.health / maxHealth;
		}

	}

	public void DecreaseHealth(float value)
	{	
		if (this.health - value <= 0)
		{
			Death();
		}
		else
		{
			this.health -= value;
			wm.ui.SetBar(-value, team);
			healthBarFill.fillAmount = this.health / maxHealth;
		}
	}

	public void Death()
	{
		Debug.Log(this.health + " worm health");
		wm.Delete(gameObject);
	
		
		if (this.health == maxHealth)
			wm.ui.SetBar(-maxHealth, team);
		else
			wm.ui.SetBar( -(this.health),team);
		//gameObject.SetActive(false);
		wm.ui.DecreaseTeamCount(1,team);
		if (wm.activeWorm == gameObject)
		{
			Camera.main.GetComponent<CameraSC>().StopFollow_Drag();
			if (wm.rm.amIHost) wm.rm._host.SetOnlineRound();
			
		}
		Destroy(gameObject);
	}

	
}
