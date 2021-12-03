
using UdonSharp;
using UnityEngine;

public class Score100 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(100);
    }
}
