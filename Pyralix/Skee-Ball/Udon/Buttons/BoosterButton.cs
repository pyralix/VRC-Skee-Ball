
using UdonSharp;
using UnityEngine;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BoosterButton : UdonSharpBehaviour
    {
        [SerializeField] private SkeeballMain SkeeballMain;
        [SerializeField] private GameObject BoosterTrigger;
        [SerializeField] private GameObject ButtonLight;
        [SerializeField] private AudioSource Speaker;
        [SerializeField] private AudioClip OnSound;
        [SerializeField] private AudioClip OffSound;
        [UdonSynced] private bool BoosterLightOn;

        public override void Interact()
        {
            if (!ButtonLight.activeSelf)
            {
                Speaker.PlayOneShot(OnSound, SkeeballMain._AudioVolume);
                BoosterLightOn = true;
            }
            else
            {
                Speaker.PlayOneShot(OffSound, SkeeballMain._AudioVolume);
                BoosterLightOn = false;
            }
            RequestSerialization();
            BoosterTrigger.SetActive(!BoosterTrigger.activeSelf);
            ButtonLight.SetActive(!ButtonLight.activeSelf);
        }

        public override void OnDeserialization()
        {
            if (BoosterLightOn)
            {
                Speaker.PlayOneShot(OnSound, SkeeballMain._AudioVolume);
                ButtonLight.SetActive(true);
                BoosterTrigger.SetActive(true);
            }
            else
            {
                Speaker.PlayOneShot(OffSound, SkeeballMain._AudioVolume);
                ButtonLight.SetActive(false);
                BoosterTrigger.SetActive(false);
            }
        }

    }
}
