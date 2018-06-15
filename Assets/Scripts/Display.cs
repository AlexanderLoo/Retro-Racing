using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Draw draw;

	public class Screen
	{
		public int screenWith;
		public int screenHeight;

		public Screen(int screenWith, int screenHeight){

			this.screenWith = screenWith;
			this.screenHeight = screenHeight;
		}
	}

	public void GetCurrentScreen(){

		Screen newScreen = new Screen (draw.GetScreenWidth(), draw.GetScreenHeight());
		//temporal
		print(newScreen.screenWith);
		print (newScreen.screenHeight);
		//mandar todo las proporciones, pantalla, etc.
	}

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

	public void Enemies(string[] enemiesArray){  //<--- paso como argumentos el arreglo de strings en forma binaria "000","101",....
		
		for (int i = 0; i < enemiesArray.Length; i++) {
			for (int j= 0; j < enemiesArray[0].Length; j++) {
				string name = "e" + i.ToString () + "-" + j.ToString ();
				draw.EnemyEnable (name, ConvertCharToBool(enemiesArray[i][j]));
			}
		}
	}

	private bool ConvertCharToBool(char c){
		bool b = (c == '1') ? true : false;
		return b;
	}

	public void Battery(int batteries){

		draw.DisableAllImage (draw.batteryArray);
		for (int i = 0; i < batteries; i++) {
			draw.batteryArray [i].enabled = true;
		}
	}

	public void RealTime(int hour, int minute, string amOrPm){

		if (hour > 12) {
			hour -= 12;
			amOrPm = "PM";
		}
		draw.hour.text = AdjustDigits (hour);
		draw.minute.text = AdjustDigits (minute);
		draw.amOrPm.text = amOrPm;
	}

	public void CountDown(bool value, int timeForNextBat = 0){

		draw.ObjectShown (draw.countDown, value);
		if (value) {
			draw.countMinutes.text = AdjustDigits (timeForNextBat / 60);
			draw.countSeconds.text = AdjustDigits (timeForNextBat % 60);
		}
	}
	//Función para ajustar los dígitos ejemplo: 7:4 --> 07:04
	private string AdjustDigits(int num){

		if (num < 10) {
			return "0" + num.ToString ();
		} else {
			return num.ToString ();
		}
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

	public void ShowExit(){

		return;
	}

	public void GameOver(){

		return;
	}
}
