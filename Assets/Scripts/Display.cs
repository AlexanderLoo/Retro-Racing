﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Draw draw;

	public class _Screen
	{
		public float width;
		public float height;

		public _Screen(float width, float height){
            
			this.width = width;
			this.height = height;
		}
	}

	public void NewScreen(){

		_Screen newScreen = new _Screen (draw.GetScreenWidth(), draw.GetScreenHeight());
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

    public void Objects(int columnLength, int rowLength){

        draw.GameObjects("b", "Enemy", 0, 20, columnLength, rowLength);
        draw.GameObjects("e", "Enemy", 1, 255, columnLength, rowLength);
    }

	public void StartCountDown(int count){

        draw.playCountDown.text = count.ToString();
        if (count <= 0) draw.playCountDown.text = null;
    }

	public void PlayerMove(int index){

		draw.DisableAllSprites (draw.playerArray);	
		draw.playerArray [index].enabled = true;
	}

	public void Enemies(string[] enemiesArray){  //<--- paso como argumentos el arreglo de strings en forma binaria "000","101",....

		int arrayLength = enemiesArray.Length;
		int rowlength = enemiesArray[0].Length;

		for (int i = 0; i < arrayLength; i++) {
			for (int j= 0; j < rowlength; j++) {
				string name = "e" + i.ToString () + "-" + j.ToString ();
				draw.Enemy (name, ConvertCharToBool(enemiesArray[i][j]));
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

	public void BatCountDown(bool value, int timeForNextBat = 0){

		draw.ObjectShown (draw.batCountDown, value);
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

	public void CurrentScore (int score){

		draw.scoreText.text = score.ToString();
	}
    public void DisableAllColision(){

        draw.DisableAllSprites(draw.colisionArray);
    }

	public void Crashed(int index){

        draw.colisionArray[index].enabled = true;
	}

	public void ShowExit(){

		return;
	}

	public void GameOver(){

		return;
	}
}
