
using UdonSharp;
using UnityEngine;

public class Score10 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(10);
    }
}
