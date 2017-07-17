using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	private Transform target;
	private Enemy targetEnemy;

	[Header("General")]
	public float range = 15f;

	[Header("Use Bullets (default)")]
	public float fireRate = 1f;
	public GameObject bulletPrefab;
	private float fireCountdown = 0f;

	[Header("Use Laser")]
	public bool useLaser = false;

	public int damageOverTime = 30;
	public float slowAmount = 0.5f;

	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	public Light impactLight;

	[Header("Setup Fields")]
	public string enemeyTag = "Enemy";

	public Transform pivot;
	public float rotationSpeed = 10f;

	public Transform firePoint;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
	}

	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag (enemeyTag);
		float shortestEnemyDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies) 
		{
			float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestEnemyDistance) 
			{
				shortestEnemyDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestEnemyDistance <= range) {
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<Enemy>();
		} else {
			target = null;
		}
	}

	// Update is called once per frame
	void Update () {
		if (target == null) { 
			if (useLaser) {
				if (lineRenderer.enabled) {
					lineRenderer.enabled = false;
					impactEffect.Stop ();
					impactLight.enabled = false;
				}
			}
			return;
		}
		LockOnTarget ();

		if (useLaser) {
			Laser ();
		} else
		{
			if (fireCountdown <= 0f) {
				Shoot ();
				fireCountdown = 1f / fireRate;
			}
			fireCountdown -= Time.deltaTime;
		}
	}

	void Laser() {
		targetEnemy.TakeDamage (damageOverTime * Time.deltaTime);
		targetEnemy.Slow (slowAmount);

		if (!lineRenderer.enabled) {
			lineRenderer.enabled = true;
			impactEffect.Play ();
			impactLight.enabled = true;
		}

		lineRenderer.SetPosition (0, firePoint.position);
		lineRenderer.SetPosition (1, target.position);

		Vector3 dir = firePoint.position - target.position;

		impactEffect.transform.position = target.position + dir.normalized;
		impactEffect.transform.rotation = Quaternion.LookRotation (dir);

	}

	void LockOnTarget() {
		Vector3 enemyDirection = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation (enemyDirection);
		Vector3 rotation = Quaternion.Lerp(pivot.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
		pivot.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	void Shoot() {
		GameObject bulletGO =  (GameObject)Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet> ();

		if (bullet != null)
			bullet.Seek (target);
	}


	void OnDrawGizmosSelected () 
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, range);
	}
}
