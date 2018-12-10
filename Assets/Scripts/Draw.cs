using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : MonoBehaviour {

	//TEST
	public Display display;
    //TEST Quizas se muevan estas variables al gameController o a una nueva clase Theme
	//Sprite personalizables del juego
    public Sprite centerCar;
    public Sprite sideCar;
    public Sprite centerColision;
    public Sprite sideColision;
	public Sprite battery;
	public Sprite live;

	//Listas de Imagenes
	public List<SpriteRenderer> playerArray;
    public List<SpriteRenderer> colisionArray;
	public List<Image> batteryArray; //Buscamos las referencias en el editor para no perder el orden
	public List<Image> livesArray;

	//UI
	//variables referentes a los elementos del canvas(para posicionar)
	public GameObject startGameCountDown, startGameCountDownB;
	public GameObject bat, batB, batMin, batMinB, batDots, batDotsB, batSec, batSecB;
	public GameObject bat0, bat0B, bat1, bat1B, bat2, bat2B;
	public GameObject score, scoreB;
	public GameObject pause, pauseB, play, playB, start, startB;
	public GameObject timeHour, timeHourB, timeDots, timeDotsB, timeMinute, timeMinuteB, timeAmOrPm, timeAmrOrPmB;
	//referencias de elementos de UI para algunas logicas
    public GameObject batCountDown;
	public Text countMinutes;
	public Text countSeconds;

    public Text scoreText;
    public Text playCountDown;

	public Text hour;
	public Text minute;
	public Text amOrPm;

    //TEST
    //Variables de temas(background)
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
        //TEST 
		//TEMAS
        skyVertices = new Vector2[] { new Vector2(0, 0), new Vector2(0, GetScreenHeight()), new Vector2(GetScreenWidth(), GetScreenHeight()), new Vector2(GetScreenWidth(), 0)};
        //ushort[] rectTriangles = new ushort[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1}; // usando cuatro triangulos(requiere de otro vertice en el medio del rectangulo --> new Vector2(screenwith, screenHeight)
        triangles = new ushort[] { 0, 1, 2, 0, 2, 3 }; //usando 2 triangulos rectangulos

        groundVertices = new Vector2[] { new Vector2(0, 0), new Vector2(0, GetScreenHeight() * 0.7f), new Vector2(GetScreenWidth(), GetScreenHeight() * 0.7f), new Vector2(GetScreenWidth(), 0)};
        roadVertices = new Vector2[] { new Vector2(0, 0), new Vector2((0.4f * GetScreenWidth()), (0.7f * GetScreenHeight())), new Vector2((0.6f * GetScreenWidth()), (0.7f * GetScreenHeight())), new Vector2(GetScreenWidth(), 0)};
        //ushort[] trapTriangles = new ushort[]{0,1,2,1,2,3,2,3,4}; <-- opcion con 3 triangulos en el medio

        sky = new Color(71, 216, 216, 190) / 255;
        ground = new Color(178, 214, 111, 190) / 255; 
        road = new Color(186, 159, 122, 190) / 255; 
    }

    void Start()
    {
        Polygon2D(skyVertices, triangles, sky, 0);
        Polygon2D(groundVertices, triangles, ground, 1);
        Polygon2D(roadVertices, triangles, road, 2);

        FillSprites("Player", centerCar, sideCar);
        FillSprites("Enemy", centerCar, sideCar);
        //FillSprites("Colision", centerColision, sideColision);
        FillImages("Battery", battery);
        //  FillImageArray (livesArray, live);

    }
	//Obtenemos el tamaño de la pantalla en pixeles	
    public Vector2 GetScreenSizeInPixels(){

        Vector2 screenSizeInPixels = new Vector2(Screen.width, Screen.height);
        return screenSizeInPixels;
    }

	//como Unity convierte los pixeles en unidades Unity:
	//tamaño en X en unidades Unity = 10 * screenWidth/screenHeight
	//el tamaño del alto de la pantalla en unidades Unity siempre es igual a 10 sin importar su tamaño en pixeles
	//el tamaño que varia en unidades Unity es el largo(width)

	//Convertimos los pixeles en unidades de Unity(retornamos las COORDENADAS)
	public Vector2 ConvertToUnityUnits(Vector2 v){

		return Camera.main.ScreenToWorldPoint(v);
	}
	//Obtenemos el largo de la pantalla en unidades de Unity
	public float GetScreenWidth(){

        Vector2 screenSize = ConvertToUnityUnits(GetScreenSizeInPixels());
        return screenSize.x * 2; //<-- para obtener la longitud(unidades de Unity)
	}
	//Obtenemos la altura de la pantalla en unidades de Unity
	public float GetScreenHeight(){

        Vector2 screenSize = ConvertToUnityUnits(GetScreenSizeInPixels());
        return screenSize.y * 2;
	}
	//Retornamos la nueva posicion en unidades de Unity, usando como parametro un array con los porcentajes de la ubicacion en la pantalla
	public Vector2 NewPos(float[] pos){

		Vector2 newPos = GetScreenSizeInPixels(); //TEST posiblemente se acceda a una variable para no llamar la funcion a cada rato
		newPos.x *= pos[0];
		newPos.y *= pos[1];
		return ConvertToUnityUnits(newPos);
	}
	//Ubicamos el elemento del UI con su respectiva coordenada
	public void UIElementPos(GameObject go, float[] pos){

		go.transform.position = NewPos(pos);
	}
	//Creacion de los objetos y distribucion
    public void GameObjects(string name,string tag, int sortingOrder, int alpha, int rowLength, int columnLength,float borderX, float borderY, bool srEnable = false, bool isPlayer = false){

		float limitArea = 0.8f;
		Vector2 normalSize = GetNormalScale(rowLength, columnLength, limitArea);
		Vector2 screenSizeInPixel = GetScreenSizeInPixels();
		Vector2 cellPosInPixel = GetCellPos(screenSizeInPixel, rowLength, columnLength, limitArea);
		Vector2 cellSize = new Vector2(GetScreenWidth()/2 - Mathf.Abs(ConvertToUnityUnits(cellPosInPixel).x),
		GetScreenHeight()/2 - Mathf.Abs(ConvertToUnityUnits(cellPosInPixel).y));
		Vector2 border = new Vector2(screenSizeInPixel.x * borderX, screenSizeInPixel.y * borderY);
		cellPosInPixel /= 2; //Ajustamos el pivote en el centro
		Vector2 cellPos = new Vector2(cellPosInPixel.x + border.x, cellPosInPixel.y + border.y);
		cellPos = ConvertToUnityUnits(cellPos);
		Vector2 newPos = cellPos;
		
        for (int i = columnLength -1; i > -1; i--)
        {
			newPos.x = cellPos.x;
            for (int j = 0; j < rowLength; j++)
            {
                GameObject go = new GameObject(name + i.ToString() + '-' + j.ToString());
                go.tag = tag;
                go.AddComponent<SpriteRenderer>();
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.enabled = srEnable;
                sr.sortingLayerName = tag;
                sr.sortingOrder = sortingOrder;
                Color newColor = Color.white;
                newColor.a = (float)alpha/255;
                sr.color = newColor;
                if (j == 0) sr.flipX = true;//pendiente por si hay mas de 3 elementos por fila
				go.transform.localScale = normalSize;
				go.transform.position = newPos;
				newPos.x += cellSize.x;
				if(isPlayer){
					playerArray.Add(sr); //Si estamos creando el player, lo agregamos a la lista playerArray
				}
            }
			if(isPlayer)return; //Con esto evitamos que se creen mas players en otras filas
			newPos.y += cellSize.y;
        }
    }
	//screenLimits es una subpantalla que se obtiene reduciendo la pantalla original
	//obtenemos el area de nuestro sprite dividiendo el area de nuestra subpantalla con el numero total de celdas
	//distribuimos los lados sacando la raiz cuadrada al area(Unity maneja los sprites en cuadrados)
	public Vector2 GetNormalScale(int rowLength, int columnLength, float limitArea){

		Vector2 screenLimits = new Vector2(GetScreenWidth(), GetScreenHeight()) * limitArea;
		Vector2 normalScale = GetSpriteSize(rowLength, columnLength, screenLimits.x, screenLimits.y);
		return normalScale;
	}
	//Obtenemos la posicion de la primera celda(esquina izquierda inferior) en pixeles
	//El pivote se encuentra en la esquina superior derecha
	public Vector2 GetCellPos(Vector2 screenSizeInPixel, int rowLength, int columnLength, float limitArea){

		Vector2 screenLimits = screenSizeInPixel * limitArea; 
		Vector2 cellPos = new Vector2(screenLimits.x/rowLength, screenLimits.y/columnLength); 
		return cellPos;
	}
	//Obtenemos el tamaño del sprite ideal como un cuadrado
	public Vector2 GetSpriteSize(int rowLength, int columnLength, float width, float height){

		float limitsArea = width * height;
		float spriteArea = limitsArea/(rowLength * columnLength);
		Vector2 spriteSize = new Vector2(Mathf.Sqrt(spriteArea), Mathf.Sqrt(spriteArea));
		return spriteSize;
	}

	// //el parametro limitArea hace referencia a cuanto porcentaje de area de la pantalla queremos usar(como limite)
	// public Vector2 GetNormalScale(int rowLength, int columnLength, float limitArea = 0.8f){

	// 	Vector2 screenLimits = new Vector2(GetScreenWidth(), GetScreenHeight()) * limitArea;
	// 	Vector2 normalSize = new Vector2(screenLimits.x/rowLength, screenLimits.y/columnLength);
	// 	return normalSize;
	// }

    public void FillSprites(string tag, Sprite centerSprite, Sprite sideSprite, float scaleP = 0.1f){ //por eliminar

        GameObject[] array = FindGameObjectsByTag(tag);
        Vector2 normalScale = GetNewSpriteScale(scaleP);//TEST(por eliminar si se usa la funcion GetNormalSize())
       
        foreach (GameObject go in array)
        {
            char lastChar = go.name[go.name.Length - 1];//pendiente por si el ultimo numero es mayor a 10
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			//ProportionalScale(go, normalScale);
            if (lastChar == '1')//TEST
            {
                sr.sprite = centerSprite;
            }else{
                sr.sprite = sideSprite;
            }
        }
    }
    //TEST
    private Vector2 GetNewSpriteScale(float p){

        //La siguiente lògica no funciona ya que Unity ignora las proporciones de los sprites
        //float width = sprite.rect.width;
        //float height = sprite.rect.height;
        //float p;
        //Vector2 newScale;
        //if (width > height)
        //{
        //    p = width / height;
        //    newScale = new Vector2(GetScreenSizeInPixels().x * 0.1f, (GetScreenSizeInPixels().x * 0.1f) / p);
        //}
        //else if (height > width)
        //{
        //    p = height / width;
        //    newScale = new Vector2((GetScreenSizeInPixels().y * 0.1f)/p, GetScreenSizeInPixels().y * 0.1f);
        //}
        //else
        //{
        //    newScale = new Vector2(GetScreenSizeInPixels().x, GetScreenSizeInPixels().x) * 0.1f;
        //}
        ////convertimos los pixeles en unidades de unity
        //float pu = sprite.pixelsPerUnit;
        //newScale.x = newScale.x / pu;
        //newScale.y = newScale.y / pu;
        //return newScale;

        Vector2 newScale = Vector2.one * GetScreenWidth() * p;

        return newScale;
    }

    //TEST
    public void ProportionalScale(GameObject go, Vector2 normalScale)
    {
        Vector2 newScale;

        char columnIndex = go.name[1]; //Segundo caracter del nombre del objeto
        switch (columnIndex)
        {
            case '3':
                newScale = normalScale;
                break;
            case '2':
                newScale = normalScale * 0.75f;
                break;
            case '1':
                newScale = normalScale * 0.5f;
                break;
            case '0':
                newScale = normalScale * 0.25f;
                break;
            default:
                newScale = normalScale;
                break;
        }
        go.transform.localScale = newScale; 
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

    public GameObject[] FindGameObjectsByTag(string tag){

        GameObject[] array = GameObject.FindGameObjectsWithTag(tag);
        return array;
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
        Texture2D texture = new Texture2D(Screen.width, Screen.height);

        List<Color> cols = new List<Color>();
        for (int i = 0; i < (texture.width * texture.height); i++)
            cols.Add(color);
        texture.SetPixels(cols.ToArray());
        texture.Apply();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector3.zero, 1);
        //sr.color = color; <--- Al parecer no es necesario ya que duplica el tono del color
        sr.sprite.OverrideGeometry(vertices, triangles);
        Vector2 newPos = new Vector2(-GetScreenWidth(),-GetScreenHeight())/2;
        polygon.transform.position = newPos;
    }

	public void Console(){
		return;
	}

	public void Lives(){
		return;
	}
}
