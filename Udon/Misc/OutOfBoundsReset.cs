
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class OutOfBoundsReset : UdonSharpBehaviour
    {
        //this function is called every time the collider of the gameobject to which this script is attached...
        //...collides with another collider
        private void OnTriggerEnter(Collider obj)
        {
            if (Utilities.IsValid(obj) && obj != null && obj.GetComponent<Ball>())
            {
                obj.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                obj.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            }
        }
        
        private void OnTriggerExit(Collider obj)
        {
            if (Utilities.IsValid(obj) && obj != null && obj.GetComponent<Ball>())
            {
                obj.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
