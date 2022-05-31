using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Weapons
{
   public string name;
   public WeaponControl.Type WeapT;
   public GameObject weapObj;
}

[System.Serializable]
public class TeamInventory
{
	public WeaponControl.Type weaponType;
	public float amount;
	public Text amountReference;
	public Button ButtonReference;
}

public class WormInventory : MonoBehaviour
{
    public Weapons[] weapon;
	public WormUI ui;
	[SerializeField] WormManager wm;
		public WeaponControl.Type t;
		GameObject weap;
	public List<TeamInventory> redTeamInventory;
	public List<TeamInventory> blueTeamInventory;

	private void Start()
	{
		wm = FindObjectOfType<WormManager>();
	}


	public void SetWormInventory()
	{
		RoundManager rman = FindObjectOfType<RoundManager>();
		switch (rman.teamRound)
		{
			case RoundManager.TeamRound.red:
				foreach(TeamInventory tm in redTeamInventory)
				{
					tm.amountReference.text = tm.amount.ToString();
					if (tm.amount > 0)
					{
						tm.ButtonReference.interactable = true;
					}
					else
						tm.ButtonReference.interactable = false;
				}
				break;
			case RoundManager.TeamRound.blue:
				foreach (TeamInventory tm in blueTeamInventory)
				{
					tm.amountReference.text = tm.amount.ToString();
					if (tm.amount > 0)
					{
						tm.ButtonReference.interactable = true;
					}
					else
						tm.ButtonReference.interactable = false;
				}
				break;
		}
	}



	public void SetTeamInventoryItem(float value, WeaponControl.Type type)
	{

		switch (wm.rm.teamRound)
		{
			case RoundManager.TeamRound.red:
				foreach (TeamInventory tm in redTeamInventory)
				{
					if (tm.weaponType == type)
					{
						if((tm.amount+value)>=0) { tm.amount += value; }
					}
				}
				break;
			case RoundManager.TeamRound.blue:
				foreach (TeamInventory tm in blueTeamInventory)
				{
					if(tm.weaponType==type)
					{
						if((tm.amount+value)>=0) { tm.amount += value; }

					}
				}
				break;
		}
	}

	//Dodanie broni Wormsowi
    public void AddWeapon(string weapName, GameObject activeWorm)
	{
		GameObject weapHolder = activeWorm.transform.GetChild(0).gameObject;
		if(weapHolder.transform.childCount!=0)
		{
		Destroy(weapHolder.transform.GetChild(0).gameObject);
		}
		if (ui.bottomIndicator.transform.childCount != 0) Destroy(ui.bottomIndicator.transform.GetChild(0).gameObject);
		foreach (Weapons w in weapon)
		{
			if(w.name==weapName)
			{
				t = w.WeapT;
				weap = w.weapObj;
				break;
			}
		} 
		//GameObject weap = Array.Find(weapon,  name => name.name == weapName).weapObj;
		if(weap!=null)
		{
		Instantiate(weap, weapHolder.transform);
			WeaponControl wc = weap.GetComponent<WeaponControl>();
			wc.type = t;
			wc.enabled = true;
			Debug.Log(t);
		}
	}
}
