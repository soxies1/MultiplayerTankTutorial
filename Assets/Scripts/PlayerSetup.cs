using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerSetup : NetworkBehaviour
{

    [SyncVar(hook="UpdateColor")]
	public Color m_playerColor;

    [SyncVar(hook ="UpdateName")]
	public string m_name = "PLAYER";
    
	public Text m_playerNameText;

	public override void OnStartClient(){
		base.OnStartClient();
        if (!isServer)
        {
            PlayerManager pManager = GetComponent<PlayerManager>();

            if (pManager != null)
            {
                GameManager.m_allPlayers.Add(pManager);
            }
        }

        UpdateName(m_name);
        UpdateColor(m_playerColor);
    }

    void UpdateColor(Color pColor)
    {
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer r in meshes){
			r.material.color = pColor;
		}
    }

    void UpdateName(string pName)
    {
        if(m_playerNameText != null){
			m_playerNameText.enabled = true;
            m_playerNameText.text = pName; 
		}
    }
}
