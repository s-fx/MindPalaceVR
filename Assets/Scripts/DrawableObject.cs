using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DrawableObject : MonoBehaviour
{


	// drawing on the texture of the drawableObject
    // play around with the numbers, but increasing textureSize will make the framerates drop on quest...
    private int textureSizeWidth = 2401;
    private int textureSizeHeight = 2401;
	private int penSize;      
	private Texture2D texture;
 

   
	private Color[] color;     // array because we set multiple pixel
	private Color[] eraserColor; 
	private int mode; // erase (1) or draw (0)...

    // currently touching board? touchinglast were we touching on hte last frame?
	private bool touching, touchingLast;
	// pos of touch on texture
	private float posTextureX, posTextureY;
    // last touch position
	private float lastTouchingX, lastTouchingY;

    // for draw shapes
    private float rayCastHitDistance = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
    	Renderer renderer = GetComponent<Renderer> ();
    	this.texture = new Texture2D (textureSizeWidth, textureSizeHeight,TextureFormat.RGBA32,true); // format so alpha is taken into account
    	renderer.material.mainTexture = this.texture;
    	eraserColor = this.texture.GetPixels(); // get the color for erasing...
    }

    // Update is called once per frame
    void Update()
    {
        // pixel values
        int x = (int) (posTextureX * textureSizeWidth - (penSize / 2));
        int y = (int) (posTextureY * textureSizeHeight - (penSize / 2));

        if(touchingLast){
        	if (mode == 0) {
        		texture.SetPixels(x,y,penSize,penSize,color);
                // linear interpolation, because some pixels are missing (no continious line when drawing fast)
                // with t as interpolation point
                // returns a value between our last touching point on texture and the first touching point
        		for (float t = 0.01f; t < 1.00f; t += 0.01f) {
              		int lerpX = (int) Mathf.Lerp(lastTouchingX, (float) x, t);
              		int lerpY = (int) Mathf.Lerp(lastTouchingY, (float) y, t);
              		texture.SetPixels(lerpX,lerpY,penSize,penSize,color);
        		} 
        	}
        	if (mode == 1) {
        		texture.SetPixels(x,y,penSize,penSize,eraserColor);
                // linear interpolation
        		for (float t = 0.01f; t < 1.00f; t += 0.01f) {
                	int lerpX = (int) Mathf.Lerp(lastTouchingX, (float) x, t);
                	int lerpY = (int) Mathf.Lerp(lastTouchingY, (float) y, t);
                	texture.SetPixels(lerpX,lerpY,penSize,penSize,eraserColor);
        		}
        	}
            if (mode == 2) {
                // draw rectangle
                DrawRectangle(x,y,texture,rayCastHitDistance);
            }
            if (mode == 3) {
                // when increasing size it is somehow not working
                // deleting the whole texture makes the app crash
                texture.SetPixels(x,y,500,500,eraserColor);
            }	
        	texture.Apply();
        	color = null;
        }

        this.lastTouchingX = (float)x;
        this.lastTouchingY = (float)y;

        this.touchingLast = this.touching;
    }


    public void IsTouching(bool touching){
    	this.touching = touching;
    }

    public void SetTouchPosition(float x, float y){
    	this.posTextureX = x;
    	this.posTextureY = y;
    }

    public void SetPenSize(int size){
    	this.penSize = size;
    }

    public void SetMode(int mode){
    	this.mode = mode;
    }

    public void SetColor(Color color){
    	// (color = result element, penSize * penSize = size of color[] see above)
    	this.color = Enumerable.Repeat<Color>(color, penSize * penSize).ToArray<Color> ();
    }

    public void SetHitDistance(float distance) {
        this.rayCastHitDistance = distance;
    }

    // choosen rectangle because easiest implementation, more shapes will come...
    // The idea is to get the distance of the raycast and multiply f.ex the radius of a
    // circle with that value, so the size will change if the controller is further away
    // from the drawableObject
    // right now the collision is detected, but somehow I have still problems to set the
    // pixels on the drawableObject
    // draw should be on button release, DrawShapesController.cs
    public void DrawRectangle(float posTextureX, float posTextureY, Texture2D texture, float distance) {
        int dis = (int)distance*100 + 100;
        for (int y = (int) posTextureY; y < (int)posTextureY + dis; y++ ) {
            for (int x = (int) posTextureX; x < (int) posTextureX + dis; x++) {
                if (y == posTextureY || y == posTextureY + dis-1) {
                    texture.SetPixels(x,y,penSize,penSize,color);
                } else if (y > posTextureY && y < posTextureY + dis-1) {
                    if (x == posTextureX || x == posTextureX + dis-1) {
                        texture.SetPixels(x,y,penSize,penSize,color);
                    }
                }
            }
        }
        texture.Apply();
    }


    
}
