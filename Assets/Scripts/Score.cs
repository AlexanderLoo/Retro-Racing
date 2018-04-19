using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

	//Guardamos el nuevo record
	public void SetScoreRecord(int newScore){

		PlayerPrefs.SetInt ("ScoreRecord", newScore);
	}
	//Obtenemos nuestro record
	public int GetScoreRecord(){

		return PlayerPrefs.GetInt ("ScoreRecord");
	}
}
