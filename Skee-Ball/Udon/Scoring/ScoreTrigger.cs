
using UdonSharp;
using UnityEngine;

namespace Pyralix.SkeeBall
{
    public class ScoreTrigger : UdonSharpBehaviour
    {
        [SerializeField] int Points;

        [SerializeField] SkeeballMain SkeeballMain;

        public void OnTriggerExit(Collider other)
        {
            SkeeballMain.ScorePoints(Points);
        }
    }
}
