using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrawShapesController : MonoBehaviour
{

    // script is on the XR Rig
    public XRController leftController;
    public InputHelpers.Button drawShapeActivationButton;
    public float activationThreshold = 0.1f;
    private bool isEnabled = false;

    // Update is called once per frame
    void Update()
    {
        leftController.gameObject.SetActive(CheckIfActivated(leftController) && isEnabled);
    }

    public bool CheckIfActivated(XRController controller) {
        InputHelpers.IsPressed(controller.inputDevice, drawShapeActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }

    // gets called in UI
    public void SetDrawModeEnabled(bool isEnabled) {
        this.isEnabled = isEnabled;
    }
}
