
using UdonSharp;
using UnityEngine;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ScoreTrigger : UdonSharpBehaviour
    {
        [SerializeField] private int Points;

        [SerializeField] private SkeeballMain SkeeballMain;

        public void OnTriggerExit(Collider other)
        {
            SkeeballMain._ScorePoints(Points);
        }
    }
}
