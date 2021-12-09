
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class StartResetButton : UdonSharpBehaviour
    {
        [SerializeField] SkeeballMain SkeeballMain;
        [SerializeField] private AudioSource Speaker;
        [SerializeField] private AudioClip OnSound;
        [SerializeField] private AudioClip OffSound;
        [SerializeField] private GameObject PowerLights;
        [SerializeField] private GameObject Button;

        [UdonSynced] private bool ButtonLightOn;

        public override void Interact()
        {
            if (PowerLights.activeSelf)
            {
                ButtonLightOn = false;
                Speaker.PlayOneShot(OffSound, SkeeballMain._AudioVolume);
                Button.SetActive(false);
            }
            else
            {
                ButtonLightOn = true;
                Speaker.PlayOneShot(OnSound, SkeeballMain._AudioVolume);
                Button.SetActive(true);
            }
            RequestSerialization();
            SkeeballMain._SetGameOwnerAndTogglePower(Networking.LocalPlayer);
        }

        public void _TurnOffButtonLight()
        {
            Button.SetActive(false);
            ButtonLightOn = false;
            RequestSerialization();
        }

        public override void OnDeserialization()
        {
            if (ButtonLightOn)
            {
                Speaker.PlayOneShot(OnSound, SkeeballMain._AudioVolume);
                Button.SetActive(true);
            }
            else
            {
                Speaker.PlayOneShot(OffSound, SkeeballMain._AudioVolume);
                Button.SetActive(false);
            }
        }
    }
}
