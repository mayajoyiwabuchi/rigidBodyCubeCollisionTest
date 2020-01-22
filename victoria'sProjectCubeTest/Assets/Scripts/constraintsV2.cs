using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constraintsV2 : MonoBehaviour { 
public bool lockOnSurface;

private bool lockRotation;
private bool lockVertical;
public bool controllerGrab;
bool objectInConstraint;
private GameObject constraintObject;

bool beingGrabbed;
GameObject triggerObject;

Vector3 pos;
Vector3 scale;

private Rigidbody myRigidBody;
private InteractionCategory myIntrCat;
private Renderer rend;

public Material onTriggerMaterial;
private Material originalMaterial;

GameObject controller;

GameObject colliding; 

    // Start is called before the first frame update
    void Start()
    {
         
            }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "objectProp")
            lockVertical = true; 
    }



}
