using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
public class NetworkData : MonoBehaviourPunCallbacks,IPunObservable
{
    public GameObject hostG,activeWorm,Message;
    public RoundManager rm;
    public PhotonView pv;
    public WormManager wm;
    public bool IsGameEnded;
    
	private void Awake()
	{
        if (PhotonNetwork.IsMasterClient)
        {
            hostG.SetActive(true);
            rm.amIHost = true;
            //pv.ObservedComponents.Add(hostG.GetComponent<Host>());
        }
        else
            rm.amIHost = false;
    }
	void Start()
    {
        PhotonNetwork.SendRate = 50;
        PhotonNetwork.SerializationRate = 40;
        Debug.Log("InRoom: "+PhotonNetwork.InRoom);
    }

    [PunRPC]
    public void SpacePressOnlineShootInfo(float chargePanels, float shootPower)
    {
        activeWorm.transform.GetChild(0).GetChild(0).GetComponent<WeaponControl>().panelCharged = chargePanels;

        activeWorm.transform.GetChild(0).GetChild(0).GetComponent<WeaponControl>().shootPower = shootPower;
    }

    [PunRPC]
    public void SetDataToClient(RoundManager.TeamRound teamRound,int activeWormNumber,RoundManager.MyTeam hostTeam,bool isMyTurn)
    {
        RoundManager r = FindObjectOfType<RoundManager>();
        r.teamRound = teamRound;
        r.ismyTurn = isMyTurn;
        Debug.Log(r.ismyTurn);
        r.wm.activeWorm = r.wm.GetFromActiveTeamByNumber(activeWormNumber, r.teamRound);
        if (hostTeam.Equals(RoundManager.MyTeam.red))
        {
            r.myTeam = (RoundManager.MyTeam.blue);
        }
        else 
        {
            r.myTeam = (RoundManager.MyTeam.red); 
        }
        if (rm.ismyTurn)
        {
            pv.RequestOwnership();
            SetKeySenders();
        }

        Debug.Log(r.ismyTurn+" turn"+" Received data from Host.");
        r.NextOnlineRound();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
    {
       
        if (stream.IsWriting )
		{
            stream.SendNext(activeWorm.transform.position);
            //stream.SendNext(key);
		}

        if(stream.IsReading )
		{
            activeWorm.transform.position =  (Vector3)stream.ReceiveNext();
            //rm.inp.KeySimulateClient((string)stream.ReceiveNext());
		}

    }

    [PunRPC]
    public void SetWeaponOnNetwork(string weaponName)
    {
        RoundManager rm = FindObjectOfType<RoundManager>();
        if (rm.ismyTurn) return;
        FindObjectOfType<WormInventory>().AddWeapon(weaponName, rm.wm.activeWorm);
        //WormUI ui = FindObjectOfType<WormUI>(); 
        
        //StartCoroutine(ui.DisableFor(ui.weaponUI, .58f));
    }


    public void SendJumpKey()
	{
        pv.RPC("SendMyActions", RpcTarget.Others, "W");
	}
    public void SendMoveRight()
	{
        pv.RPC("SendMyActions",RpcTarget.Others,"D");
    }

    public void SendMoveRight_UP()
	{
        pv.RPC("SendMyActions", RpcTarget.Others, "D_up");
	}

    public void SendLeft()
	{
        pv.RPC("SendMyActions", RpcTarget.Others, "A");
	}

    public void SendLeft_UP()
	{
        pv.RPC("SendMyActions", RpcTarget.Others, "A_up");
	}

    public void SendKeySpace()
	{
        //Debug.Log("Space Pressed");
        pv.RPC("SendMyActions", RpcTarget.Others, "Space");
    } 
    public void SendKeySpaceUP()
	{
        //Debug.Log("Space UP");
        pv.RPC("SendMyActions", RpcTarget.Others, "Space_up");
    } 
    public void SendKeySpaceDown()
	{
        //Debug.Log("Space UP");
        pv.RPC("SendMyActions", RpcTarget.Others, "Space_down");
    } 


    public void SendKey_E_()
	{
        pv.RPC("SendMyActions", RpcTarget.Others, "E");
    }

    public void SendArrowUP()
    {
        pv.RPC("SendMyActions", RpcTarget.Others, "ArrowUP");
    }

    public void SendArrowUP_down()
    {
        pv.RPC("SendMyActions", RpcTarget.Others, "ArrowUP_down");
    }
    public void SendArrowDOWN()
    {
        pv.RPC("SendMyActions", RpcTarget.Others, "ArrowDOWN");
    }
    public void SendArrowDOWN_down()
    {
        pv.RPC("SendMyActions", RpcTarget.Others, "ArrowDOWN_down");
    }

    
    [PunRPC]
    public void SendMyActions(string key)
	{
        rm.inp.KeySimulateClient(key);
	}
 
  
    public void SetKeySenders()
	{
        Debug.Log("IS MY TURN START " + rm.ismyTurn);
        InputManager inp = FindObjectOfType<InputManager>();
        //usuwanie listenerów z poprzedniej rundy, ¿eby zapobiec wielokrotnemu wywo³aniu
        inp.InputDKeyEvent.RemoveAllListeners();
        inp.InputDKeyUPEvent.RemoveAllListeners();
        inp.InputEKeyEvent.RemoveAllListeners();
        inp.InputAKeyEvent.RemoveAllListeners();
        inp.InputAKeyUPEvent.RemoveAllListeners();
        inp.InputArrowUPkey.RemoveAllListeners();
        inp.InputArrowUPkeyDown.RemoveAllListeners();
        inp.InputArrowDOWNkey.RemoveAllListeners();
        inp.InputArrowDOWNkeyDown.RemoveAllListeners();
        inp.InputJumpKeyEvent.RemoveAllListeners();
        inp.InputSpaceKeyUPEvent.RemoveAllListeners();
        inp.InputSpacePressedKeyEvent.RemoveAllListeners();
        inp.InputSpaceKeyDownEvent.RemoveAllListeners();
        //dodawanie listenerów
        inp.InputDKeyEvent.AddListener(SendMoveRight);
        inp.InputDKeyUPEvent.AddListener(SendMoveRight_UP);
        inp.InputEKeyEvent.AddListener(SendKey_E_);
        inp.InputAKeyEvent.AddListener(SendLeft);
        inp.InputAKeyUPEvent.AddListener(SendLeft_UP);
        inp.InputArrowUPkey.AddListener(SendArrowUP);
        inp.InputArrowUPkeyDown.AddListener(SendArrowUP_down);
        inp.InputArrowDOWNkey.AddListener(SendArrowDOWN);
        inp.InputArrowDOWNkeyDown.AddListener(SendArrowDOWN_down);
        inp.InputJumpKeyEvent.AddListener(SendJumpKey);
        inp.InputSpaceKeyUPEvent.AddListener(SendKeySpaceUP);
        inp.InputSpacePressedKeyEvent.AddListener(SendKeySpace);
        inp.InputSpaceKeyDownEvent.AddListener(SendKeySpaceDown);
    }

    public void ExitToMenu()
	{
        StartCoroutine(DisconnectAndExit());
        Debug.Log(PhotonNetwork.IsMasterClient+" isMaster");
	}

    public IEnumerator DisconnectAndExit()
	{
        Debug.Log("DISC");
        PhotonNetwork.Disconnect();
        //PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.IsConnected)
		{
            Debug.Log("CONNECTED");
        yield return null;

		}
        Debug.Log(PhotonNetwork.IsConnectedAndReady+"isConnected");
        SceneManager.LoadScene(0);

    }

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
        if (IsGameEnded) return;
		base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("Player left");
        ExitToMenu();
        DontDestroyOnLoad(Instantiate(Message));
	}

  
	
}
