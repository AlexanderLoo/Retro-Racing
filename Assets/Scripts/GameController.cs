using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public TimeManager time;

	public Pause pause;

	public Lives lives;

	public class State
	{
		public string name;
		public float score;
		public int[] enemies;
		public int playerPos;
		public float gameSpeed;
		public float racingTime;
	}

	private string globalState;

	// Constantes
	public float lowestSpeed = 16.6f; //16.6 m/s

	public int gameSpeed = 1; //Velocidad del juego donde 1 es normal
	public float racingTime = 0; //Tiempo en carrera

	//variables para controllar las recargas de batería
	public const int waitingTime = 10; //segundos de espera para obtener una batería(una vez perdida)
	private int timeForNextBat; //Variable para saber el tiempo que falta para una batería(mostrada en el display)
	private int timeToReachForBat;//Temporal 
	private bool charging;//Temporal

	public int gameOverWait;
	public float timeToMainMenu;



	void Start(){

		if (StartingFromInterupted ()) {
			LoadState ();

		} else {
			// prepare for new game
			AlreadyPlayed();
			display.ShowSplashScreen ();
			bat.batteries = pref.Get ("CurrentBatteries");
			display.MainMenu (bat.Get(),lives.Get());

			SetState ("mainMenu");
		}

		AlreadyPlayed ();
		//RemainingCharge ();

	}

	void Update(){		

		print (globalState);

		Charging();
		display.RealTime (time.hour, time.minute, time.amPm);

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


	private void AlreadyPlayed(){

		if (pref.Get("AlreadyPlayed") == 0) {
			pref.Set ("AlreadyPlayed", 1);
			pref.Set ("CurrentBatteries", bat.maxBatteries);
		}
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
				timeToReachForBat = time.totalTime + waitingTime;
				charging = true;
			}

			display.Battery (bat.Get());
			display.StartingGame();
			SetState ("playing");
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

		racingTime += Time.deltaTime;

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

//		if (time.EnemiesMoveNow()) {
//			arrayOfEnemies = enemies.MoveDown();
//			display.Enemies(arrayOfEnemies);
//		}
//
//		if(colision.Crashed(player.currentIndex, arrayOfEnemies )){
//
//			SetState("crashed");
//		}
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
			timeToMainMenu = time.Now () + gameOverWait;
		}
	}

	void GameOverState(){

		if (buttons.StartPressed()) {
			SetState ("mainMenu");
		}
		else if (time.Now() < timeToMainMenu) {
			SetState ("mainMenu");
		}
	}

	//Temporal
	public void Charging(){

		if (timeToReachForBat > time.totalTime) {
			timeForNextBat = timeToReachForBat - time.totalTime;
		} else {
			timeForNextBat = 0;
			if (charging) {
				bat.Add (1);
				pref.Set ("CurrentBatteries", bat.batteries);
				display.Battery (bat.Get());
				if (bat.Get () != bat.maxBatteries) {
					timeToReachForBat = time.totalTime + waitingTime;
				} else {
					charging = false;
				}
			}
		}
	}

//	public void RemainingCharge(){
//
//		int reachTimeForFullCharge = pref.Get ("LastTimePlayed") + pref.Get ("RemainingTime");
//
//		if (time.totalTime >= reachTimeForFullCharge) {
//			bat.Add (bat.maxBatteries - bat.Get ());
//		} else {
//			int offLineTime = reachTimeForFullCharge - time.totalTime;
//			bat.Add (offLineTime / waitingTime);
//			timeToReachForBat = (offLineTime % waitingTime) + time.totalTime;
//			charging = true;
//		}
//	}
//	//Temporal
//	private void OnDisable(){
//
//		int remainingTimeForFullCharge = (bat.maxBatteries - bat.Get ()) * waitingTime -(waitingTime - timeForNextBat);
//		pref.Set("RemainingTime", remainingTimeForFullCharge);
//		pref.Set ("LastTimePlayed", time.totalTime);
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