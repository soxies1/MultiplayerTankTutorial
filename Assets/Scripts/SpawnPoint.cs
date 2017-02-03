using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	public bool m_isOccupied = false;

	void OnTriggerEnter (Collider other){
		if(other.gameObject.tag == "Player"){
			m_isOccupied = true;
		}
	}
	void OnTriggerStay(Collider other){
		if(other.gameObject.tag == "Player"){
			m_isOccupied = true;
		}
	}
	void OnTriggerExit(Collider other){
		if(other.gameObject.tag == "Player"){
			m_isOccupied = false;
		}
	}
}
