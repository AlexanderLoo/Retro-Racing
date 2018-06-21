using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System; //comento la librería system por que se crea un conflicto con la clase random de unity y la clase random de system.

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

	[SerializeField]
	public class State
	{
		public string name;
		public float score;
		public string[] enemiesArray; //array de enemigos
		public int playerPos;
		public float gameSpeed;
		public float racingTime;
	}

	private string globalState = "mainMenu";

	public float lowestSpeed = 16.6f; //16.6 m/s

	public float gameSpeed = 1f; //Velocidad del juego donde 1 es normal(para animaciones)
	private float racingTime = 0; //Tiempo en carrera
	private int startingTime;

	private string[] arrayOfEnemies;
	//public int enemiesRowLength = 4;
	//private List<string> arrayOfEnemies;
	public string newEnemiesSpawn = "000"; //el nuevo spawn de los enemigos expresado en un string binario
	public float enemySpeed = 1f;
	private double timeToNextEnemyMove;

	//variables para controllar las recargas de batería
	public int waitingTime = 10; //segundos de espera para obtener una batería(una vez perdida)
	private int timeForNextBat; //Variable para saber el tiempo que falta para una batería(mostrada en el display)
	private int timeToReachForBat;//Temporal 
	private bool charging = false;//Temporal

	public int gameOverWait = 5;
	private int timeToMainMenu;

	//Para mostrar la hora actual(local)
	private int hour, minute, seconds;
	private string amPm = "AM";

	private double currentTime; //Tiempo total en segundos desde 1970

	//TEST
	public bool level2;
	public int space = 1; //<--Con level design establecemos el espacio de spawneo
	private int spaceCounter;

	private bool interupted = false;

	void Awake(){

		Time ();
	}

	void Start(){

		display.GetCurrentScreen ();
		FirstTimePlaying();
		bat.batteries = pref.GetInt ("CurrentBatteries");

		if (StartingFromInterupted ()) {
			LoadState ();
			display.PlayerMove (player.currentIndex);
			display.Enemies (enemies.array);

		} else {
			// prepare for new game
			display.ShowSplashScreen ();
			display.PlayerMove (player.currentIndex);
		}
		if (bat.Get() != bat.maxBatteries) {
			RemainingCharge ();
		}					
		display.MainMenu (bat.Get(),lives.Get());
		SetState (globalState);
	}

	void Update(){		

		//TEST
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

		if (pref.GetInt ("Interupted") == 1) {
			return true;
		} else {
			return false;
		}
	}

	private void FirstTimePlaying(){

		if (pref.GetInt("FirstTimePlaying") == 0) {
			pref.SetInt ("FirstTimePlaying", 1);
			pref.SetInt ("CurrentBatteries", bat.maxBatteries);
		}

		//state.enemies = enemyFirstTimePlaying();
	}

	//states: mainMenu, startGame, playing, crashed, paused, gameover
	void SetState(string state){

		globalState = state;
	}

	string GetState(){

		return globalState;
	}

	void MainMenuState(){

		buttons.Show (buttons.startButton, true);
		buttons.Show (buttons.pauseButton, false);
		if (buttons.StartPressed ()) {
			SetState ("startGame");
			buttons.SetStart (false);
		} 
	}

	void StartGameState(){

		if (bat.Left ()) {

			bat.Add (-1);
			pref.SetInt ("CurrentBatteries", bat.batteries);
			buttons.Show (buttons.startButton, false);
			buttons.Show (buttons.playButton, false);	
			buttons.Show (buttons.pauseButton, true);
			//temporal
			if (!charging) {
				timeToReachForBat = (int)currentTime + waitingTime;
				charging = true;
			}

			display.Battery (bat.Get());
			display.StartingGame();
			SetState ("playing");
			startingTime = (int)currentTime;
			pause.BeforeStart();
		} else {
			display.NotEnoughtBat ();
			sound.BadLuck();
			SetState("mainMenu");
		}
	}

	void PlayingState(){

		interupted = true;
		//TEST
		#if UNITY_EDITOR
		buttons.KeysController();
		#endif

		racingTime = (int)currentTime - startingTime; 
		//racingTime += Time.deltaTime; //Esta lógica hace que el score corra más rápido

		buttons.Show (buttons.pauseButton, true);
		buttons.Show (buttons.playButton, false);

		if (buttons.PausePressed()) {
			SetState("paused");
			buttons.SetPause (false);
		}
		display.CurrentScore(score.Distance (lowestSpeed, gameSpeed, racingTime));

		display.PlayerMove (player.currentIndex);

		if (buttons.Left() && player.CanLeft()) {
			display.PlayerMove(player.Movement(-1));
			buttons.SetLeft (false);
		}
		if (buttons.Right() && player.CanRight()) {
			display.PlayerMove(player.Movement(1));
			buttons.SetRight (false);
		}

		if (EnemiesMoveNow()) {
			if (CanSpawn ()) {
				if (level2)
					NewSpawn (1,0);
				else
					NewSpawn (0,1);
			} else {
				newEnemiesSpawn = "000";
			}
			arrayOfEnemies = enemies.MoveDown(newEnemiesSpawn);
			display.Enemies(arrayOfEnemies);
		}

		if(colision.Crashed(arrayOfEnemies, player.currentIndex)){

			SetState("crashed");
		}
	}

	void PausedState(){

		display.ShowExit ();
		buttons.Show (buttons.pauseButton, false);
		buttons.Show (buttons.playButton, true);
		if (buttons.StartPressed()) {
			SetState ("playing");
			buttons.SetStart (false);
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
			timeToMainMenu = (int)currentTime + gameOverWait;
		}
	}

	void GameOverState(){

		interupted = false;
		 if ((int)currentTime > timeToMainMenu) {
			SetState ("mainMenu");
		}
	}

	private void Time(){

		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		currentTime = (System.DateTime.UtcNow - epochStart).TotalSeconds;

		System.DateTime time = System.DateTime.Now; //Tiempo local

		hour = time.Hour;
		minute = time.Minute;
		seconds = time.Second;
	}

	private bool EnemiesMoveNow(){
		 
		if (currentTime >= timeToNextEnemyMove) {
			timeToNextEnemyMove = currentTime + enemySpeed * gameSpeed;
			return true;
		} else {
			return false;
		}
	}
	//TEST
	private bool CanSpawn(){
		
		if (spaceCounter == 0) {
			spaceCounter = space;
			return true;
		} else {
			spaceCounter--;
			return false;
		}
	}

	//Función par crear los waves, manipula si se spawnea 1 ò 2 enemigos por fila
	//para un enemigo por fila --> setup:0 , chosenOne:1 --> NewSpawn(0,1);
	//para dos enemigos por fila --> setup:1, chosenOne:0 ---> NewSpawn(1,0);
	private void NewSpawn(int setup = 0, int chosenOne = 1){

		int arrayLength = newEnemiesSpawn.Length;
		int [] temporalArray = new int[arrayLength];

		for (int i = 0; i < arrayLength; i++) {
			temporalArray [i] = setup;
		}
		int randomIndex = new System.Random ().Next (0, arrayLength);
		temporalArray [randomIndex] = chosenOne;
		//única solución encontrada...
		string newString = string.Join("", new List<int>(temporalArray).ConvertAll(i => i.ToString()).ToArray());
		newEnemiesSpawn = newString;
	}

	//Temporal
	private void Charging(){

		if (timeToReachForBat > (int)currentTime) {
			timeForNextBat = timeToReachForBat - (int)currentTime;
		} else {
			timeForNextBat = 0;
			if (charging) {
				bat.Add (1);
				pref.SetInt ("CurrentBatteries", bat.batteries);
				display.Battery (bat.Get());
				if (bat.Get () != bat.maxBatteries) {
					timeToReachForBat = (int)currentTime + waitingTime;
				} else {
					charging = false;
				}
			}
		}
	}

	//TEST
	void TestingGetcurrentScreen(){

		if (Input.GetKeyDown(KeyCode.Space)) {
			display.GetCurrentScreen ();
			PlayerPrefs.DeleteAll ();
		}
	}

	public void RemainingCharge(){

//		int reachTimeForFullCharge = pref.GetInt ("LastTimePlayed") + pref.GetInt ("RemainingTime");
//
//		if ((int)currentTime >= reachTimeForFullCharge) {
//			bat.Add (bat.maxBatteries - bat.Get ());
//			pref.SetInt ("CurrentBatteries", bat.batteries);
//		} else {
//			int offLineTime = (int)currentTime - pref.GetInt("LastTimePlayed");
//			bat.Add ((offLineTime + pref.GetInt("TimeElapsedForBat"))/ waitingTime);
//			pref.SetInt ("CurrentBatteries", bat.batteries);
//			timeToReachForBat = ((reachTimeForFullCharge - (int)currentTime) % waitingTime) + (int)currentTime;
//			charging = true;
//		}

		int offLineTime = (int)currentTime - pref.GetInt("LastTimePlayed") + pref.GetInt("TimeElapsedForBat");
		bat.Add (offLineTime / waitingTime);
		pref.SetInt ("CurrentBatteries", bat.batteries);
		if (bat.Get () == bat.maxBatteries) {
			timeToReachForBat = 0;
		} else {
			timeToReachForBat = (int)currentTime + (waitingTime -(offLineTime % waitingTime));
			charging = true;
		}
	}
	//TEST(cuando salimos de la aplicación)
	private void OnDisable(){

//		int remainingTimeForFullCharge;
		int timeElapsedForBat = waitingTime - timeForNextBat;
//		if (bat.Get () != bat.maxBatteries) {
//			
//			remainingTimeForFullCharge = (bat.maxBatteries - bat.Get ()) * waitingTime - (timeElapsedForBat);
//		} else {
//			remainingTimeForFullCharge = 0;
//		}
//		pref.SetInt ("RemainingTime", remainingTimeForFullCharge);
		pref.SetInt ("LastTimePlayed", (int)currentTime);
		pref.SetInt("TimeElapsedForBat",timeElapsedForBat);
		SaveState ();
		//TEST
		pref.SetInt("Interupted", pref.SetInterupted(interupted));
	}

	private void SaveState() {

		State state = new State ();
		state.name = globalState;
		state.score = score.Distance (lowestSpeed, gameSpeed, racingTime);
		state.enemiesArray = arrayOfEnemies;
		state.playerPos = player.currentIndex;
		state.gameSpeed = gameSpeed;
		state.racingTime = racingTime;
		string toSaveJson = JsonUtility.ToJson(state);
		pref.SetString ("CurrentState", toSaveJson);
	}

	public void LoadState() {

		string loadJson = pref.GetString("CurrentState");
		State loadedState = JsonUtility.FromJson<State>(loadJson);
		globalState = loadedState.name;
		display.CurrentScore(loadedState.score);
		enemies.array = loadedState.enemiesArray;
		player.currentIndex = loadedState.playerPos;
		gameSpeed = loadedState.gameSpeed;
		racingTime = loadedState.racingTime;
	}
}







