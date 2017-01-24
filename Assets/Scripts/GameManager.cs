using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour {

    public Text m_messageText;

    public int m_minPlayers = 2;
    int m_maxPlayers = 4;

    [SyncVar]
    public int m_playerCount = 0;

    public static Color[] m_playerColors = { Color.red, Color.blue, Color.green, Color.magenta };

	static GameManager instance;

    public List<PlayerController> m_allPlayers;

    public List<Text> m_nameLabelText, m_playerScoreText;

	public static GameManager Instance{
		get{
			if(instance == null){
				instance = GameObject.FindObjectOfType<GameManager>();

				if(instance == null){
					instance = new GameObject().AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	void Awake(){
		if (instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}
	}

	void Start (){
		StartCoroutine("GameLoopRoutine");
	}

	IEnumerator GameLoopRoutine(){
		yield return StartCoroutine("EnterLobby");
        yield return StartCoroutine("PlayGame");
        yield return StartCoroutine("EndGame");
	}

	IEnumerator EnterLobby(){
        
        while(m_playerCount < m_minPlayers)
        {
            UpdateMessage("waiting for players");
            DisablePlayers();
		    yield return null;
        }
	}

	IEnumerator PlayGame(){
        yield return new WaitForSeconds(2f);
        UpdateMessage("3");
        yield return new WaitForSeconds(1f);
        UpdateMessage("2");
        yield return new WaitForSeconds(1f);
        UpdateMessage("1");
        yield return new WaitForSeconds(1f);
        UpdateMessage("Fight");
        yield return new WaitForSeconds(1f);
        EnablePlayers();
        UpdateScoreboard();
        UpdateMessage("");
        yield return new WaitForSeconds(10f);
	}
	
	IEnumerator EndGame(){
        UpdateMessage("Game Over");
		yield return null;
	}

	void SetPlayerState(bool state){
		PlayerController[] allPlayers = GameObject.FindObjectsOfType<PlayerController>();

		foreach (PlayerController p in allPlayers)
		{
			p.enabled = state;
		}
	}

	void EnablePlayers(){
		SetPlayerState(true);
	}

	void DisablePlayers(){
		SetPlayerState(false);
	}

    public void AddPlayer(PlayerSetup pSetup)
    {
        if(m_playerCount < m_maxPlayers)
        {
            m_allPlayers.Add(pSetup.GetComponent<PlayerController>());
            pSetup.m_playerColor = m_playerColors[m_playerCount];
            pSetup.m_playerNum = m_playerCount + 1;
        }
    }

    [ClientRpc]
    void RpcUpdateScoreboard(string[] playerNames, int[] playerScores)
    {
        for(int i=0; i < m_playerCount; i++)
        {
            if(playerNames[i] != null)
            {
                m_nameLabelText[i].text = playerNames[i];
            }
            if(playerScores[i] != null)
            {
                m_playerScoreText[i].text = playerScores[i].ToString();
            }
        }
    }

    public void UpdateScoreboard()
    {
        if (isServer)
        {
            string[] names = new string[m_playerCount];
            int[] scores = new int[m_playerCount];
            for (int i = 0; i < m_playerCount; i++)
            {
                names[i] = m_allPlayers[i].GetComponent<PlayerSetup>().m_playerNameText.text;
                scores[i] = m_allPlayers[i].m_score;
            }
            RpcUpdateScoreboard(names, scores);
        }
    }

    [ClientRpc]
    void RpcUpdateMessage(string msg)
    {
        if (m_messageText != null)
        {
            m_messageText.gameObject.SetActive(true);
            m_messageText.text = msg;
        }
    }

    public void UpdateMessage(string msg)
    {
        if (isServer)
        {
            RpcUpdateMessage(msg);
        }
    }
}
