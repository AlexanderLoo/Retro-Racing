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

	public Lives lives;

	[SerializeField] //UNITY
	public class State
	{
		public string name;
		public string[] enemiesArray; //array de enemigos
		public int playerPos;
		public int level;
		public int racingTime;
	}

	private string globalState;

	public float lowestSpeed = 16.6f; //16.6 m/s

	public float gameSpeed = 1f; //Velocidad del juego donde 1 es normal(para animaciones)
	private int racingTime = 0; //Tiempo en carrera
	private int startingTime; //TEST, se piensa cambiar el nombre de la variable

	private string[] arrayOfEnemies;
	//public int enemiesRowLength = 4;
	//private List<string> arrayOfEnemies;
	private string newEnemiesSpawn = "000"; //el nuevo spawn de los enemigos expresado en un string binario
	public float enemySpeed = 1f;
	private double timeToNextEnemyMove;

	//variables para controllar las recargas de batería
	public int waitingTime = 60; //segundos de espera para obtener una batería(una vez perdida)
	private int timeForNextBat; //Variable para saber el tiempo que falta para una batería(mostrada en el display)
	private int timeToReachForBat;
	private bool charging = false;

    public int timeToStartGame;
	public int gameOverWait = 5;
	private int timeToMainMenu;

	//Para mostrar la hora actual(local)
	private int hour, minute, seconds;
	private string amPm = "AM";

	private double currentTime; //Tiempo total en segundos desde 1970

    private int score;
    private int scoreForNextLevel;
    public int[] levelScore;
    public int level; //indice de levelScore
	//TEST
	public bool doubleSpawn;
	public int space = 1; //<--Con level design establecemos el espacio de spawneo
	private int spaceCounter;

	private bool interupted = false;

	void Awake(){

		TimeManager ();
	}

	void Start(){

		display.GetCurrentScreen (); //TEST
		FirstTimePlaying();
		bat.batteries = pref.GetInt ("CurrentBatteries");

		if (StartingFromInterupted ()) {
			LoadState ();
			display.PlayerMove (player.currentIndex);
			display.Enemies (enemies.array);
			startingTime = (int)currentTime;
            display.CurrentScore (Distance());
            scoreForNextLevel = levelScore[level];
            spaceCounter = space;
		} else {
			// prepare for new game
			display.ShowSplashScreen ();
			display.PlayerMove (player.currentIndex);
			globalState = "mainMenu";
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
		TimeManager ();
		print (globalState); //TEST

		Charging();
		display.RealTime (hour, minute, amPm);
        buttons.Back();

		//TEST	
		if (timeForNextBat > 0) {
			display.BatCountDown (true, timeForNextBat);
		} else {
			display.BatCountDown (false);
		}

		switch (globalState) {
			
		case "mainMenu":
			MainMenuState ();
			break;
		case "startGame":
			StartGameState ();
			break;
        case "playCountDown":
            PlayCountDown();
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
			Debug.Log ("No se encuentra en ningún estado");	//TEST
			break;
		}
	}

	private bool StartingFromInterupted(){

		if (pref.GetInt ("Interupted") == 1) {
            interupted = true;
		} else {
			interupted = false;
		}
        return interupted;
	}

	private void FirstTimePlaying(){

		if (pref.GetInt("FirstTimePlaying") == 0) {
			pref.SetInt ("FirstTimePlaying", 1);
			pref.SetInt ("CurrentBatteries", bat.maxBatteries);
		}
	}

	//states: mainMenu, startGame, playing, crashed, paused, gameover
	void SetState(string state){

		globalState = state;
	}
	//TEST, SIN UTILIDAD POR AHORA
	string GetState(){

		return globalState;
	}

	void MainMenuState(){

		buttons.Show (buttons.startButton, true);
		buttons.Show (buttons.pauseButton, false);
		buttons.Show (buttons.playButton, false);
		enemies.Reset ();
		display.Enemies (enemies.array);
        display.DisableAllColision();
		racingTime = 0;
		if (buttons.StartPressed ()) {
			buttons.SetStart (false);
            SetState("startGame");
		} 
	}

	void StartGameState(){

		if (bat.Left ()) {

			bat.Add (-1);
			pref.SetInt ("CurrentBatteries", bat.batteries);
			buttons.Show (buttons.startButton, false);
			//TEST
			if (!charging) {
				timeToReachForBat = (int)currentTime + waitingTime;
				charging = true;
			}
			display.Battery (bat.Get ());
            display.CurrentScore(0);
            level = 0;
            scoreForNextLevel = levelScore[0];
            timeToStartGame = (int)currentTime + 3; 
            SetState("playCountDown");
		} else {
			display.NotEnoughtBat (); //Posible animacion de la batería
			sound.BadLuck();
			SetState("mainMenu");
		}
	}

    void PlayCountDown(){

        buttons.Show(buttons.pauseButton, false);
        buttons.Show(buttons.playButton, false);
        buttons.Show(buttons.startButton, false);

        display.StartCountDown(timeToStartGame - (int)currentTime);
        if (currentTime >= timeToStartGame)
        {
            startingTime = (int)currentTime;
            SetState("playing");
        }
    }

	void PlayingState(){

		interupted = true;
		//TEST
		#if UNITY_EDITOR
		buttons.KeysController();
		#endif

		buttons.Show (buttons.pauseButton, true);

		if (buttons.PausePressed()) {
			buttons.SetPause (false);
            SetState("paused");
		}
		//TEST, buscando la mejor manera de manejar los segundos
        racingTime += ((int)currentTime - startingTime);
		startingTime = (int)currentTime;
		//racingTime += Time.deltaTime; //Esta lógica hace que el score corra más rápido

		display.CurrentScore(Distance ());
        LevelManager();

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
				if (doubleSpawn)
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
			buttons.SetStart (false);
            timeToStartGame = (int)currentTime + 3;
            SetState("playCountDown");
			//startingTime = (int)currentTime;
            //SetState("playing");
		}
	}

	void CrashedState(){

		lives.Decrease (1);
		display.Crashed (player.currentIndex);
		sound.Crashed();
		//kenemies.Reset(); //TEST QUIZAS NO SE DEBA RESETEAR
		if (lives.Left ()) {
			SetState ("playing");
		} else {
			bat.GameOver();
			display.GameOver();
			sound.GameOver();
			timeToMainMenu = (int)currentTime + gameOverWait;
            SetState("gameOver");
		}
	}

	void GameOverState(){

		interupted = false;
		buttons.Show (buttons.pauseButton, false);
		 if ((int)currentTime > timeToMainMenu) {
			SetState ("mainMenu");
		}
	}

	private void TimeManager(){

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

    private int Distance()
    {
        //Usando como parámetro mínimo de velocidad 60 km/h ---> 16.6 m/s(velocidad normal)
        //Aplicamos la siguiente fórmula para hallar la distancia ---> d = v*t
        //distance = (16.6 / speed) * racingTime
        //dividimos la velocidad(16.6) entre la variable speed por que mientras el speed sea mas bajo, la velocidad es mas alta

        float distance = (lowestSpeed / gameSpeed) * racingTime;
        score = (int)(distance);
        return score;
    }

    private void LevelManager(){

        if (score >= scoreForNextLevel)
        {
            level++;
            scoreForNextLevel = levelScore[level];
        }
        switch (level)
        {
            case 0:
                ChangeLevel(1,4,false);
                break;
            case 1:
                ChangeLevel(1,3,RandomBool());
                break;
            case 2:
                ChangeLevel(1,2,true);
                break;
            case 3:
                ChangeLevel(1,1,RandomBool());
                break;
            case 4:
                ChangeLevel(0.9f,1,true);
                break;
            case 5:
                ChangeLevel(0.8f,1,RandomBool());
                break;
            case 6:
                ChangeLevel(0.7f,1,true);
                break;
            case 7:
                ChangeLevel(0.6f,1,RandomBool());
                break;
            case 8:
                ChangeLevel(0.5f,RandomInt(1,3),true);
                break;
            case 9:
                ChangeLevel(0.4f,1,RandomBool());
                break;
        }
    }

    private void ChangeLevel(float gameSpeed, int space, bool doubleSpawn){

        this.gameSpeed = gameSpeed;
        this.space = space;
        this.doubleSpawn = doubleSpawn;
    }

    private int RandomInt(int min = 0, int max = 2)
    {
        int randomIndex = new System.Random().Next(min, max);
        return randomIndex;
    }

    private bool RandomBool(int min = 0, int max = 2){

        int randomIndex = RandomInt(min,max);
        bool value = (randomIndex == 0) ? false : true;
        return value;
    }

	//TEST
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
    private void SavePrefs(){
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
		state.enemiesArray = enemies.array;
		state.playerPos = player.currentIndex;
        state.level = level;
		state.racingTime = racingTime;
		string toSaveJson = JsonUtility.ToJson(state); //UNITY
		pref.SetString ("CurrentState", toSaveJson);
	}

	public void LoadState() {

		string loadJson = pref.GetString("CurrentState");
		State loadedState = JsonUtility.FromJson<State>(loadJson); //UNITY
		globalState = loadedState.name;
		enemies.array = loadedState.enemiesArray;
		player.currentIndex = loadedState.playerPos;
        level = loadedState.level;
		racingTime = loadedState.racingTime;
	}

    //FUNCIONES DE SALIDA DEL JUEGO

    private void OnApplicationFocus(bool focus)
    {
        SavePrefs();
    }
    private void OnDisable()
    {
        SavePrefs();
    }
    private void OnApplicationQuit()
    {
        SavePrefs();
    }
    //private void OnApplicationPause(bool pause)
    //{
    //    print("pause");
    //    SavePrefs();
    //}

}







