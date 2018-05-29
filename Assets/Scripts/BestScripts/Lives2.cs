using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives2 : MonoBehaviour {


	private int lives;
	private bool livesLeft;

	public int Get(){

		return lives;
	}

	public bool Left(){

		return livesLeft;
	}

	public void Decrease(int value){

		lives -= value;
	}
}
