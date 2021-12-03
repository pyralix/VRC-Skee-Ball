
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

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
