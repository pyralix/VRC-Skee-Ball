
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class ResetButton : UdonSharpBehaviour
{
    [SerializeField] SkeeballMain SkeeballMain;
    [SerializeField] private AudioSource audio;
    public GameObject PowerButtonLight;
    public override void Interact()
    {
        SkeeballMain.Reset(Networking.LocalPlayer);
        PowerButtonLight.SetActive(false);
        audio.Play();
    }
}
