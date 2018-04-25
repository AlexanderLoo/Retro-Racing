using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour {

	public SpriteRenderer[] background;
	public PlayerController playerController;
	public Text speedText;

	private BatteryDisplay bd;

	void Start(){

		bd = FindObjectOfType<BatteryDisplay> ();
	}
	void Update(){

		if (Input.GetKeyDown(KeyCode.Space)) {
			bd.AddLife ();
		}
	}

	public void DeleteMemory(){

		PlayerPrefs.DeleteAll ();
	}

	public void ActiveBackground(){

		foreach (SpriteRenderer item in background) {
			item.enabled = !item.enabled;
		}
	}

	public void ChangePlayerController(){

		playerController.m2 = !playerController.m2;
	}

	public void Addlife(){

		bd.AddLife ();
	}

	public void AlternateSpawn(){
		LevelManager.levelManager.alternateSpawn = !LevelManager.levelManager.alternateSpawn;
	}

	public void RandomSpawn(){
		LevelManager.levelManager.randomSpawn = !LevelManager.levelManager.randomSpawn;
	}

	public void AddSpeed(){

		GameController.gameController.speed -= 0.1f;
		speedText.text = GameController.gameController.speed.ToString ();
	}

	public void Restart(){
		SceneManager.LoadScene ("Main");
	}
}
