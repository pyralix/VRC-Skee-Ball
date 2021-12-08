
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AddForceTowardTarget : UdonSharpBehaviour
    {
        [SerializeField] private GameObject _Target;
        [SerializeField] private float _Multiplier;
        //this function is called every time the collider of the gameobject to which this script is attached...
        //...collides with another collider
        private void OnTriggerEnter(Collider obj)
        {
            if (Utilities.IsValid(obj) && obj != null && obj.GetComponent<Ball>())
            {
                obj.gameObject.GetComponent<Rigidbody>().AddForce((_Target.transform.position - transform.position) * _Multiplier);
            }
        }
    }
}
