
using UdonSharp;
using UnityEngine;

public class Score30 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(30);
    }
}
