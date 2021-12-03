
using UdonSharp;
using UnityEngine;

public class Score40 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(40);
    }
}
