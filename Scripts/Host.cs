using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
public class Host : MonoBehaviourPunCallbacks
{
    System.Random rand = new System.Random();
    public RoundManager rm;
    public PhotonView photonV;
    public NetworkData nd;
    // Start is called before the first frame update
    void Awake()
    {
        SetScene();
	}

    void SetScene()
	{
        rm.myTeam = (RoundManager.MyTeam)rand.Next(0, 2);
        rm.teamRound = (RoundManager.TeamRound)rand.Next(0, 2);
        if ((int)rm.teamRound == (int)(rm.myTeam))
        {
            rm.ismyTurn = true;
            nd.SetKeySenders();
        }
        else rm.ismyTurn = false;

        Debug.Log(rm.ismyTurn);
        rm.NextRound();
        photonV.RPC("SetDataToClient", RpcTarget.Others, rm.teamRound, rm.activeWormNumber, rm.myTeam, !rm.ismyTurn);
        Debug.Log(rm.activeWormNumber + "Ac");
    }

	public void SetOnlineRound()
	{
        rm.SwitchTeam();
        
        if ((int)rm.teamRound == (int)(rm.myTeam))
        {
            rm.ismyTurn = true;
            nd.SetKeySenders();
            photonV.RequestOwnership();
        }
        else rm.ismyTurn = false; 
        rm.NextRound();
        Debug.Log(rm.ismyTurn);
        photonV.RPC("SetDataToClient", RpcTarget.Others, rm.teamRound, rm.activeWormNumber, rm.myTeam, !rm.ismyTurn);
        Debug.Log(rm.activeWormNumber + "Ac");
    }


}

