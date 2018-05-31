using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public SpriteRenderer[] playerSprites;

	public void MainMenu(){

		//draw.console();
		//draw.lives(lives);
		//draw.battery(battery);
	}

	public void ShowSplashScreen(){

		return;
	}
	public void StartingGame(){

		return;
	}

	public void PlayerMove(int index){

		foreach (SpriteRenderer player in playerSprites) {
			player.enabled = false;
		}
		playerSprites [index].enabled = true;
	}

	public void NotEnoughtBat(){

		return;
	}

	public void CurrentScore (){

		return;
	}

	public void Crashed(){

		return;
	}

	public void ShowExit(){

		return;
	}

	public void GameOver(){

		return;
	}
}
