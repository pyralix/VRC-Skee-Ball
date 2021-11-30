
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class BoosterButton : UdonSharpBehaviour
{
    public GameObject BoosterTrigger;
    public GameObject ButtonLight;
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
