using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class ContinuousMovement : MonoBehaviour
{
    // controll speed of character
    public float speed = 1;
    public XRNode inputSource;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;
    
    public float gravity = -9.81f;
    private float fallingSpeed;
    private XRRig rig;
    private Vector2 inputAxis;
    private CharacterController character;
    
    
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
       // Access device using XRNode
       InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
       // listen for input
       device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }
    
    // actual movement of character
    private void FixedUpdate()
    {
    	CapsuleFollowHeadset();
        // for movement facing our direction we need to multiply vectoe3 by headyaw
    	Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y,0);
    	Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
    	
    	character.Move(direction * Time.fixedDeltaTime * speed);
    	
    	// gravity when character jumps off something (just in case haha)
    	bool isGrounded = CheckIfGrounded();
    	if (isGrounded) 
    		fallingSpeed = 0;
    	else	
    		fallingSpeed += gravity * Time.fixedDeltaTime;
    		
    	character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    	
    }
    
    
    void CapsuleFollowHeadset()
    {
    	character.height = rig.cameraInRigSpaceHeight + additionalHeight;
    	Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
    	character.center = new Vector3(capsuleCenter.x, character.height/2, capsuleCenter.z);
    }
    
    bool CheckIfGrounded()
    {
    	// tell us if on ground
    	Vector3 rayStart = transform.TransformPoint(character.center);
    	float rayLength = character.center.y + 0.01f;
    	bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    	return hasHit;
    	
    }
}
