using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameManager : NetworkBehaviour {

	static GameManager instance;

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
		StartCoroutine("EnterLobby");
		StartCoroutine("PlayGame");
		StartCoroutine("EndGame");
	}

	IEnumerator EnterLobby(){
		yield return null;
	}

	IEnumerator PlayGame(){
		yield return null;
	}
	
	IEnumerator EndGame(){
		yield return null;
	}

	void SetPlayerState(bool state){
		PlayerController[] allPlayers = GameObject.FindObjectOfType<PlayerController>();

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
}
