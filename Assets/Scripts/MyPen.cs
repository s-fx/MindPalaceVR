using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MyPen : XRGrabInteractable
{

 	public XRController controller;
 	public Material drawColorMaterial;
	private DrawableObject drawableBoard;
	private RaycastHit raycastHit;
	private bool lastTouch;
	private Quaternion lastAngle;
	private int penSize = 5;
	private int eraseSize = 20;
	public GameObject penHead;

	private Color penColor = Color.white;

	public enum Modes {
		Draw,	// 0
		Eraser,	// 1
	    Shape,   // 2, currently not working properly
        ClearTexture // 3, currently not working properly
    }

	private Modes mode = Modes.Draw;

    // public XRRayInteractor visualLine;
    // private bool showLine = false;
    // public InputHelpers.Button drawShapeActivationButton;
    // public float activationThreshold = 0.1f;
    // public XRController controller;
    // private RaycastHit drawRayHit;

    public XRRayInteractor xrRay;

	// rewrite Code, maybe check for which mode is selected first....
	

    // Start is called before the first frame update
    void Start()
    {
    	this.drawableBoard = GameObject.Find("DrawableObject").GetComponent<DrawableObject> (); 
    }

    // Update is called once per frame
    void Update()
    {

        // should be 0.015 look at PenHead scale on the y axis
        float penHeadHeight = 0.015f;    
        // get position of the head of the pen
        Vector3 penHead = transform.Find("PenHead").transform.position;

        // not working...
        // the visualLine is showing but not reacting to input of controller... 
        // if (showLine) {
        //     visualLine.gameObject.SetActive(true);
        //     if(CheckIfDrawShapesButtonIsPressed(controller)) {
        //         if(visualLine.GetCurrentRaycastHit(out drawRayHit)){
        //             if(drawRayHit.collider.tag == "drawableBoard") {
        //                 this.drawableBoard = drawRayHit.collider.GetComponent<drawableBoard> ();
        //                 this.drawableBoard.SetTouchPosition(drawRayHit.textureCoord.x, drawRayHit.textureCoord.y);
        //                 this.drawableBoard.IsTouching(true);
        //                 this.drawableBoard.SetColor(Color.red); 
        //                 this.drawableBoard.SetPenSize(penSize);
        //                 this.drawableBoard.SetMode(0);
        //                 if (!lastTouch) {
        //                     lastTouch = true;
        //                 }
        //             } else {
        //                 return;
        //             }
                    
        //         } else {
        //             this.drawableBoard.IsTouching(false);
        //             lastTouch = false;
        //         }
        //     } else {
        //         visualLine.gameObject.SetActive(false);
        //     } 
        // }
    
        /////////

        // Physics.Raycast return true if it intersects with a collider
        // penHead = origin; transform.up = direction of ray, penHeadHeight = maxDistance 
        if (Physics.Raycast(penHead,transform.up,out raycastHit, penHeadHeight)) {
        	if (raycastHit.collider.tag == "DrawableObject"){

        		this.drawableBoard = raycastHit.collider.GetComponent<DrawableObject> ();
        		if (mode == Modes.Draw) {
        			this.drawableBoard.SetPenSize(penSize);
        			this.drawableBoard.SetMode(0);	
        		} else if (mode == Modes.Eraser) {
        			SetMyPenColor(Color.clear); 
        			this.drawableBoard.SetPenSize(eraseSize);
        			this.drawableBoard.SetMode(1);	
        		// } else if (mode == Modes.Shape) {
          //           this.drawableBoard.SetMode(2);
          //           this.drawableBoard.SetPenSize(penSize);
          //           // LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
          //           // lineRenderer.widthMultiplier = 0.2f;
          //           // lineRenderer.positionCount = 2;
          //           // Vector3[] ArrayWithPositions = new Vector3[2];
          //           // ArrayWithPositions[0] = penHead;
          //           // ArrayWithPositions[1] = raycastHit.point;
          //           // lineRenderer.SetPositions(ArrayWithPositions);
                } else if (mode == Modes.ClearTexture) {
                    this.drawableBoard.SetMode(3);
                }
        		// not working right now
        		controller.SendHapticImpulse(0.9f,5f);
        		
				this.drawableBoard.SetColor(penColor);		
        		this.drawableBoard.SetTouchPosition(raycastHit.textureCoord.x, raycastHit.textureCoord.y);
        		this.drawableBoard.IsTouching(true);

        		if (!lastTouch){
        			lastTouch = true;
        			lastAngle = transform.rotation; 
        		}

        	} else {
                // if collider is not in the family DrawableObject do nothing
        		return;
        	}
        	
        } else if (mode == Modes.Shape) { // Framerate dropping rapidly when in this mode....
            if(xrRay.GetCurrentRaycastHit(out RaycastHit xrRayHit)){
                if(xrRayHit.collider.tag == "DrawableObject"){
                    this.drawableBoard = xrRayHit.collider.GetComponent<DrawableObject>();
                    this.drawableBoard.SetMode(2);
                    this.drawableBoard.SetPenSize(penSize);
                    this.drawableBoard.SetColor(penColor);
                    this.drawableBoard.SetHitDistance(xrRayHit.distance);
                    this.drawableBoard.SetTouchPosition(xrRayHit.textureCoord.x,xrRayHit.textureCoord.y);
                    this.drawableBoard.IsTouching(true);
                }
            }
        } else {
            // Raycast is not touching drawableBoard anymore
            this.drawableBoard.IsTouching(false);
            lastTouch = false;
        }

        if (lastTouch) {
        	transform.rotation = lastAngle; // pen rotation not locked anymore
        }
    }


    // new Color try gets called in UI
    public void SetMyPenColor(Color penColor) {
    	this.penColor = penColor;
    }

    // gets called in UI
    public void SetPenSize(int penSize) {
    	this.penSize = penSize;
    }

    // gets called in UI
    public void SetEraseSize(int eraseSize) {
    	this.eraseSize = eraseSize;
    }

    // sets Material color of pen Head to currently selected color
    public void SetPenHeadColor(Color penHeadColor) {
    	this.penHead.GetComponent<MeshRenderer>().material.color = penHeadColor;
    }

    public void SetMode(int mode_) {
    	if (mode_ == 0) {
    		this.mode = Modes.Draw;
    	} else if (mode_ == 1) {
    		this.mode = Modes.Eraser;
    	} else if (mode_ == 2) {
            this.mode = Modes.Shape;
        } else if (mode_ == 3) {
            this.mode = Modes.ClearTexture;
        }
    }


    // public void SetRayInteractor(bool showLine) {
    //     this.showLine = showLine;
    // }

    // public bool CheckIfDrawShapesButtonIsPressed(XRController controller) {
    //     InputHelpers.IsPressed(controller.inputDevice, drawShapeActivationButton, out bool isActivated, activationThreshold);
    //     return isActivated;
    // }



}
