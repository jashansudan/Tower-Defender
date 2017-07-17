using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static bool isGameEnded;

	public GameObject gameOverUI;

	void Start (){
		isGameEnded = false;
	}

	// Update is called once per frame
	void Update () {
		if (isGameEnded) {
			return;
		}

		if (PlayerStats.Lives <= 0) {
			EndGame ();
		}
	}

	void EndGame (){
		isGameEnded = true;
		gameOverUI.SetActive (true);	
	}
}
