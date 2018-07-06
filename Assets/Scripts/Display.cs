using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Draw draw;

    //TEST
    //Variables de temas
    public Vector2[] skyVertices;
    public ushort[] triangles;

    public Vector2[] groundVertices;
    public Vector2[] roadVertices;

    public Color sky;
    public Color ground;
    public Color road;

	public class _Screen
	{
		public float width;
		public float height;

		public _Screen(float width, float height){
            
			this.width = width;
			this.height = height;
		}
	}

    //TEST
    void Start()
    {
        //TEMAS
        skyVertices = new Vector2[] { new Vector2(0, 1) * 2, new Vector2(0, draw.GetScreenHeight()) * 2, new Vector2(draw.GetScreenWidth(), draw.GetScreenHeight()) * 2, new Vector2(draw.GetScreenWidth(), 1) * 2 };
        //ushort[] rectTriangles = new ushort[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1}; // usando cuatro triangulos(requiere de otro vertice en el medio del rectangulo --> new Vector2(screenwith, screenHeigth)
        triangles = new ushort[] { 0, 1, 2, 0, 2, 3 }; //usando 2 triangulos rectangulos

        groundVertices = new Vector2[] { new Vector2(0, 1) * 2, new Vector2(0, draw.GetScreenHeight() * 0.7f) * 2, new Vector2(draw.GetScreenWidth(), draw.GetScreenHeight() * 0.7f) * 2, new Vector2(draw.GetScreenWidth(), 1) * 2 };
        roadVertices = new Vector2[] { new Vector2(0, 1) * 2, new Vector2((0.33f * draw.GetScreenWidth()), (0.7f * draw.GetScreenHeight())) * 2, new Vector2((0.66f * draw.GetScreenWidth()), (0.7f * draw.GetScreenHeight())) * 2, new Vector2(draw.GetScreenWidth(), 1) * 2 };
        //ushort[] trapTriangles = new ushort[]{0,1,2,1,2,3,2,3,4}; <-- opcion con 3 triangulos en el medio

        sky = new Color(154, 202, 231, 255) / 255; //Ford Desert Sky Blue
        ground = new Color(225, 169, 95, 255) / 255; //Yellow Earth Color
        road = new Color(132, 115, 90, 255) / 255; //Cement Color
    }

	public void NewScreen(){

		_Screen newScreen = new _Screen (draw.GetScreenWidth(), draw.GetScreenHeight());
        draw.Polygon2D(skyVertices, triangles, sky, 0);
        draw.Polygon2D(groundVertices, triangles, ground, 1);
        draw.Polygon2D(roadVertices, triangles, road, 2);
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
