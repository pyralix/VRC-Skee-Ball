
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    public class StartResetButton : UdonSharpBehaviour
    {
        [SerializeField] SkeeballMain SkeeballMain;
        [SerializeField] private AudioSource OnSound;
        [SerializeField] private AudioSource OffSound;
        public GameObject PowerLights;
        public GameObject Button;
        [UdonSynced]
        private bool ButtonLightOn;

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
            SkeeballMain.SetGameOwnerAndTogglePower(Networking.LocalPlayer);
        }

        public void TurnOffButtonLight()
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
