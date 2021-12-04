
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class StartResetButton : UdonSharpBehaviour
    {
        [SerializeField] SkeeballMain SkeeballMain;
        [SerializeField] private AudioSource OnSound;
        [SerializeField] private AudioSource OffSound;
        [SerializeField] private GameObject PowerLights;
        [SerializeField] private GameObject Button;

        [UdonSynced] private bool ButtonLightOn;

        public override void Interact()
        {
            if (PowerLights.activeSelf)
            {
                ButtonLightOn = false;
                OffSound.Play();
                Button.SetActive(false);
            }
            else
            {
                ButtonLightOn = true;
                OnSound.Play();
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
                OnSound.Play();
                Button.SetActive(true);
            }
            else
            {
                OffSound.Play();
                Button.SetActive(false);
            }
        }
    }
}
