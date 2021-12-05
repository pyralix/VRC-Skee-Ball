
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AddForce : UdonSharpBehaviour
    {
        //this function is called every time the collider of the gameobject to which this script is attached...
        //...collides with another collider
        private void OnTriggerEnter(Collider obj)
        {
            if (Utilities.IsValid(obj) && obj != null && obj.GetComponent<Ball>())
            {
                obj.gameObject.GetComponent<Rigidbody>().AddForce(obj.gameObject.GetComponent<Rigidbody>().velocity.normalized * Time.deltaTime * 10000f);
            }
        }
    }
}
