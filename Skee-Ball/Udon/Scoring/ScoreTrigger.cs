
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ScoreTrigger : UdonSharpBehaviour
    {
        [SerializeField] private int Points;

        [SerializeField] private SkeeballMain SkeeballMain;

        public void OnTriggerExit(Collider other)
        {
            if(Utilities.IsValid(other) && other != null && other.GetComponent<Ball>())
            {
                SkeeballMain._ScorePoints(Points);
            }
        }
    }
}
