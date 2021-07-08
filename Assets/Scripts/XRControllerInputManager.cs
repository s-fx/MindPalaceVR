using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerInputManager : MonoBehaviour
{

	public XRController rightController;
	public InputHelpers.Button button;
	public GameObject canvas;
	private bool isShowing = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // used to show the UI canvas on primary button press
    // Update is called once per frame
    void Update()
    {
    	bool isPressed = false;
    	rightController.inputDevice.IsPressed(button, out isPressed);

    	if (isPressed) {
			isShowing = !isShowing;
    		canvas.SetActive(isShowing);
    	} 
    }
}
