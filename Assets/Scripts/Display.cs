using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Draw draw;

	//Constantes de las posiciones de los elementos del UI
	private float[] startGameCountDownPos = new float[]{0.5f, 0.8f};
	private float[] batPos = new float[]{0.08f, 0.9f}, batMinPos = new float[]{0.18f, 0.9f}, batDotsPos = new float[]{0.21f, 0.9f}, batSecPos = new float[]{0.24f, 0.9f};
	private float[] bat0Pos = new float[]{0.05f, 0.89f}, bat1Pos = new float[]{0.08f, 0.89f}, bat2Pos = new float[]{0.11f, 0.89f};
	private float[] scorePos = new float[]{0.5f, 0.93f};
	private float[] pausePos = new float[]{0.93f, 0.95f}, playPos = new float[]{0.79f, 0.95f}, startPos = new float[]{0.93f, 0.07f};
	private float[] timeHourPos = new float[]{0.04f, 0.07f}, timeDotsPos = new float[]{0.075f, 0.07f}, timeMinutePos = new float[]{0.11f, 0.07f}, timeAmOrPmPos = new float[]{0.16f, 0.065f};

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

	public void UI(){

		//Dibujamos el canvas
		draw.UIElementPos(draw.startGameCountDown, startGameCountDownPos);
		draw.UIElementPos(draw.startGameCountDownB, startGameCountDownPos);
		draw.UIElementPos(draw.bat, batPos);
		draw.UIElementPos(draw.batB, batPos);
		draw.UIElementPos(draw.batMin, batMinPos);
		draw.UIElementPos(draw.batMinB, batMinPos);
		draw.UIElementPos(draw.batDots, batDotsPos);
		draw.UIElementPos(draw.batDotsB, batDotsPos);
		draw.UIElementPos(draw.batSec, batSecPos);
		draw.UIElementPos(draw.batSecB, batSecPos);
		draw.UIElementPos(draw.bat0, bat0Pos);
		draw.UIElementPos(draw.bat0B, bat0Pos);
		draw.UIElementPos(draw.bat1, bat1Pos);
		draw.UIElementPos(draw.bat1B, bat1Pos);
		draw.UIElementPos(draw.bat2, bat2Pos);
		draw.UIElementPos(draw.bat2B, bat2Pos);
		draw.UIElementPos(draw.score, scorePos);
		draw.UIElementPos(draw.scoreB, scorePos);
		draw.UIElementPos(draw.pause, pausePos);
		draw.UIElementPos(draw.pauseB, pausePos);
		draw.UIElementPos(draw.play, playPos);
		draw.UIElementPos(draw.playB, playPos);
		draw.UIElementPos(draw.start, startPos);
		draw.UIElementPos(draw.startB, startPos);
		draw.UIElementPos(draw.timeHour, timeHourPos);
		draw.UIElementPos(draw.timeHourB, timeHourPos);
		draw.UIElementPos(draw.timeDots, timeDotsPos);
		draw.UIElementPos(draw.timeDotsB, timeDotsPos);
		draw.UIElementPos(draw.timeMinute, timeMinutePos);
		draw.UIElementPos(draw.timeMinuteB, timeMinutePos);
		draw.UIElementPos(draw.timeAmOrPm, timeAmOrPmPos);
		draw.UIElementPos(draw.timeAmrOrPmB, timeAmOrPmPos);
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

    public void Objects(bool game, int rowLength, int columnLength){

        draw.GameObjects(game, "b", "Enemy", 0, 20, rowLength, columnLength, 0.1f, 0.1f, true);
        draw.GameObjects(game, "e", "Enemy", 1, 255, rowLength, columnLength, 0.1f, 0.1f);
		draw.GameObjects(game, "p", "Player", 0, 255, rowLength, columnLength, 0.1f, 0.1f, isPlayer:true);
    }

	public void StartCountDown(int count){

        draw.playCountDown.text = count.ToString();
        if (count <= 0) draw.playCountDown.text = null;
    }

	public void PlayerMove(int index){

		draw.DisableAllSprites (draw.playerArray);	
		draw.playerArray [index].enabled = true;
	}
	//mostramos a los enemigos con valor 1
	public void Enemies(string[] enemiesArray){  //<--- paso como argumentos el arreglo de strings en forma binaria "000","101",....

		int arrayLength = enemiesArray.Length;
		int rowLength = enemiesArray[0].Length;

		for (int i = 0; i < arrayLength; i++) {
			for (int j= 0; j < rowLength; j++) {
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
	//si es de cifra impar, concatenar con un espacio antes del numero (" "), si es menor que 10, concatenamos con un "0"
	public void CurrentScore (int score){
		string scoreStr = score.ToString();
		if(scoreStr.Length%2 != 0){
			if(score < 10) scoreStr = "0" + scoreStr;
			else scoreStr = " " + scoreStr;
		}
		draw.scoreText.text = scoreStr;
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
