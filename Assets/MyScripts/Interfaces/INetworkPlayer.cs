using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface INetworkPlayer
{
    bool IsMine();
    GameObject GetLocalPlayer();

    int GetPlayerNumber();

    string GetNickName();

    void OnPlayerEnteredRoom();

    void OnPlayerLeftRoom();
}
