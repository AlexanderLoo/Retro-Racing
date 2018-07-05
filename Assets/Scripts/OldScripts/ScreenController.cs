using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour {

	public static ScreenController screen;
	//IMPORTANTE: NO es la longitud del ancho y del alto de la pantalla, es la posición de los extremos positivos(para obtener la longitud total multiplicar x2)
	public float screenWidth, screenHeight;

	void Awake(){
		//Singleton
		if (screen == null) {
			screen = this;
		}
		else if (screen != this) {
			Destroy (gameObject);
		}
		//Convertimos el tamaño de la pantalla de pixeles a medidas de Unity
		Vector2 screenSizeInPixels = new Vector2 (Screen.width, Screen.height);
		Vector2 screenSize = Camera.main.ScreenToWorldPoint (screenSizeInPixels);
		screenWidth = screenSize.x;
		screenHeight = screenSize.y;
	}
    //TEST https://answers.unity.com/questions/1411572/how-do-i-draw-simple-shapes.html
    void Start()
    {
        //Para un rectangulo
        Vector2[] rectVertices = new Vector2[] { new Vector2(0, 0), new Vector2(0, screenHeight)*2, new Vector2(screenWidth, screenHeight)*2, new Vector2(screenWidth, 0)*2, new Vector2(screenWidth, screenHeight)};
        //ushort[] rectTriangles = new ushort[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1};
        ushort[] rectTriangles = new ushort[] { 0, 1, 2, 0, 2, 3 }; //<- otra opción usando 2 triangulos rectangulos

         //TEST COLOR TEMAS
         Color sky = new Color(154, 202, 231, 255) / 255; //Ford Desert Sky Blue
         Color ground = new Color(225, 169, 95, 255) / 255; //Yellow Earth Color
         Color road = new Color(132, 115, 90, 255) / 255; //Cement Color

        DrawPolygon2D(rectVertices, rectTriangles, sky, 0);
    }

    void DrawPolygon2D(Vector2[] vertices, ushort[] triangles, Color color, int sortingOrder)
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

        sr.color = color;

        sr.sprite = Sprite.Create(texture, new Rect(0, 0, 1024, 1024), Vector3.zero, 1);
        sr.sprite.OverrideGeometry(vertices, triangles);
        Vector2 newPos = new Vector2(-screenWidth, -screenHeight);
        polygon.transform.position = newPos;
    }
}
