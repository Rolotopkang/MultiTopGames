using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Cinemachine;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEditor;
using WebSocketSharp;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject JoinButtons;
    public GameObject Camera;
    public GameObject DeathUI;
    public GameObject ChooseHeroUI;

    public static Dictionary<String,GameObject> inRoomPlayerDictionary = new Dictionary<String, GameObject>();


    private PhotonTeam[] teamList;
    
    private int teamID;
    private String heroName;
    private PhotonTeamsManager TeamsManager;
    private CinemachineVirtualCamera cmvc;

    public DissolveController MapDissolveController;
    [SerializeField]private GameObject[] teamRebornPoint;

    private void Awake()
    {
        cmvc = Camera.GetComponent<CinemachineVirtualCamera>();
        TeamsManager = GetComponent<PhotonTeamsManager>();
        teamList = TeamsManager.GetAvailableTeams();
    }

    public void JoinTeam(int _teamID)
    {
        JoinButtons.SetActive(false);
        teamID = _teamID;
        ChooseHeroUI.SetActive(true);
    }

    public void ChooseHero(String _hero)
    {
        heroName = _hero;
        GameObject player_temp=PhotonNetwork.Instantiate(heroName,
            teamRebornPoint[teamID].transform.position, Quaternion.identity, 0);
        //加入队伍
        player_temp.GetPhotonView().Owner.JoinTeam(teamList[teamID]);
        cmvc.Follow = player_temp.transform;
        //TODO
        ChooseHeroUI.SetActive(false);
        MapDissolveController.show();
        Debug.Log("manager_show");
    }

    public void Reborn()
    {
        DeathUI.SetActive(false);
        cmvc.Follow = PhotonNetwork.Instantiate(heroName,
            teamRebornPoint[teamID].transform.position,
            Quaternion.identity, 0).transform;
    }


    public static void AddPlayerList(Player player,GameObject playerActor)
    {
        inRoomPlayerDictionary.Add(player.NickName,playerActor);
    }
    
    public static void DelPlayerList(Player player,GameObject playerActor)
    {
        if (inRoomPlayerDictionary.ContainsKey(player.NickName))
        {
            inRoomPlayerDictionary.Remove(player.NickName);
        }
    }
    
}
