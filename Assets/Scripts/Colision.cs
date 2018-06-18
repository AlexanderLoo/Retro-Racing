using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colision : MonoBehaviour {

	public bool Crashed(string[]enemiesArray, int playerIndex){

		if (enemiesArray [enemiesArray.Length - 1] [playerIndex] == '1') {
			return true;
		} else {
			return false;
		}
	}
}
