
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AddForce : UdonSharpBehaviour
{
    //this function is called every time the collider of the gameobject to which this script is attached...
    //...collides with another collider
    void OnTriggerEnter(Collider obj)
    {
            obj.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 450f);
    }
}
