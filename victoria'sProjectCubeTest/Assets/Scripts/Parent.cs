﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTwo : MonoBehaviour {

    private GameObject childObject;

    private void SetChildObject(Collision col)
    {
        if (!col.gameObject.GetComponent<InteractionCategory>().categoryTwo)
        {
            return;
        }

        childObject = col.gameObject;
    }

    void OnCollisionEnter(Collision other)
    {
        SetChildObject(other);
    }

    void OnCollisionStay(Collision other)
    {
        SetChildObject(other);
    }

    private void ParentObject()
    {
        var joint = AddFixedJoint();
        joint.connectedBody = childObject.GetComponent<Rigidbody>();
    }
	
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }
	
}