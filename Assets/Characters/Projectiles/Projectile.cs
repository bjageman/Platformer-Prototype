using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public GameObject parent;

	void OnTriggerEnter2D(Collider2D collider){
		if (!GameObject.ReferenceEquals(collider.gameObject, parent)){
			var damageSystem = collider.GetComponent<DamageSystem>();
			if (damageSystem){
				damageSystem.TakeDamage();
			}
			Destroy(gameObject, .01f);
		}
	}
}
