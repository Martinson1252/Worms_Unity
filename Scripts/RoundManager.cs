using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class RoundManager : MonoBehaviourPun
{
    public WormManager wm;
    public WormUI ui;
    public GameObject winScreen;
    public InputManager inp;
    public NetworkData netD;
    public Host _host;
    public PhotonView photonV;
    public int activeWormNumber;
    System.Random rand = new System.Random();
    public TeamRound teamRound;
    public MyTeam myTeam;
    public bool ismyTurn,amIHost;
    public enum MyTeam
	{
        red,blue
	}
    public enum TeamRound
    {
        red, blue
    }
  


    public void NextRound()
    {

        //SwitchTeam();
        if (ui.bottomIndicator.transform.childCount != 0) Destroy(ui.bottomIndicator.transform.GetChild(0).gameObject);

        //ui.Reset_Enable_RoundCounter();
        inp.InputEKeyEvent.RemoveAllListeners();
        inp.InputEKeyEvent.AddListener(ui.RoundKeys);
        inp.InputEKeyEvent.AddListener(ui.wi.SetWormInventory);
        if (!(wm.lastWorm == null))
        {
            GameObject lw = wm.lastWorm;
            lw.TryGetComponent<WormMovement>(out WormMovement v);
            v.RemoveAllMoveEvents();
            v.enabled = false;
            if (lw.transform.GetChild(lw.transform.childCount - 1).name.Contains("arrow"))
                Destroy(lw.transform.GetChild(lw.transform.childCount - 1).gameObject);

            if (lw.transform.GetChild(0).childCount != 0)
                Destroy(wm.lastWorm.transform.GetChild(0).GetChild(0).gameObject);
        }
        GameObject G = SelectWormRandomly(teamRound,wm);
        netD.activeWorm = G;
        Camera.main.GetComponent<CameraSC>().SelectedObject = G;
        Camera.main.GetComponent<CameraSC>().StartFollow();
        Camera.main.fieldOfView = .42f;
        Debug.Log(ismyTurn+" Round manager HOST");
        if(ismyTurn)
		{
        ui.SpawnArrow(teamRound);
        ui.SetArrow();
		}
        ui.SpawnUpperMessage(teamRound,ismyTurn);
        G.GetComponent<WormMovement>().enabled = true;


    }

    public void NextOnlineRound()
	{
        if (ui.bottomIndicator.transform.childCount != 0) Destroy(ui.bottomIndicator.transform.GetChild(0).gameObject);

        CheckWinner();
        //ui.Reset_Enable_RoundCounter();
        inp.InputEKeyEvent.RemoveAllListeners();
        inp.InputEKeyEvent.AddListener(ui.RoundKeys);
        inp.InputEKeyEvent.AddListener(ui.wi.SetWormInventory);
        if (!(wm.lastWorm == null))
        {
            GameObject lw = wm.lastWorm;
            lw.TryGetComponent<WormMovement>(out WormMovement v);
            v.RemoveAllMoveEvents();
            v.enabled = false;
            if (lw.transform.GetChild(lw.transform.childCount - 1).name.Contains("arrow"))
                Destroy(lw.transform.GetChild(lw.transform.childCount - 1).gameObject);

            if (lw.transform.GetChild(0).childCount != 0)
                Destroy(wm.lastWorm.transform.GetChild(0).GetChild(0).gameObject);
        }
        GameObject G = wm.activeWorm; //SelectWormRandomly(teamRound, wm);
        netD.activeWorm = G;
        wm.lastWorm = wm.activeWorm;
        Camera.main.GetComponent<CameraSC>().SelectedObject = G;
        Camera.main.GetComponent<CameraSC>().StartFollow();
        Camera.main.fieldOfView = .42f;
        Debug.Log(ismyTurn + " Round manager CLIENT");
        if (ismyTurn)
        {
            ui.SpawnArrow(teamRound);
            ui.SetArrow();
        }
            ui.SpawnUpperMessage(teamRound, ismyTurn);
        G.GetComponent<WormMovement>().enabled = true;
    }

    public GameObject SelectWormRandomly(TeamRound team, WormManager wm)
    {
        CheckWinner();
        switch (team)
        {
            case TeamRound.red:
                {
                    activeWormNumber = rand.Next(0, wm.redTeam.Count);
                    wm.SelectNewWorm(wm.redTeam[activeWormNumber].wormObj);
                    return wm.redTeam[activeWormNumber].wormObj;
                }
            case TeamRound.blue:
                {
                    activeWormNumber = rand.Next(0, wm.blueTeam.Count);
                    wm.SelectNewWorm(wm.blueTeam[activeWormNumber].wormObj);
                    return wm.blueTeam[activeWormNumber].wormObj;
                }
            default: return null;
        }
    }

    public void SwitchTeam()
    {
        switch (this.teamRound)
        {
            case TeamRound.blue:
                teamRound = TeamRound.red;
                break;
            case TeamRound.red:
                teamRound = TeamRound.blue;
                break;
        }
    }

    public void CheckWinner()
    {
        float RED = wm.redTeam.Count, BLUE = wm.blueTeam.Count;
        
        if (RED > 0 && BLUE == 0)
        {
            netD.IsGameEnded = true;
            Debug.Log("RED Won!");
            winScreen.SetActive(true);
            winScreen.transform.GetChild(0).GetComponent<TMP_Text>().text = "Red Team";
            Time.timeScale = 0;
            gameObject.GetComponent<RoundManager>().enabled = false;
            return;
        }
        else
             if (RED == 0 && BLUE > 0)
        {
            netD.IsGameEnded = true;
            Debug.Log("BLUE Won!");
            winScreen.SetActive(true);
            winScreen.transform.GetChild(0).GetComponent<TMP_Text>().text = "Blue Team";
            Time.timeScale = 0;
            gameObject.GetComponent<RoundManager>().enabled = false;
            return;
        }
        else
                 if (RED == 0 && BLUE == 0)
        {
            netD.IsGameEnded = true;
            Debug.Log("DRAW!");
            winScreen.SetActive(true);
            winScreen.transform.GetChild(0).GetComponent<TMP_Text>().text = "Nobody";
            Time.timeScale = 0;
            gameObject.GetComponent<RoundManager>().enabled = false;
            return;
        }

    }
}


