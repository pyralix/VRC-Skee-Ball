
using UdonSharp;
using UnityEngine;

public class Score20 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(20);
    }
}
