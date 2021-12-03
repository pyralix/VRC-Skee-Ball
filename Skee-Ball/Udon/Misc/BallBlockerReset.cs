
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class BallBlockerReset : UdonSharpBehaviour
{
    private int _ballCount;
    [SerializeField] SkeeballMain SkeeballMain;
    public void OnTriggerExit(Collider other)
    {
        _ballCount++;
        if (Networking.IsOwner(gameObject))
        {
            SkeeballMain.IncrementBlockCount();
        }
    }

}
