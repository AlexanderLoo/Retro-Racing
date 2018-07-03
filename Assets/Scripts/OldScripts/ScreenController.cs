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
        //Para un cuadrado
        Vector2[] vertices = new Vector2[] { new Vector2(0, 0), new Vector2(0, 2), new Vector2(2, 2), new Vector2(2, 0), new Vector2(1, 1)};
        ushort[] triangles = new ushort[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 1};
        DrawPolygon2D(vertices, triangles, Color.red);
    }

    void DrawPolygon2D(Vector2[] vertices, ushort[] triangles, Color color)
    {
        GameObject polygon = new GameObject(); //create a new game object
        SpriteRenderer sr = polygon.AddComponent<SpriteRenderer>(); // add a sprite renderer
        sr.sortingLayerName = "Background";
        Texture2D texture = new Texture2D(1025, 1025); // create a texture larger than your maximum polygon size

        // create an array and fill the texture with your color
        List<Color> cols = new List<Color>();
        for (int i = 0; i < (texture.width * texture.height); i++)
            cols.Add(color);
        texture.SetPixels(cols.ToArray());
        texture.Apply();

        sr.color = color; //you can also add that color to the sprite renderer

        sr.sprite = Sprite.Create(texture, new Rect(0, 0, 1024, 1024), Vector2.zero, 1); //create a sprite with the texture we just created and colored in

        //convert coordinates to local space
        float lx = Mathf.Infinity, ly = Mathf.Infinity;
        foreach (Vector2 vi in vertices)
        {
            if (vi.x < lx)
                lx = vi.x;
            if (vi.y < ly)
                ly = vi.y;
        }
        Vector2[] localv = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            localv[i] = vertices[i] - new Vector2(lx, ly);
        }

        sr.sprite.OverrideGeometry(localv, triangles); // set the vertices and triangles

        polygon.transform.position = (Vector2)transform.InverseTransformPoint(transform.position) + new Vector2(lx, ly); // return to world space
    }
}
