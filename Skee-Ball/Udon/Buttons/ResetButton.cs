
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    public class ResetButton : UdonSharpBehaviour
    {
        [SerializeField] SkeeballMain SkeeballMain;
        [SerializeField] private AudioSource audio;
        public GameObject PowerButtonLight;
        public override void Interact()
        {
            SkeeballMain._Reset(Networking.LocalPlayer);
            PowerButtonLight.SetActive(false);
            audio.Play();
        }
    }
}
