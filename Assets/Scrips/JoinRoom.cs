using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    public Text roomID;
    
    public void JoinThisRoom()
    {
        RoomOptions roomOptions = new RoomOptions{MaxPlayers = 6 };
        PhotonNetwork.JoinOrCreateRoom(roomID.text, roomOptions, default);
    }
}
