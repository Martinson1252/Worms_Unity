using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Room : MonoBehaviourPunCallbacks
{
	public RoomInfo roomInfo;

	public void JoinThisRoom()
	{
		PhotonNetwork.JoinRoom(roomInfo.Name);
	}
	
}
