using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

	public Display display;
	public Sound sound; 
	public Buttons buttons;

	public Player player;
	public Enemies enemies;
	public Colision colision;

	public Preference pref;

	public Battery bat;
	public ScoreManager score;

	public Pause pause;

	public Lives lives;

	public class State
	{
		public string name;
		public float score;
		public string[] enemiesArray; //array de enemigos
		public int playerPos;
		public float gameSpeed;
		public float racingTime;
	}

	private string globalState;

	// Constantes
	public float lowestSpeed = 16.6f; //16.6 m/s

	public int gameSpeed = 1; //Velocidad del juego donde 1 es normal(para animaciones)
	private float racingTime = 0; //Tiempo en carrera
	private int startingTime;

	private string[] arrayOfEnemies;

	//variables para controllar las recargas de batería
	public const int waitingTime = 10; //segundos de espera para obtener una batería(una vez perdida)
	private int timeForNextBat; //Variable para saber el tiempo que falta para una batería(mostrada en el display)
	private int timeToReachForBat;//Temporal 
	private bool charging;//Temporal

	private int gameOverWait;
	private float timeToMainMenu;

	//Para mostrar la hora actual(local)
	private int hour, minute, seconds;
	private string amPm = "AM";

	private int currentTime; //Tiempo total en segundos desde 1970

	void Awake(){

		Time ();
	}

	void Start(){

		display.GetCurrentScreen ();

		if (StartingFromInterupted ()) {
			LoadState ();

		} else {
			// prepare for new game
			PrepareForNew();
			display.ShowSplashScreen ();
			bat.batteries = pref.Get ("CurrentBatteries");
			display.MainMenu (bat.Get(),lives.Get());

			SetState ("mainMenu");
		}
		//RemainingCharge ();

	}

	void Update(){		

		//Test
		TestingGetcurrentScreen();
		Time ();
		print (globalState);

		Charging();
		display.RealTime (hour, minute, amPm);

		//Temporal
		if (timeForNextBat > 0) {
			display.CountDown (true, timeForNextBat);
		} else {
			display.CountDown (false);
		}

		switch (globalState) {
			
		case "mainMenu":
			MainMenuState ();
			break;
		case "startGame":
			StartGameState ();
			break;
		case "playing":
			PlayingState ();
			break;
		case "crashed":
			CrashedState ();
			break;
		case "paused":
			PausedState ();
			break;
		case "gameOver":
			GameOverState ();
			break;
		default:
			Debug.Log ("No se encuentra en ningún estado");	
			break;
		}
	}

	private bool StartingFromInterupted(){
		//Acceder a un prefs
		return false;
	}

	private void LoadState(){
		//Accedemos a la variables de la clase State y los asignamos al juego
		return;
	}

	private void PrepareForNew(){

		if (pref.Get("PrepareForNew") == 0) {
			pref.Set ("PrepareForNew", 1);
			pref.Set ("CurrentBatteries", bat.maxBatteries);
		}

		//state.enemies = enemyPrepareForNew();
	}

	//states: mainMenu, startGame, playing, crashed, paused, gameover
	void SetState(string state){

		globalState = state;
	}

	string GetState(){

		return globalState;
	}

	void MainMenuState(){

		if (buttons.StartPressed ()) {
			SetState ("startGame");
		} 
	}

	void StartGameState(){

		if (bat.Left ()) {

			bat.Add (-1);
			pref.Set ("CurrentBatteries", bat.batteries);
			//temporal
			if (!charging) {
				timeToReachForBat = currentTime + waitingTime;
				charging = true;
			}

			display.Battery (bat.Get());
			display.StartingGame();
			SetState ("playing");
			startingTime = currentTime;
			pause.BeforeStart();
		} else {
			display.NotEnoughtBat ();
			sound.BadLuck();
			SetState("mainMenu");
		}
	}

	void PlayingState(){

		#if UNITY_EDITOR
		buttons.KeysController();
		#endif

		racingTime = currentTime - startingTime; 
		//racingTime += Time.deltaTime;
		if (buttons.PausePressed()) {
			SetState("paused");
		}
		display.CurrentScore(score.Distance (lowestSpeed, gameSpeed, racingTime));


		if (buttons.Left() && player.CanLeft()) {
			display.PlayerMove(player.Movement(-1));
		}
		if (buttons.Right() && player.CanRight()) {
			display.PlayerMove(player.Movement(1));
		}

		if (EnemiesMoveNow()) {
			arrayOfEnemies = enemies.MoveDown();
			display.Enemies(arrayOfEnemies);
		}

		if(colision.Crashed(player.currentIndex, enemies.crashable)){

			SetState("crashed");
		}
	}

	void PausedState(){

		display.ShowExit ();

		if (buttons.StartPressed()) {
			SetState ("playing");
		}
	}

	void CrashedState(){

		lives.Decrease (1);
		display.Crashed ();
		sound.Crashed();
		pause.Crashed();
		enemies.Reset();
		if (lives.Left ()) {
			SetState ("playing");
		} else {
			bat.GameOver();
			display.GameOver();
			sound.GameOver();
			SetState("gameOver");
			timeToMainMenu = currentTime + gameOverWait;
		}
	}

	void GameOverState(){

		if (buttons.StartPressed()) {
			SetState ("mainMenu");
		}
		else if (currentTime < timeToMainMenu) {
			SetState ("mainMenu");
		}
	}

	private void Time(){

		DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		currentTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;

		DateTime time = DateTime.Now; //Tiempo local

		hour = time.Hour;
		minute = time.Minute;
		seconds = time.Second;
	}

	private bool EnemiesMoveNow(){
		return false;
	}


	//Temporal
	private void Charging(){

		if (timeToReachForBat > currentTime) {
			timeForNextBat = timeToReachForBat - currentTime;
		} else {
			timeForNextBat = 0;
			if (charging) {
				bat.Add (1);
				pref.Set ("CurrentBatteries", bat.batteries);
				display.Battery (bat.Get());
				if (bat.Get () != bat.maxBatteries) {
					timeToReachForBat = currentTime + waitingTime;
				} else {
					charging = false;
				}
			}
		}
	}

	//Temporal
	void TestingGetcurrentScreen(){

		if (Input.GetKeyDown(KeyCode.Space)) {
			display.GetCurrentScreen ();
		}
	}

//	public void RemainingCharge(){
//
//		int reachTimeForFullCharge = pref.Get ("LastTimePlayed") + pref.Get ("RemainingTime");
//
//		if (time.currentTime >= reachTimeForFullCharge) {
//			bat.Add (bat.maxBatteries - bat.Get ());
//		} else {
//			int offLineTime = reachTimeForFullCharge - time.currentTime;
//			bat.Add (offLineTime / waitingTime);
//			timeToReachForBat = (offLineTime % waitingTime) + time.currentTime;
//			charging = true;
//		}
//	}
//	//Temporal
//	private void OnDisable(){
//
//		int remainingTimeForFullCharge = (bat.maxBatteries - bat.Get ()) * waitingTime -(waitingTime - timeForNextBat);
//		pref.Set("RemainingTime", remainingTimeForFullCharge);
//		pref.Set ("LastTimePlayed", time.currentTime);
//	}
}


//pref.saveState {
//
//
//	string tosavejson = JsonUtility.ToJson(myObject);
//
//	magia para guardar tosavejson
//}
//
//
//
//pref.loadState {
//
//	loadjson = load pref
//
//		loadedState = JsonUtility.FromJson<State>(loadjson);
//
//	return loadedState;
//}