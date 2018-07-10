using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour {

    //TEST Quizas se muevan estas variables al gameController o a una nueva clase Theme
	//Sprite personalizables del juego
    public Sprite centerCar;
    public Sprite sideCar;
    public Sprite centerColision;
    public Sprite sideColision;
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


    //TEST
    //Variables de temas
    public Vector2[] skyVertices;
    public ushort[] triangles;

    public Vector2[] groundVertices;
    public Vector2[] roadVertices;

    public Color sky;
    public Color ground;
    public Color road;

    //TEST
    void Awake()
    {
        //TEMAS
        skyVertices = new Vector2[] { new Vector2(0, 1) * 2, new Vector2(0, GetScreenHeight()) * 2, new Vector2(GetScreenWidth(), GetScreenHeight()) * 2, new Vector2(GetScreenWidth(), 1) * 2 };
        //ushort[] rectTriangles = new ushort[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1}; // usando cuatro triangulos(requiere de otro vertice en el medio del rectangulo --> new Vector2(screenwith, screenHeigth)
        triangles = new ushort[] { 0, 1, 2, 0, 2, 3 }; //usando 2 triangulos rectangulos

        groundVertices = new Vector2[] { new Vector2(0, 1) * 2, new Vector2(0, GetScreenHeight() * 0.7f) * 2, new Vector2(GetScreenWidth(), GetScreenHeight() * 0.7f) * 2, new Vector2(GetScreenWidth(), 1) * 2 };
        roadVertices = new Vector2[] { new Vector2(0, 1) * 2, new Vector2((0.33f * GetScreenWidth()), (0.7f * GetScreenHeight())) * 2, new Vector2((0.66f * GetScreenWidth()), (0.7f * GetScreenHeight())) * 2, new Vector2(GetScreenWidth(), 1) * 2 };
        //ushort[] trapTriangles = new ushort[]{0,1,2,1,2,3,2,3,4}; <-- opcion con 3 triangulos en el medio

        sky = new Color(154, 202, 231, 255) / 255; //Ford Desert Sky Blue
        ground = new Color(225, 169, 95, 255) / 255; //Yellow Earth Color
        road = new Color(132, 115, 90, 255) / 255; //Cement Color
    }

    void Start()
    {
        Polygon2D(skyVertices, triangles, sky, 0);
        Polygon2D(groundVertices, triangles, ground, 1);
        Polygon2D(roadVertices, triangles, road, 2);

        FillSprites("Player", centerCar, sideCar);
        FillSprites("Colision", centerColision, sideColision);
        FillSprites("Enemy", centerCar, sideCar);
        FillImages("Battery", battery);
        //  FillImageArray (livesArray, live);
    }

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
	//public void FillSpriteArray(List<SpriteRenderer> spriteArray, Sprite sprite){
	//	foreach (SpriteRenderer _sr in spriteArray) {
	//		_sr.sprite = sprite;
	//	}
	//}

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

	public GameObject FindName(string name){  //Buscamos un GameObject por su nombre

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
