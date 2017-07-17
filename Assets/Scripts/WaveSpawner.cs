using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public Transform enemyPrefab;

	public Transform spawnLocation;

	public float timeBetweenWaves = 5.5f;
	private float countdown = 5.5f;

	public Text waveCountdownText;

	private int waveNumber = 1;

	void Update ()
	{
		if (countdown <= 0f) 
		{
			StartCoroutine (SpawnWave ());
			countdown = timeBetweenWaves;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp (countdown, 0f, Mathf.Infinity);

		waveCountdownText.text = string.Format("{0:00.00}", countdown);
	}

	IEnumerator SpawnWave ()
	{
		PlayerStats.Waves++;
		for (int i = 0; i < waveNumber; i++) 
		{
			SpawnEnemy ();
			yield return new WaitForSeconds(0.5f);
		}
		waveNumber++;
	}

	void SpawnEnemy()
	{	
		Instantiate (enemyPrefab, spawnLocation.position, spawnLocation.rotation);
	}
}
