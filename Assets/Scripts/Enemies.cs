using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

	//public List<string> array = new List<string>(4){"000","000","000","000"};
	public string[] array = new string[4];

//	//Función que hace uso de la librería system.collectios.generic
//	public List<string> MoveDown(string newSpawn = "000"){
//
//		array.Insert (0, newSpawn);
//		return array;
//	}

//	public void SetRowLength(int rowLength){
//
//		array = new string[rowLength];
//		for (int i = 0; i < rowLength; i++) {
//			array [i] = "000"; //<---Quizas no sea necesario
//		}
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

		int arrayLength = array.Length;

		for (int i = 0; i < arrayLength; i++) {
			array [i] = "000";
		}
	}

}
