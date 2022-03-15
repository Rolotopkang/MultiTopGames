using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public GameObject RoomNameButtonPrefab;
    public Transform gridLayout;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //房间断联保护
        for (int i = 0; i < gridLayout.childCount; i++)
        {
            if (gridLayout.GetChild(i).gameObject.GetComponentInChildren<Text>().text == roomList[i].Name)
            {
                Destroy(gridLayout.GetChild(i).gameObject);
                if (roomList[i].PlayerCount == 0)
                {
                    roomList.Remove(roomList[i]);
                }
            }
        }
        foreach (var room in roomList)
        {
            GameObject newRoom = Instantiate(RoomNameButtonPrefab, gridLayout.position, Quaternion.identity);

            Text[] texts = newRoom.GetComponentsInChildren<Text>();
            
            texts[0].text = room.Name +"      "+room.PlayerCount+"/"+room.MaxPlayers;
            texts[1].text = room.Name;
            newRoom.transform.SetParent(gridLayout);
        }
    }
}
