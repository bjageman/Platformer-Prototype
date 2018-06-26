using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.GetComponent<Player>()){
			other.transform.parent = transform;
		}
	}
	
	void OnTriggerExit2D(Collider2D other){
		if (other.GetComponent<Player>()){
			other.transform.parent = null;
		}
	}
}
