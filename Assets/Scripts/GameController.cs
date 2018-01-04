using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController gameController;
	public SpriteRenderer[] enemy;
	public SpriteRenderer[] player;
	public SpriteRenderer[] collision;
	public Image[] lives;
	//Velocidad 1 que equivale a 16.6 m/s
	public float speed = 1;
	public int score;
	public Text scoreText;
	public bool canPlay;
	public GameObject starButton;
	public bool startGame;
	//public bool gameOver;
	public int currentLife;


	void Awake(){
		//Singleton
		if (gameController == null) {
			gameController = this;
		} else if (gameController != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		//EN ESPERA DE SUGERENCIAS PARA MEJORAR LA LÓGICA
		if (PlayerPrefs.GetInt ("FirstTime") == 0) {
			PlayerPrefs.SetInt ("FirstTime", 1);
			currentLife = 3;
			PlayerPrefs.SetInt ("CurrentLife", currentLife);
		} else {
			currentLife = PlayerPrefs.GetInt ("CurrentLife");
		}
		ShowCurrentLives ();
		if (currentLife <= 0) {
			canPlay = false;
		} else {
			canPlay = true;
			starButton.SetActive (true);
		}

		scoreText.text = "00";
		Time.timeScale = 0;
	}

	void Update(){
		//Lógica de colisión
		if (startGame) {
			//Si el indice del sprite activo del player encaja con el del enemigo, significa que colisionaron; por lo tanto detenemos el juego
			for (int i = 0; i < player.Length; i++) {
				if (enemy[i].enabled && player[i].enabled) {
					startGame = false;
					//mostramos la colisión
					collision[i].enabled = true;
					RemoveLife ();
					//INVOCACIÓN TEMPORAL
					Invoke ("RestartGame",3);
				}
			}
			Distance ();
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			Application.Quit();
		}
	}
	//Función para mostrar las vidas disponibles al iniciar el juego
	void ShowCurrentLives(){

		currentLife = PlayerPrefs.GetInt ("CurrentLife");

		for (int i = lives.Length -1; i > lives.Length - 1 - currentLife ; i--) {
			lives [i].enabled = true;
		}
	}
	//Función para remover una vida
	void RemoveLife(){

		currentLife -= 1;
		PlayerPrefs.SetInt ("CurrentLife", currentLife);
		for (int i = 0; i < lives.Length; i++) {
			if (lives[i].enabled) {
				lives [i].enabled = false;
				break;
			}
		}
	}
	//Función para reiniciar el juego
	void RestartGame(){
		SceneManager.LoadScene ("Main");
	}

	//Función para calcular la distancia en km y mostrarlas en el score
	void Distance(){
		//Usando como parámetro mínimo de velocidad 60 km/h ---> 16.6 m/s(km/h * 0.277)
		//Aplicamos la siguiente fórmula para hallar la distancia ---> d = v*t
		//distance = (16.6 / speed) * time ---> distance = (16.6 / speed) * Time.time(distancia en metros, distancia en km --> score/1000)
		//dividimos la velocidad(16.6) entre la variable speed por que mientras el speed sea mas bajo, la velocidad es mas alta

		float distance = (float)(16.6/speed) * Time.time;
		scoreText.text = Mathf.Round (distance).ToString ();
	}
}
