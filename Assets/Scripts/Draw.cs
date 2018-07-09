using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour {

    //TEST Quizas se muevan estas variables al gameController o a una nueva clase Theme
	//Sprite personalizables del juego
    public Sprite centerCar;
    public Sprite sideCar;
    public Sprite colision;
	public Sprite battery;
	public Sprite live;

	//Listas de Imagenes
	public List<SpriteRenderer> playerArray; //Buscamos las referencias en el editor para no perder el orden
    public List<SpriteRenderer> colisionArray;
	public List<Image> batteryArray; //Buscamos las referencias en el editor para no perder el orden
	public List<Image> livesArray;

	//UI
    public GameObject batCountDown;
	public Text countMinutes;
	public Text countSeconds;

    public Text scoreText;
    public Text playCountDown;

	public Text hour;
	public Text minute;
	public Text amOrPm;


    public Vector2 GetScreenSizeInPixels(){

        Vector2 screenSizeInPixels = new Vector2(Screen.width, Screen.height);
        return screenSizeInPixels;
    }

	//Función para acceder al tamaño de la pantalla convertidas a unidades de Unity
	public float GetScreenWidth(){

        Vector2 screenSize = Camera.main.ScreenToWorldPoint(GetScreenSizeInPixels());
		return screenSize.x;
	}
	public float GetScreenHeight(){

        Vector2 screenSize = Camera.main.ScreenToWorldPoint(GetScreenSizeInPixels());
        return screenSize.y;
	}

    public void FillSprites(string tag, Sprite centerSprite, Sprite sideSprite){

        GameObject[] array = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject go in array)
        {
            char lastChar = go.name[go.name.Length - 1];
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (lastChar == '1')
            {
                sr.sprite = centerSprite;
            }else{
                sr.sprite = sideSprite;
            }
        }
    }

	//Funciones para buscar las listas en la escena según su tag
	//void FindSpriteRendererArray(List<SpriteRenderer> array, string tag){
	//	GameObject[] goArray = GameObject.FindGameObjectsWithTag (tag);
	//	foreach (GameObject go in goArray) {
	//		array.Add (go.GetComponent<SpriteRenderer>());
	//	}
	//}
    //void FindImageArray(List<Image> array, string tag){
	//	GameObject[] goArray = GameObject.FindGameObjectsWithTag (tag);
	//	foreach (GameObject go in goArray) {
	//		array.Add (go.GetComponent<Image>());
	//	}
	//}

	void Start(){

        FillSprites("Player", centerCar, sideCar);
        FillSpriteArray(colisionArray, colision);
        FillSprites("Enemy", centerCar, sideCar);
		FillImages ("Battery", battery);
    //	FillImageArray (livesArray, live);
	}

	public void FillSpriteArray(List<SpriteRenderer> spriteArray, Sprite sprite){

		foreach (SpriteRenderer _sr in spriteArray) {
			_sr.sprite = sprite;
		}
	}

	public void FillImages(string tag, Sprite sprite){

        GameObject[] images = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject image in images)
        {
            Image i = image.GetComponent<Image>();
            i.sprite = sprite;
        }
    }

	public void DisableAllSprites(List<SpriteRenderer> spriteArray){

		foreach (SpriteRenderer _sr in spriteArray) {
			_sr.enabled = false;
		}
	}
	public void DisableAllImage(List<Image> imageArray){

		foreach (Image image in imageArray) {
			image.enabled = false;
		}
	}

	public void Enemy(string name, bool value){

		SpriteRenderer sr = FindName(name).GetComponent<SpriteRenderer>();
		sr.enabled = value;
	}

	public GameObject FindName(string name){  //Buscamos un GameObject por sunombre

		GameObject go = GameObject.Find (name);
		return go;
	}


	public void ObjectShown(GameObject gameObject, bool value){

		gameObject.SetActive (value);
	}
    //Función para dibujar poligonos
    public void Polygon2D(Vector2[] vertices, ushort[] triangles, Color color, int sortingOrder)
    {
        GameObject polygon = new GameObject();
        SpriteRenderer sr = polygon.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Background";
        sr.sortingOrder = sortingOrder;
        Texture2D texture = new Texture2D(1025, 1025);

        List<Color> cols = new List<Color>();
        for (int i = 0; i < (texture.width * texture.height); i++)
            cols.Add(color);
        texture.SetPixels(cols.ToArray());
        texture.Apply();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, 1024, 1024), Vector3.zero, 1);
        //sr.color = color; <--- Al parecer no es necesario ya que duplica el tono del color
        sr.sprite.OverrideGeometry(vertices, triangles);
        Vector2 newPos = new Vector2(-GetScreenWidth(),-GetScreenHeight());
        polygon.transform.position = newPos;
    }

	public void Console(){
		return;
	}

	public void Lives(){
		return;
	}


}
