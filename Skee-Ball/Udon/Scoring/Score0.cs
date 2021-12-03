
using UdonSharp;
using UnityEngine;

public class Score0 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(0);
    }
}
