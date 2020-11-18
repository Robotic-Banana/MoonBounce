using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerListController : MonoBehaviour
{
    public List<GameObject> playersList;

    void Awake()
    {
        foreach (var player in playersList)
        {
            player.transform.Find("PlayerName").GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }

    void Update()
    {
        UpdatePlayerList();
    }

    private void UpdatePlayerList()
    {
        var networkList = PhotonNetwork.PlayerList;

        if (networkList.Length > 0)
        {
            // for (var i = 0; i < networkList.Length; i++)
            // {
            //     Debug.LogFormat("Player: {0}", networkList[i].NickName);   
            // }
        }
    }
}
