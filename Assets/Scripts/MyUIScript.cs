using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MyUIScript : MonoBehaviour
{

    public MyPen pen;

    public Slider penSizeSlider;

    public Slider eraseSizeSlider;
    
    public Text penSizeText;

    public Text eraseSizeText;

    public Button drawMode;

    public Button eraseMode;

    public Dropdown colorPicker;

    public Dropdown skyBoxPicker;

    public Button shapesButton;

    public Material morningSkyMaterial;

    public Material universeSkyMaterial;

    public Material milkywaySkyMaterial;

    public Material mountainSkyMaterial;

    public Button clearTextureButton;

    public DrawShapesController drawShapesController; // look at XR Rig


    // Start is called before the first frame update
    void Start()
    {

        penSizeSlider.onValueChanged.AddListener((value) => {
            penSizeText.text = $"Pen Size: {value}";
            pen.SetPenSize((int)value);
            });

        eraseSizeSlider.onValueChanged.AddListener((value) => {
            eraseSizeText.text = $"Erase Size: {value}";
            pen.SetEraseSize((int)value);
            });

        drawMode.onClick.AddListener(() => {
            //pen.SetRayInteractor(false);
            pen.SetMode(0);
            eraseMode.GetComponent<Image>().color = Color.white;
            drawMode.GetComponent<Image>().color = Color.green;
            drawShapesController.SetDrawModeEnabled(false);
            });

        eraseMode.onClick.AddListener(() => {
            //pen.SetRayInteractor(false);
            pen.SetMode(1);
            drawMode.GetComponent<Image>().color = Color.white;
            eraseMode.GetComponent<Image>().color = Color.green;
            drawShapesController.SetDrawModeEnabled(false);
            });

        colorPicker.onValueChanged.AddListener(delegate {
            DropdownValueChanged(colorPicker);
            });

        shapesButton.onClick.AddListener(() => {
            pen.SetMode(2);
            drawShapesController.SetDrawModeEnabled(true);
            });


        skyBoxPicker.onValueChanged.AddListener(delegate {
        	SkyBoxValueChanged(skyBoxPicker);
        	});

        clearTextureButton.onClick.AddListener(() => {
            pen.SetMode(3);
            });

    }

    void SkyBoxValueChanged(Dropdown skyBoxPicker) {
    	if (skyBoxPicker.value == 0) {
    		RenderSettings.skybox = universeSkyMaterial;
    	} else if (skyBoxPicker.value == 1) {
    		RenderSettings.skybox = morningSkyMaterial;
    	} else if (skyBoxPicker.value == 2) {
    		RenderSettings.skybox = milkywaySkyMaterial;
    	} else if (skyBoxPicker.value == 3) {
            RenderSettings.skybox = mountainSkyMaterial;
        } else {
            RenderSettings.skybox = universeSkyMaterial;
        }
    }

    void DropdownValueChanged(Dropdown colorPicker) {
        if (colorPicker.value == 0) {
            pen.SetMyPenColor(Color.white);
            pen.SetPenHeadColor(Color.white);
        } else if (colorPicker.value == 1){
            pen.SetMyPenColor(Color.black);
            pen.SetPenHeadColor(Color.black);
        } else if (colorPicker.value == 2) {
            pen.SetMyPenColor(Color.blue);
            pen.SetPenHeadColor(Color.blue);
        } else if (colorPicker.value == 3) {
            pen.SetMyPenColor(Color.green);
            pen.SetPenHeadColor(Color.green);
        } else if (colorPicker.value == 4) {
            pen.SetMyPenColor(Color.yellow);
            pen.SetPenHeadColor(Color.yellow);
        } else if (colorPicker.value == 5) {
            pen.SetMyPenColor(Color.red);
            pen.SetPenHeadColor(Color.red);
        } else {
            pen.SetMyPenColor(Color.white);
            pen.SetPenHeadColor(Color.white);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

