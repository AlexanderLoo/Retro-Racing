using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testing : MonoBehaviour {

	public SpriteRenderer[] background;
	public PlayerController playerController;

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



}
