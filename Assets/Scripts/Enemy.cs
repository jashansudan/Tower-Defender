using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[HideInInspector]
	public float speed;

	public float startSpeed = 10f;
	public float health = 100;

	public int worth = 50;

	public GameObject deathEffect;

	private Transform target;
	private int wavepointIndex = 0;

	void Start () {
		speed = startSpeed;
	}

	public void TakeDamage (float damage){
		health -= damage;

		if (health <= 0) {
			Die ();
		}
	}

	public void Slow (float slowAmount){
		speed = startSpeed * (1f - slowAmount);
	}

	void Die (){
		PlayerStats.Money += worth;

		GameObject effect = (GameObject) Instantiate (deathEffect, transform.position, Quaternion.identity);
		Destroy (effect, 5f);
		Destroy (gameObject);
	}
}
