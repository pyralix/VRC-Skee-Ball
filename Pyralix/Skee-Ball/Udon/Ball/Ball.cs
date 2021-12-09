
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Ball : UdonSharpBehaviour
    {
        [SerializeField] private AudioSource Speaker;
        [SerializeField] private AudioClip BallCollisionClip;
        [SerializeField] private bool HitSoundEnabled;
        private VRCObjectSync objectSync;
        private bool isColliding;

        private const string
            BallPrefix = "Ball";

        private void Start()
        {
            objectSync = (VRCObjectSync)GetComponent(typeof(VRCObjectSync));
        }
        

        private void OnCollisionEnter(Collision other)
        {
            if(Utilities.IsValid(other) && other != null)
            {
                string otherName = other.gameObject.name;

                if (otherName.Contains(BallPrefix) && HitSoundEnabled)
                {
                    Speaker.PlayOneShot(BallCollisionClip, Mathf.Clamp01(other.relativeVelocity.magnitude / 2f));
                }
            }
        }

        public void _ResetBall()
        {
            Networking.SetOwner(Networking.LocalPlayer, objectSync.gameObject);
            objectSync.Respawn();
        }
    }
}
