﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;
public class NetworkLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
        LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();

        PlayerSetup pSetup = gamePlayer.GetComponent<PlayerSetup>();

        pSetup.m_name = lPlayer.playerName;
        pSetup.m_playerColor = lPlayer.playerColor;

        PlayerManager pManager = gamePlayer.GetComponent<PlayerManager>();
        if (pManager != null)
        {
            GameManager.m_allPlayers.Add(pManager);
        }
    }
}
