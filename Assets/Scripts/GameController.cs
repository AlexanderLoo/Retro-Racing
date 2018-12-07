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

    private State _state;

	[SerializeField] //UNITY
	public class State
	{
		public string name;
        public string[] enemiesArray; 
		public int playerPos = 1;
		public int level = 0;
		public int racingTime = 0;
        public int spaceCounter = 0;

        public State(int columnLength, string newWave){
            
             enemiesArray = new string[columnLength];
             for (int i = 0; i < columnLength; i++)
            {
                enemiesArray[i] = newWave;
            }
        }
	}

    private bool stateFirstRun = true;

	public float lowestSpeed = 16.6f; //16.6 m/s
	public float gameSpeed = 1f; //Velocidad del juego donde 1 es normal(para animaciones)
	private int startingTime; //TEST, se piensa cambiar el nombre de la variable

    public int rowLength = 3;
    public int columnLength = 4;

    private string emptyWave; //string rellenado con ceros
	private string newEnemiesWave; //el nuevo spawn de los enemigos expresado en un string binario
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

    private int score;
    private int scoreForNextLevel;
    private int[] levelScore = new int[] { 200, 300, 500, 800, 1300, 2100, 3400, 5500, 8900, 14400, 999999999};
	//TEST
	public bool doubleSpawn;
	public int space = 1; //<--Con level design establecemos el espacio de spawneo

	private bool interupted = false;

    void Awake()
    {
        //TEST
        display.Objects(rowLength, columnLength);
    }

    void Start(){

		display.NewScreen (); //TEST
		FirstTimePlaying();
		bat.batteries = pref.GetInt ("CurrentBatteries");

		if (StartingFromInterupted ()) {
			LoadState ();
			display.PlayerMove (_state.playerPos);
            emptyWave = Repeat("0", _state.enemiesArray[0].Length);
			display.Enemies (_state.enemiesArray);
			startingTime = CurrentTime();
            display.CurrentScore (Distance());
            scoreForNextLevel = levelScore[_state.level];
		} else {
            //prepare for new game
            emptyWave = Repeat("0", rowLength);
            _state = new State(columnLength, emptyWave);
			display.ShowSplashScreen ();
			display.PlayerMove (_state.playerPos);
			_state.name = "mainMenu";
		}
        newEnemiesWave = emptyWave;
		if (bat.Get() != bat.maxBatteries) {
			RemainingCharge ();
		}
		display.UI();					
		display.MainMenu (bat.Get(),lives.Get());
		SetState (_state.name);
	}

	void Update(){		

		//TEST
		TestingGetcurrentScreen();
		//print (_state.name); //TEST

		Charging();
        LocalTime();
		display.RealTime (hour, minute, amPm);
        buttons.Back();

		//TEST	
		if (timeForNextBat > 0) {
			display.BatCountDown (true, timeForNextBat);
		} else {
			display.BatCountDown (false);
		}

		switch (_state.name) {
			
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

    //states: mainMenu, startGame,playCountDown, playing, crashed, paused, gameover
	void SetState(string state){

		_state.name = state;
        stateFirstRun = true;
	}
	//TEST, SIN UTILIDAD POR AHORA
	string GetState(){

		return _state.name;
	}

    string Repeat(string str, int count){

        string newString = "";
        for (int i = 0; i < count; i++)
        {
            newString += str;
        }
        return newString;
    }

	void MainMenuState(){

        if (stateFirstRun)
        {
            buttons.Show(buttons.startButton, true);
            buttons.Show(buttons.pauseButton, false);
            buttons.Show(buttons.playButton, false);
            enemies.Reset(_state.enemiesArray, emptyWave);
            display.Enemies(_state.enemiesArray);
            display.DisableAllColision();
            _state.racingTime = 0;
            stateFirstRun = false;
        }
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
				timeToReachForBat = CurrentTime() + waitingTime;
				charging = true;
			}
			display.Battery (bat.Get ());
            display.CurrentScore(0);
            _state.level = 0;
            scoreForNextLevel = levelScore[0];
            timeToStartGame = CurrentTime() + 3; 
            SetState("playCountDown");
		} else {
			display.NotEnoughtBat (); //Posible animacion de la batería
			sound.BadLuck();
			SetState("mainMenu");
		}
	}

    void PlayCountDown(){

        if (stateFirstRun)
        {
            buttons.Show(buttons.pauseButton, false);
            buttons.Show(buttons.playButton, false);
            buttons.Show(buttons.startButton, false);
            stateFirstRun = false;
        }
        display.StartCountDown(timeToStartGame - CurrentTime());
        if (Now() >= timeToStartGame)
        {
            startingTime = CurrentTime();
            SetState("playing");
        }
    }

	void PlayingState(){
        
		//TEST
		#if UNITY_EDITOR
		buttons.KeysController();
		#endif

        if (stateFirstRun)
        {
            interupted = true;
            buttons.Show(buttons.pauseButton, true);
            display.PlayerMove(_state.playerPos);
            stateFirstRun = false;
        }

		if (buttons.PausePressed()) {
			buttons.SetPause (false);
            SetState("paused");
		}
		//TEST, buscando la mejor manera de manejar los segundos
        _state.racingTime += (CurrentTime() - startingTime);
		startingTime = CurrentTime();
		//racingTime += Time.deltaTime; //Esta lógica hace que el score corra más rápido

		display.CurrentScore(Distance ());
        LevelManager();

		if (buttons.Left() && player.CanLeft(_state.playerPos)) {
			display.PlayerMove(player.Movement(ref _state.playerPos, -1));
			buttons.SetLeft (false);
		}
		if (buttons.Right() && player.CanRight(_state.playerPos,rowLength - 1)) {
			display.PlayerMove(player.Movement(ref _state.playerPos, 1));
			buttons.SetRight (false);
		}
		//TEST
		//PlayerAi();

		if (EnemiesMoveNow()) {
			if (CanSpawn ()) {
				if (doubleSpawn)
					NewSpawn (1,0);
				else
					NewSpawn (0,1);
			} else {
				newEnemiesWave = emptyWave;
			}
			_state.enemiesArray = enemies.MoveDown(newEnemiesWave, _state.enemiesArray);
			display.Enemies(_state.enemiesArray);
		}

		if(colision.Crashed(_state.enemiesArray, _state.playerPos)){

			SetState("crashed");
		}
	}

	void PausedState(){

        if (stateFirstRun)
        {
            display.ShowExit();
            buttons.Show(buttons.pauseButton, false);
            buttons.Show(buttons.playButton, true);
            stateFirstRun = false;
        }
       
		if (buttons.StartPressed()) {
			buttons.SetStart (false);
            timeToStartGame = CurrentTime() + 3;
            SetState("playCountDown");
			//startingTime = CurrentTime();
            //SetState("playing");
		}
	}

	void CrashedState(){

		lives.Decrease (1);
		display.Crashed (_state.playerPos);
		sound.Crashed();
		//kenemies.Reset(); //TEST QUIZAS NO SE DEBA RESETEAR
		if (lives.Left ()) {
			SetState ("playing");
		} else {
			bat.GameOver();
			display.GameOver();
			sound.GameOver();
			timeToMainMenu = CurrentTime() + gameOverWait;
            SetState("gameOver");
		}
	}

	void GameOverState(){

        if (stateFirstRun)
        {
            interupted = false;
            buttons.Show(buttons.pauseButton, false);
            stateFirstRun = false;
        }
       
		 if (CurrentTime() > timeToMainMenu) {
			SetState ("mainMenu");
		}
	}

    private double Now(){

        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        double time = (System.DateTime.UtcNow - epochStart).TotalSeconds;
        return time;
    }

    private int CurrentTime(){

        return (int)Now();
    }

	private void LocalTime(){

		System.DateTime time = System.DateTime.Now; //Tiempo local

		hour = time.Hour;
		minute = time.Minute;
		seconds = time.Second;

	}

	private bool EnemiesMoveNow(){
		 
		if (Now() >= timeToNextEnemyMove) {
			timeToNextEnemyMove = Now() + enemySpeed * gameSpeed;
			return true;
		} else {
			return false;
		}
	}
	//TEST
	private bool CanSpawn(){
		
		if (_state.spaceCounter == 0) {
			_state.spaceCounter = space;
			return true;
		} else {
			_state.spaceCounter--;
			return false;
		}
	}

	//Función par crear los waves, manipula si se spawnea 1 ò 2 enemigos por fila
	//para un enemigo por fila --> setup:0 , chosenOne:1 --> NewSpawn(0,1);
	//para dos enemigos por fila --> setup:1, chosenOne:0 ---> NewSpawn(1,0);
	private void NewSpawn(int setup = 0, int chosenOne = 1){

		int arrayLength = newEnemiesWave.Length;
		int [] temporalArray = new int[arrayLength];

		for (int i = 0; i < arrayLength; i++) {
			temporalArray [i] = setup;
		}
		//int randomIndex = new System.Random ().Next (0, arrayLength);
		int randomIndex = RandomInt(0,arrayLength);
		temporalArray [randomIndex] = chosenOne;
		//única solución encontrada...
		string newString = string.Join("", new List<int>(temporalArray).ConvertAll(i => i.ToString()).ToArray());
		newEnemiesWave = newString;
	}

    private int Distance()
    {
        //Usando como parámetro mínimo de velocidad 60 km/h ---> 16.6 m/s(velocidad normal)
        //Aplicamos la siguiente fórmula para hallar la distancia ---> d = v*t
        //distance = (16.6 / speed) * racingTime
        //dividimos la velocidad(16.6) entre la variable speed por que mientras el speed sea mas bajo, la velocidad es mas alta

        float distance = (lowestSpeed / gameSpeed) * _state.racingTime;
        score = (int)(distance);
        return score;
    }

    private void LevelManager(){

        if (score >= scoreForNextLevel)
        {
            _state.level++;
            scoreForNextLevel = levelScore[_state.level];
        }
        switch (_state.level)
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
			case 10:
				ChangeLevel(0.1f,1,RandomBool());//<-- God Mode
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

		if (timeToReachForBat > CurrentTime()) {
			timeForNextBat = timeToReachForBat - CurrentTime();
		} else {
			timeForNextBat = 0;
			if (charging) {
				bat.Add (1);
				pref.SetInt ("CurrentBatteries", bat.batteries);
				display.Battery (bat.Get());
				if (bat.Get () != bat.maxBatteries) {
					timeToReachForBat = CurrentTime() + waitingTime;
				} else {
					charging = false;
				}
			}
		}
	}

	//TEST
	void TestingGetcurrentScreen(){

		if (Input.GetKeyDown(KeyCode.Space)) {
			display.NewScreen ();
			PlayerPrefs.DeleteAll ();
		}
	}
	//TEST
	//basic AI for fast testing
	void PlayerAi(){

		if(_state.enemiesArray[columnLength - 2][_state.playerPos] == '1'){
			for(int i = 0; i < _state.enemiesArray[columnLength - 2].Length; i++){
				if(_state.enemiesArray[columnLength - 2][i] == '0'){
					display.PlayerMove(player.Movement(ref _state.playerPos, i -_state.playerPos));
					break;
				}
			}
		}
	}

	public void RemainingCharge(){

//		int reachTimeForFullCharge = pref.GetInt ("LastTimePlayed") + pref.GetInt ("RemainingTime");
//
//		if (CurrentTime() >= reachTimeForFullCharge) {
//			bat.Add (bat.maxBatteries - bat.Get ());
//			pref.SetInt ("CurrentBatteries", bat.batteries);
//		} else {
//			int offLineTime = CurrentTime() - pref.GetInt("LastTimePlayed");
//			bat.Add ((offLineTime + pref.GetInt("TimeElapsedForBat"))/ waitingTime);
//			pref.SetInt ("CurrentBatteries", bat.batteries);
//			timeToReachForBat = ((reachTimeForFullCharge - CurrentTime()) % waitingTime) + CurrentTime();
//			charging = true;
//		}

		int offLineTime = CurrentTime() - pref.GetInt("LastTimePlayed") + pref.GetInt("TimeElapsedForBat");
		bat.Add (offLineTime / waitingTime);
		pref.SetInt ("CurrentBatteries", bat.batteries);
		if (bat.Get () == bat.maxBatteries) {
			timeToReachForBat = 0;
		} else {
			timeToReachForBat = CurrentTime() + (waitingTime -(offLineTime % waitingTime));
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
		pref.SetInt ("LastTimePlayed", CurrentTime());
		pref.SetInt("TimeElapsedForBat",timeElapsedForBat);
		SaveState ();
		//TEST
		pref.SetInt("Interupted", pref.SetInterupted(interupted));
	}

	private void SaveState() {
		
		string toSaveJson = JsonUtility.ToJson(_state); //UNITY
		pref.SetString ("CurrentState", toSaveJson);
	}

	public void LoadState() {

		string loadJson = pref.GetString("CurrentState");
		_state = JsonUtility.FromJson<State>(loadJson); //UNITY
	}

    //FUNCIONES DE SALIDA DEL JUEGO
    //private void OnApplicationPause(bool pause)
    //{
    //    SavePrefs();
    //}
    //private void OnApplicationFocus(bool focus)
    //{
    //    SavePrefs();
    //}
    //private void OnDisable()
    //{
    //    SavePrefs();
    //}
    private void OnApplicationQuit()
    {
        SavePrefs(); //TEST en caso de bug con playerPref, comentar esta linea
    }
}







