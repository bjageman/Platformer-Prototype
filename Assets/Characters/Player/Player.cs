using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

    [Header("Movement")]
	[SerializeField] float runSpeed = 10f;
	[SerializeField] float jumpSpeed = 10f;

    [Header("Shooting")]
    [SerializeField] float maxAim = 5f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float recoilSpeed = 5f;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float rateOfFire = .1f;
    [SerializeField] float ammoRegenInS = 1f;
    [SerializeField] int maxAmmo = 20;
    [SerializeField] Text ammoUI;


    [Header("Health")]
    [SerializeField] Text healthUI;
    [SerializeField] int health = 3;
	[SerializeField] float damageFlashTime = .1f;
    [SerializeField] float damageRecoveryTime = 10;
    [SerializeField] float blinkSpeed = 1f;

	const string isRunning = "isRunning";
	const string isJumping = "isJumping";
    const string isFalling = "isFalling";
    const string GROUND_LAYER = "Ground";

    float ammo;
    float lastTimeDamaged;
    float lastTimeShotFired;
    bool isInvincible = false;
	Rigidbody2D rigidBody;
	Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    SpriteRenderer spriteRenderer;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        lastTimeShotFired = 0;
        lastTimeDamaged = -damageRecoveryTime; //prevents starting as invulnerable
        ammo = maxAmmo;
        ammoUI.text = ammo.ToString();
        healthUI.text = health.ToString();
        StartCoroutine(RegenerateAmmo());
	}
	
	// Update is called once per frame
	void Update ()
    {
        Run();
        Jump();
        if (Input.GetMouseButton(0)){
            Shoot();
        }
        isInvincible = Time.time - lastTimeDamaged < damageRecoveryTime;
    }

    private IEnumerator RegenerateAmmo()
    {
        while (true){
            if (IsTouchingGround() && ammo < maxAmmo){
                ModifyAmmoAmount(1);
            }
            yield return new WaitForSeconds(ammoRegenInS);
        }
    }

    private void Run()
    {
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(h * runSpeed, rigidBody.velocity.y);
        if (Mathf.Abs(h) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(h), 1f);
            animator.SetBool(isRunning, true);
            
        }
        else
        {
            animator.SetBool(isRunning, false);
        }
    }

	private void Jump()
    {
        if (!IsTouchingGround()) { return; }
        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            rigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
        if (rigidBody.velocity.y > 0)
        {
            animator.SetBool(isJumping, true);
        }
        else if (rigidBody.velocity.y < 0)
        {
            animator.SetBool(isFalling, true);
            animator.SetBool(isJumping, false);
        }
        else
        {
            animator.SetBool(isFalling, false);
            animator.SetBool(isJumping, false);
        }
    }

    private bool IsTouchingGround()
    {
        return feetCollider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER));
    }

    private void Shoot(){
        if (Time.time - lastTimeShotFired > rateOfFire && ammo > 0)
        {
            lastTimeShotFired = Time.time;
            ModifyAmmoAmount(-1);
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 unitVectortoMousePosition = (worldPoint - transform.position).normalized;
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.parent = gameObject;
        projectile.GetComponent<Rigidbody2D>().velocity = unitVectortoMousePosition * projectileSpeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = -unitVectortoMousePosition * recoilSpeed;
        Destroy(projectile, 5f);
    }

    private void ModifyAmmoAmount(int amount)
    {
        ammo = ammo + amount;
        ammoUI.text = ammo.ToString();
    }

    public void TakeDamage(){
		if (!isInvincible){
            lastTimeDamaged = Time.time;
            StartCoroutine(HandleDamage());
        }
	}

    private IEnumerator HandleDamage()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
		health--;
        healthUI.text = health.ToString();
        if (health <= 0)
        {
            Die();
        }
		yield return new WaitForSecondsRealtime(damageFlashTime);
		spriteRenderer.color = Color.white;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        while (isInvincible){
            spriteRenderer.enabled = false;
            yield return new WaitForSecondsRealtime(blinkSpeed / 4);
            spriteRenderer.enabled = true;
            yield return new WaitForSecondsRealtime(blinkSpeed);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.black;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 worldPoint2d = new Vector2(worldPoint.x, worldPoint.y);
        Gizmos.DrawLine(transform.position, worldPoint2d);
    }
}
