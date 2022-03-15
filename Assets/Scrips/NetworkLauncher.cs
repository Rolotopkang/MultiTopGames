using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    public GameObject nameUI;
    public GameObject loginUI;
    public GameObject connectingUI;
    public GameObject roomlistUI;

    public InputField playerName;
    public InputField roomName;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        connectingUI.SetActive(false);
        nameUI.SetActive(true);
        Debug.Log("connected successful");
        PhotonNetwork.JoinLobby();
    }

    public void PlayButton()
    {
        nameUI.SetActive(false);
        PhotonNetwork.NickName = playerName.text;
        loginUI.SetActive(true);
        Debug.Log("lobby?  "+PhotonNetwork.InLobby);
        if (PhotonNetwork.InLobby)
        {
            roomlistUI.SetActive(true);
        }
    }

    public void JoinOrCreateButton()
    {
        if(roomName.text.Length < 2)
            return;
        loginUI.SetActive(false);
        RoomOptions roomOptions = new RoomOptions{MaxPlayers = 6 };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, default);
        roomlistUI.SetActive(false);
        loginUI.SetActive(false);
        connectingUI.SetActive(true);
        Debug.Log("房间数"+PhotonNetwork.CountOfRooms);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("房间数"+PhotonNetwork.CountOfRooms);
        PhotonNetwork.LoadLevel(1);
    }
}
