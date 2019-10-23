using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionLog : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        print("Enter " + col.gameObject + " collision.");
    }

    void OnCollisionStay(Collision col)
    {
        print(col.gameObject + " collision stay occurring.");
    }

    void OnCollisionExit(Collision col)
    {
        print("Exit " + col.gameObject + " collision.");
    }

    void OnTriggerEnter(Collider trig)
    {
        print("Enter " + trig.gameObject + " trigger.");
    }

    void OnTriggerStay(Collider trig)
    {
        print(trig.gameObject + " trigger stay occurring.");
    }

    void OnTriggerExit(Collider trig)
    {
        print("Exit " + trig.gameObject + " trigger.");
    }
}
