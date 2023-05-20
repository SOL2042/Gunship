using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class NetworkManager_Room : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button EnterRoomButton;


    private void Awake()
    {
        
        EnterRoomButton.onClick.AddListener(() =>
        {
            EventManager.instance.PostEvent("OpenInGameScene", null);
        });
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료");
    }

}
