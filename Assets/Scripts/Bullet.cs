using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public GameObject impactEffect;

	public float speed = 70f;
	public float explosionRadius = 0;

	public int damage = 50;

	public void Seek (Transform _target){
		target = _target;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (target == null) {
			Destroy (gameObject);
			return;
		}

		Vector3 direction = target.position - transform.position;
		float distanceMovedThisFrame = speed * Time.deltaTime;

		if (direction.magnitude <= distanceMovedThisFrame) {
			HitTarget ();
			return;
		}

		transform.Translate (direction.normalized * distanceMovedThisFrame, Space.World);
	
		transform.LookAt (target);
	}


	void HitTarget() {
		GameObject effectIns = (GameObject)Instantiate (impactEffect, transform.position, transform.rotation);
		Destroy (effectIns, 2);

		if (explosionRadius > 0f) {
			explode ();
		} else {
			Damage (target);
		}

		Destroy (gameObject);
	}

	void explode(){
		Collider[] colliders = Physics.OverlapSphere (transform.position, explosionRadius);
		foreach (Collider collider in colliders) {
			if (collider.tag == "Enemy") {
				Damage (collider.transform);
			}
		}
	}

	void Damage (Transform enemy){
		Enemy e = enemy.GetComponent<Enemy> ();

		if (e != null) {
			e.TakeDamage (damage);
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, explosionRadius);
	}
}
