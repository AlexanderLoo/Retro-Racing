using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Draw draw;

	public void MainMenu(int batteries, int lives){

		//Función para mostrar las vidas y baterías que nos quedan
		draw.Console();
		draw.Lives();
		Battery(batteries);
	}

	public void ShowSplashScreen(){

		return;
	}
	public void StartingGame(){

		return;
	}

	public void PlayerMove(int index){

		draw.DisableAllSprites (draw.playerArray);	
		draw.playerArray [index].enabled = true;
	}

	public void Enemies(List<SpriteRenderer> arrayOfEnemies){

		return;
	}

	public void Battery(int batteries){

		draw.DisableAllImage (draw.batteryArray);
		for (int i = 0; i < batteries; i++) {
			draw.batteryArray [i].enabled = true;
		}
	}

	public void ShowRealTime(int hour, int minute, string amOrPm){

		if (hour > 12) {
			hour -= 12;
			amOrPm = "PM";
		}
		if (hour < 10) {
			draw.hour.text = "0" + hour.ToString ();
		} else {
			draw.hour.text = hour.ToString();
		}
		if (minute < 10) {
			draw.minute.text = "0" + minute.ToString ();
		} else {
			draw.minute.text = minute.ToString ();
		}
		draw.amOrPm.text = amOrPm;
	}

	public void NotEnoughtBat(){

		return;
	}

	public void CurrentScore (float score){

		draw.scoreText.text = score.ToString();
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
