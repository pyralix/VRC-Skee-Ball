
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Score30 : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        SkeeballMain.ScorePoints(30);
    }
}
