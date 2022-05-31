using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkRooms : MonoBehaviourPunCallbacks
{
	public delegate void delMet();
	public delMet toDo;
	public GameObject roomBarPrefab, rightPanel, createRoomPanel, roomsContent, connectingScreen, waitingScreen;
	public Button createRoom_Button, cancelCreateRoom_Button;
	public Text roomNameField;
	List<RoomInfo> room = new List<RoomInfo>();
	
	public void Start()
	{
		Connect();
		PhotonNetwork.AutomaticallySyncScene = true;
	}

	public void Connect()
	{
		Debug.Log("Connecting to server...");
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Succesfully connected to server.");
		waitingScreen.SetActive(false);
		if(!PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
		base.OnConnectedToMaster();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("Failed to join.");
		base.OnJoinRandomFailed(returnCode, message);
	}


	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		room = roomList;
		foreach (Transform g in roomsContent.transform)
			Destroy(g.gameObject);

		
		for(int i=0; i< roomList.Count;i++)
		{
			if (roomList[i].PlayerCount!=0) 
			{
				
			GameObject roomBar = Instantiate(roomBarPrefab, roomsContent.transform);
			roomBar.transform.Find("Name").GetComponent<Text>().text = roomList[i].Name.ToString();
			roomBar.transform.Find("PlayerAmount").GetComponent<Text>().text = roomList[i].PlayerCount.ToString();
			Room r = roomBar.GetComponent<Room>();
				r.roomInfo = roomList[i];
				if (roomList[i].PlayerCount == roomList[i].MaxPlayers)
					r.transform.GetComponentInChildren<Button>().interactable = false;
			}
			else
			{
				room.Remove(room[i]);
			}
		}
		roomList = room;
		Debug.Log("Roooms count: "+room.Count);
		//base.OnRoomListUpdate(roomList);
	}


	public void CreateNewRoom()
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		CloseCreateRoomPanel();
		connectingScreen.SetActive(true);
		PhotonNetwork.CreateRoom(roomNameField.text, roomOptions);
		roomNameField.text = "";
	}
	public void OpenCreateRoomPanel()
	{
		createRoomPanel.SetActive(true);
	}

	public void CloseCreateRoomPanel()
	{
		createRoomPanel.SetActive(false);
	}

	public void DisconnectFromRoom()
	{
		connectingScreen.SetActive(false);
		PhotonNetwork.LeaveRoom(true);
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("Players in room: "+PhotonNetwork.CurrentRoom.PlayerCount);
		if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
		{
			PhotonNetwork.LoadLevel(1);
			StartCoroutine(Round());
		IEnumerator Round()
		{
			Time.timeScale = 0;
			while(SceneManager.GetSceneAt(1).isLoaded)
			{
				yield return null;
			}
			Time.timeScale = 1;
		}
		}
		
		base.OnJoinedRoom();
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
		if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
		{

			PhotonNetwork.LoadLevel(1);
			StartCoroutine(Round());
			IEnumerator Round()
			{
				Time.timeScale = 0;
				while (SceneManager.GetSceneAt(1).isLoaded)
				{
					yield return null;
				}
				Time.timeScale = 1;
			}
		}
		base.OnPlayerEnteredRoom(newPlayer);
	}

	public override void OnLeftRoom()
	{
		room.Clear();
		
		base.OnLeftRoom();
	}

	public void DoesRoomExist(Transform bar)
	{
		string name = bar.transform.GetChild(0).GetComponent<InputField>().text;
		Button button = bar.Find("Create").GetComponent<Button>();
		foreach(RoomInfo r in room)
		{
			if(r.Name==name)
			{
				button.interactable = false;
				return;
			}
		}
		button.interactable = true;

	}

	public void ExitApplication()
	{
		StartCoroutine(WaitTilldisconnect());
	}

	IEnumerator WaitTilldisconnect()
	{
		PhotonNetwork.Disconnect();
		while(PhotonNetwork.IsConnected)
		yield return null;
		Application.Quit();
	}
}

