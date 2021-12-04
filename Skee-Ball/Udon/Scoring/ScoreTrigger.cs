
using UdonSharp;
using UnityEngine;

namespace Pyralix.SkeeBall
{
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
