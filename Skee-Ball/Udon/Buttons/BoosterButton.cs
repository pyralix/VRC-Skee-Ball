
using UdonSharp;
using UnityEngine;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BoosterButton : UdonSharpBehaviour
    {
        [SerializeField] private GameObject BoosterTrigger;
        [SerializeField] private GameObject ButtonLight;
        [SerializeField] private AudioSource OnSound;
        [SerializeField] private AudioSource OffSound;
        [UdonSynced] private bool BoosterLightOn;

        public override void Interact()
        {
            if (!ButtonLight.activeSelf)
            {
                OnSound.Play();
                BoosterLightOn = true;
            }
            else
            {
                OffSound.Play();
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
                OnSound.Play();
                ButtonLight.SetActive(true);
                BoosterTrigger.SetActive(true);
            }
            else
            {
                OffSound.Play();
                ButtonLight.SetActive(false);
                BoosterTrigger.SetActive(false);
            }
        }

    }
}
