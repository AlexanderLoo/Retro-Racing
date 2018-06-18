using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

	//public List<string> array = new List<string>(4){"000","000","000","000"};
	public string[] array = new string[4]{"000","000","000","000"};
	public int crashable; //indice del enemigo que el player puede chocar

//	//Función que hace uso de la librería system.collectios.generic
//	public List<string> MoveDown(string newSpawn = "000"){
//
//		array.Insert (0, newSpawn);
//		return array;
//	}

	//Función para hacer Insert() a una array que no use la librería system(List)
	public string[] MoveDown(string newSpawn){

		int arrayLength = array.Length;
		string newValue = array [0];
		string oldValue;

		for (int i = 1; i < arrayLength; i++) {
			oldValue = array [i];
			array [i] = newValue;
			newValue = oldValue;
		}
		array[0] = newSpawn;

		return array;
	}

//		for (int i = array.Length -1; i > -1; i--) { <-- Otra alternativa pero quizas consuma más memoria
//			if (i == 0)
//				array [i] = newSpawn;
//			else
//				array [i] = array [i - 1];
//		}
//		return array;
//	}

	public void Reset(){

		return;
	}

}
