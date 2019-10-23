using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constraints : MonoBehaviour {

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

    private void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myIntrCat = GetComponent<InteractionCategory>();
        rend = GetComponent<Renderer>();
        originalMaterial = rend.material;
        controller = GameObject.FindWithTag("GameController");
    }

    void OnCollisionStay(Collision col)
    {
        InteractionCategory colIntrCat = col.gameObject.GetComponent<InteractionCategory>();

        lockTransformations();

        if (lockOnSurface && colIntrCat.categoryOne)
        {
            lockVertical = true;
            lockRotation = true;
        }

        if (colIntrCat.categoryZero || colIntrCat.categoryOne)
        {
            myRigidBody.constraints = RigidbodyConstraints.None;
            transform.SetParent(col.gameObject.transform, true);

            if (controllerGrab)
                lockTransformations();
        }

        if (myIntrCat.isConstraint && colIntrCat.categoryTwo)
        {
            rend.material = onTriggerMaterial;
        }

    }

    void OnCollisionExit(Collision col)
    {
        InteractionCategory colIntrCat = col.gameObject.GetComponent<InteractionCategory>();

        if (colIntrCat.categoryOne)
        {
            lockVertical = false;
        }
        lockTransformations();

        if (myIntrCat.isConstraint && colIntrCat.categoryTwo)
        {
            rend.material = originalMaterial;
        }

        if (colIntrCat.categoryZero || colIntrCat.categoryOne)
        {
            transform.parent = null;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerObject = other.gameObject;
        InteractionCategory otherIntrCat = other.gameObject.GetComponent<InteractionCategory>();

        if (otherIntrCat != null && otherIntrCat.isConstraint)
        {
            objectInConstraint = true;
            constraintObject = other.gameObject;
        }

        ControllerGrabObject myCtrlGrab = other.gameObject.GetComponent<ControllerGrabObject>();
        if (myCtrlGrab)
        {
            myCtrlGrab.GrabStarted += OnGrabStarted;
            myCtrlGrab.GrabEnded += OnGrabEnded;
        }

        
    }

    void OnTriggerStay(Collider other)
    {
        triggerObject = other.gameObject;
        InteractionCategory otherIntrCat = other.gameObject.GetComponent<InteractionCategory>();

        if (other.gameObject.tag == "GameController")
        {
            if (myIntrCat.categoryTwo)
            {
                myRigidBody.isKinematic = false;
            }
            
            triggerObject = other.gameObject;
            return;
        }
        
        if (otherIntrCat.categoryTwo)
        {
            if (triggerObject.GetComponent<Constraints>().controllerGrab && myIntrCat.isConstraint && GetComponent<Constraints>().onTriggerMaterial != null)
            {
                rend.material = onTriggerMaterial;
            }
            else if (!triggerObject.GetComponent<Constraints>().controllerGrab)
            {
                rend.material = originalMaterial;
            }
        }

        if (otherIntrCat.isConstraint)
        {
            objectInConstraint = true;
            constraintObject = other.gameObject;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractionCategory otherIntrCat = other.gameObject.GetComponent<InteractionCategory>();

        if (other.gameObject.tag == "GameController")
        {
            if (myIntrCat.categoryTwo)
            {
                myRigidBody.isKinematic = true;
            }
            
            return;
        }
        if (otherIntrCat.categoryTwo)
        {
            if (myIntrCat.isConstraint)
            {
                rend.material = originalMaterial;
            }
        }
        if (otherIntrCat.isConstraint)
        {
            objectInConstraint = false;
        }

        ControllerGrabObject myCtrlGrab = other.gameObject.GetComponent<ControllerGrabObject>();
        if (myCtrlGrab)
        {
            myCtrlGrab.GrabStarted -= OnGrabStarted;
            myCtrlGrab.GrabEnded -= OnGrabEnded;
        }

    }

    private void Update()
    {
       
    }

    private void lockTransformations()
    {

        if (lockRotation)
        {
            myRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        if (lockVertical && lockRotation)
        {
            myRigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void snapToConstraintPos(GameObject constraint)
    {
        myRigidBody.constraints = RigidbodyConstraints.None;

        pos = transform.position;
        Vector3 constraintPos = constraint.transform.position;
        float height = GetComponent<Collider>().bounds.size.y;
        float bottomY = GetComponent<Collider>().bounds.min.y;
        float constraintHeight = constraint.GetComponent<Collider>().bounds.size.y;

        myRigidBody.useGravity = false;
        pos.x = constraintPos.x;
        //pos.y = constraintPos.y - constraintHeight/2 + height/2;
        pos.y = constraintPos.y - constraintHeight / 2 + pos.y - bottomY;
        pos.z = constraintPos.z;

        transform.position = pos;
    }

    public void OnGrabStarted()
    {
        if (myIntrCat.categoryTwo && triggerObject.tag == "GameController")
        {
            beingGrabbed = true;
        }
        Debug.Log("On Grab Started");
        if (beingGrabbed && triggerObject.tag == "GameController")
        {
            myRigidBody.isKinematic = false;
            myRigidBody.useGravity = true;

        }
    }

    public void OnGrabEnded()
    {
        if(!controllerGrab && objectInConstraint)
        {
            transform.SetParent(constraintObject.transform, true);
            myRigidBody.useGravity = false;
            myRigidBody.isKinematic = true;
            snapToConstraintPos(constraintObject);
        }
        if (!objectInConstraint)
        {
            myRigidBody.useGravity = true;
        }
    }
}

/**
 * NOTES
 * if button released and object is in trigger
 *      1. make object child of constraint
 *      2 disable gravity
 *      3 set position of object to constraint
 * 
 * cube becomes gravitational and non-kinematic regardless what object is being grabbed - fix
 **/
