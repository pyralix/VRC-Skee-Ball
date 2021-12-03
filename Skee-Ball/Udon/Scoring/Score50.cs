
using UdonSharp;
using UnityEngine;

public class Score50 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(50);
    }
}
