
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ResetButton : UdonSharpBehaviour
    {
        [SerializeField] private SkeeballMain SkeeballMain;
        [SerializeField] private AudioSource audio;
        [SerializeField] private GameObject PowerButtonLight;

        public override void Interact()
        {
            SkeeballMain._Reset(Networking.LocalPlayer);
            PowerButtonLight.SetActive(false);
            audio.Play();
        }
    }
}
