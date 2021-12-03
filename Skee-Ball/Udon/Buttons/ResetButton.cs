
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

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
