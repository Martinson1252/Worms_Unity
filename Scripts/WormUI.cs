using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;
using System;

public class WormUI : MonoBehaviourPunCallbacks
{
    Action Counter;
    public  GameObject weaponUI, DragIcon, round_counter, 
        bottomIndicator, ammunitionIconRifle,ammunitionIconShotgun, arrow,weapGrid, upperMessage;
    [SerializeField] public  Animator uiAnim;
   [SerializeField] PointerEventData ev;
   [SerializeField] InputManager inp;
    public TextMeshProUGUI WeapText, clock, redAmount, blueAmount, roundCount, ammoCount;
    public WormManager wm;
    public WormInventory wi;
    public PhotonView pv;
    public Slider redTeamBar, blueTeamBar;
    public string weaponName;
    public Color[] color;
    float minute, second, teamHealth, wormHealth, roundTime;
    public enum WeaponType
	{
        Rifle,Shotgun
	}
    public WeaponType weaponType;
    private void Start()
    {
        redAmount.text = wm.redTeam.Count.ToString();
        blueAmount.text = wm.blueTeam.Count.ToString();
        wormHealth = wm.health;
        teamHealth = wormHealth * wm.wormName.Count / 2;
        redTeamBar.maxValue = teamHealth;
        blueTeamBar.maxValue = teamHealth;
       
    }
	private void FixedUpdate()
	{
        Counter?.Invoke();
        Clock();
	}

	public void SetBar(float value,Worm.Team t)
	{
        switch(t)
		{
            case Worm.Team.red:
				{
                    redTeamBar.value += value;
                    break;
				}
            case Worm.Team.blue:
				{
                    blueTeamBar.value += value;
                    break;
				}
		}
	}

    public void CreateAmmoBar( float rounds, WeaponType weaponType)
	{
        switch(weaponType)
		{
            case WeaponType.Rifle:
       GameObject a = Instantiate(ammunitionIconRifle, bottomIndicator.transform);
        ammoCount = a.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                break;
            case WeaponType.Shotgun:
       GameObject b = Instantiate(ammunitionIconShotgun, bottomIndicator.transform);
        ammoCount = b.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                break;
		}
        ammoCount.text = rounds.ToString();
    }

    public void SetAmmoBar(float value, ref float rounds)
	{
        rounds+=value;
        ammoCount.text = rounds.ToString();
	}

    public void DecreaseTeamCount(float value, Worm.Team team)
	{
        switch(team)
		{
            case Worm.Team.red:
                redAmount.text = (float.Parse( redAmount.text ) - value).ToString();
                break;
            case Worm.Team.blue:
                blueAmount.text = (float.Parse( blueAmount.text ) - value).ToString();
                break;
		}
	}

	public void PanelShow()
	{
        
        if (weaponUI.activeSelf)
        {
            uiAnim.SetTrigger("right");
            StartCoroutine(DisableFor(weaponUI,.58f));
           
        }
        else
		{
            weaponUI.SetActive(true);
            uiAnim.SetTrigger("left");
		}
	}

    public void DragIconShow(bool isActive)
	{
        if(isActive)
		{
            DragIcon.SetActive(true);
            DragIcon.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(DisableFor(DragIcon,2));
           
        }else
		{
            DragIcon.SetActive(true);
            DragIcon.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(DisableFor(DragIcon, 2));
          
        }
	}

    public void DeleteArrow()
	{
        inp.InputJumpKeyEvent.RemoveListener(DeleteArrow);
        inp.InputAKeyEvent.RemoveListener(DeleteArrow);
        inp.InputDKeyEvent.RemoveListener(DeleteArrow);
        inp.InputEKeyEvent.RemoveListener(DeleteArrow);
        Destroy( wm.activeWorm.transform.GetChild(2).gameObject);
    }

    public void SetArrow()
	{
        inp.InputJumpKeyEvent.AddListener(DeleteArrow);
        inp.InputAKeyEvent.AddListener(DeleteArrow);
        inp.InputDKeyEvent.AddListener(DeleteArrow);
        inp.InputEKeyEvent.AddListener(DeleteArrow);

    }

   public void SpawnArrow(RoundManager.TeamRound t)
	{
        switch (t)
		{
            case RoundManager.TeamRound.blue:
               GameObject a = Instantiate(arrow,wm.activeWorm.transform);
                a.GetComponent<Image>().color = color[0];
                break;
            case RoundManager.TeamRound.red:
               GameObject ar = Instantiate(arrow, wm.activeWorm.transform);
                ar.GetComponent<Image>().color = color[1];
                break;
		}
	}

    public void SpawnUpperMessage(RoundManager.TeamRound t, bool isMyRound)
	{
        GameObject a = Instantiate(upperMessage);
        if (isMyRound) a.transform.GetChild(0).GetComponent<Text>().text = "Your turn!";
        else a.transform.GetChild(0).GetComponent<Text>().text = "Your enemy turn!";
        Debug.Log("Message");
        switch (t)
        {
            case RoundManager.TeamRound.blue:

                a.transform.GetChild(0).GetComponent<Text>().color = color[0];
                break;
            case RoundManager.TeamRound.red:
                a.transform.GetChild(0).GetComponent<Text>().color = color[1];
                break;
        }
        
    }

    public IEnumerator DisableFor(GameObject G,float time)
    {
        yield return new WaitForSeconds(time);
        G.SetActive(false);
    }

    private void OnGUI()
	{
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, result);
        foreach(RaycastResult ray in result)
			{

                    WeapText.text = ray.gameObject.name;
                weaponName = ray.gameObject.name;
		
			}
        }
	}

    //Uruchamia siê po wybraniu broni w panelu i wykonuje wszystkie pozosta³e akcje
    public void SetWeapon()
	{

        wm.GetComponent<WormInventory>().AddWeapon(weaponName,wm.activeWorm);
        uiAnim.SetTrigger("right");
        StartCoroutine(DisableFor(weaponUI, .58f));
        pv.RPC("SetWeaponOnNetwork", RpcTarget.Others,weaponName);
        
	}

   

    public void RoundKeys()
    {
            PanelShow();
    }

    public void Clock()
	{
        if (second >= 60)
        {
            minute++;
            second = 0;
        }
        else
            second += Time.fixedDeltaTime;
        clock.text = minute.ToString() +":"+ second.ToString("00");
	}
   
    public void RoundCounter()
	{
        if(roundTime<=0) wm.rm._host.SetOnlineRound();
		else
		{
            roundTime -= Time.fixedDeltaTime;
            roundCount.text = Mathf.Floor( roundTime ).ToString();
		}
	}
    public void Reset_Enable_RoundCounter()
	{
        roundTime = 40;
        round_counter.SetActive(true);
        Counter = null;
        Counter += RoundCounter;
	}

    public void Disable_RoundCounter()
	{
        round_counter.SetActive(false);
        Counter -= RoundCounter;
	}
}
