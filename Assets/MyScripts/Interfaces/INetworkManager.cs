using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface INetworkManager 
{
    bool IsConnected();

    bool IsMasterClient();

    void OnLeftRoom();

    void LeaveRoom();

    void Replay();

    void LoadArena();

    string GetNetworkClientState();

    void AutomaticallySyncScene(bool sync);

    List<object> GetPlayerList();

}
