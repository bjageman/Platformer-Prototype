using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO make separate component
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
public class DamageSystem : MonoBehaviour {
	[SerializeField] int health = 3;
	[SerializeField] float damageFlashTime = .1f;
    [SerializeField] AudioClip hitSound;

	public void TakeDamage(){
		StartCoroutine(HandleDamage());
	}

	protected IEnumerator HandleDamage()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(hitSound);
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.color = Color.red;
		health--;
        if (health <= 0)
        {
            Die();
        }
		yield return new WaitForSecondsRealtime(damageFlashTime);
		renderer.color = Color.white;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
