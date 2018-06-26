using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {	

	void OnCollisionEnter2D(Collision2D col)
    {
		print(col.gameObject.name);
		var player = col.gameObject.GetComponent<Player>();
        if (player){
			player.TakeDamage();
		}
    }

	void OnTriggerEnter2D(Collider2D other){
		var player = other.gameObject.GetComponent<Player>();
        if (player){
			player.TakeDamage();
		}
	}
    
}
