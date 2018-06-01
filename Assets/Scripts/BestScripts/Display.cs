using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Draw draw;

	public void MainMenu(int lives, int batteries){

		//Función para mostrar las vidas y baterías que nos quedan
		draw.Console();
		draw.Lives();
		draw.Battery();
	}

	public void ShowSplashScreen(){

		return;
	}
	public void StartingGame(){

		return;
	}

	public void PlayerMove(int index){

		foreach (SpriteRenderer player in draw.playerSprites) {
			player.enabled = false;
		}
		draw.playerSprites [index].enabled = true;
	}

	public void Enemies(List<SpriteRenderer> arrayOfEnemies){

		return;
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

	public void BatTimeFor(int time){

		return;
	}

	public void ShowExit(){

		return;
	}

	public void GameOver(){

		return;
	}
}
