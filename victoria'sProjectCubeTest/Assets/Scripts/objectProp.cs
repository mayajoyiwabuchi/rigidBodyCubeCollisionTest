using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectProp : MonoBehaviour
{
    public int health = 10; 
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

    public void saySomething() {
        Debug.Log("Hi I'm an object!"); 
    }

    
}
