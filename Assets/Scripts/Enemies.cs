using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

	public string[] array = new string[4]{"000","000","000","000"};// arreglo de enemigos bidimensional ---> ["001","101","000","110"]
	public string newSpawnArray;
	public int crashable; //indice del enemigo que el player puede chocar

	public string[] MoveDown(){

		return array;
	}

	public void Reset(){

		return;
	}

}
