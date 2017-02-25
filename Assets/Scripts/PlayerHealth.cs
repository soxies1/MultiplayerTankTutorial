using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class PlayerHealth : NetworkBehaviour {
	[SyncVar(hook="UpdateHealthBar")]
	float m_currentHealth;
	public float m_maxHealth = 3;

	public GameObject m_deathPrefab;

	public bool m_isDead= false;

	public RectTransform m_healthBar;

    public PlayerManager m_lastAttacker;

	// Use this for initialization
	void Start () {
		Reset();
	}
	
	void UpdateHealthBar(float value){
		if(m_healthBar != null){
			m_healthBar.sizeDelta = new Vector2(value/m_maxHealth*150f, m_healthBar.sizeDelta.y);
		}
	}

	public void Damage(float damage, PlayerManager pc = null){
		if(!isServer){
			return;
		}

        if(pc != null && this.GetComponent<PlayerManager>() != pc)
        {
            m_lastAttacker = pc;
        }
		m_currentHealth -= damage;
		if(m_currentHealth <= 0 && !m_isDead){
            if(m_lastAttacker != null)
            {
                m_lastAttacker.m_score++;
                m_lastAttacker = null;
            }
            GameManager.Instance.UpdateScoreboard();
			m_isDead = true;
			RpcDie(); 
		}
	}
	[ClientRpc]
	void RpcDie(){
		 if(m_deathPrefab){
			 GameObject deathFX = Instantiate(m_deathPrefab, transform.position + Vector3.up * .05f, Quaternion.identity) as GameObject;
			 GameObject.Destroy(deathFX, 3.0f);
		 }
		 SetActiveState(false);
		 gameObject.SendMessage("Respawn");
	}

	void SetActiveState(bool state){
		foreach (Collider c in GetComponentsInChildren<Collider>())
		{
			c.enabled = state;
		}

		foreach (Canvas c in GetComponentsInChildren<Canvas>())
		{
			c.enabled = state;
		}

		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.enabled = state;
		}
	}

	public void Reset(){
		m_currentHealth = m_maxHealth;

		SetActiveState(true);

		m_isDead = false;
	}
}
