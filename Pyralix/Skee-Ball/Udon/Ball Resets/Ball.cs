
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Ball : UdonSharpBehaviour
    {
        [SerializeField] private AudioSource BallRollingSoundSource;
        [SerializeField] private AudioSource BallCollisionSoundSource;

        private AudioClip collisionClip;
        private Rigidbody rigidBody;
        private VRCObjectSync objectSync;
        private bool isColliding;

        private const string
            HiddenPrefix = "Hidden",
            ChassisPrefix = "Chassis",
            BallPrefix = "Ball";

        private void Start()
        {
            rigidBody = (Rigidbody)GetComponent(typeof(Rigidbody));
            objectSync = (VRCObjectSync)GetComponent(typeof(VRCObjectSync));

            collisionClip = BallCollisionSoundSource.clip;
        }

        private void Update()
        {
            if (isColliding)
            {
                BallRollingSoundSource.volume = Mathf.Clamp01(rigidBody.velocity.magnitude);

                if (rigidBody.IsSleeping())
                {
                    BallRollingSoundSource.Pause();
                    isColliding = false;
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            isColliding = false;

            BallRollingSoundSource.Pause();
        }

        private void OnCollisionEnter(Collision other)
        {
            if(Utilities.IsValid(other) && other != null)
            {
                string otherName = other.gameObject.name;

                if (otherName.Contains(BallPrefix))
                {
                    BallCollisionSoundSource.PlayOneShot(collisionClip, Mathf.Clamp01(other.relativeVelocity.magnitude / 2f));
                }

                if(otherName.Contains(HiddenPrefix) || otherName.Contains(ChassisPrefix))
                {
                    isColliding = true;

                    BallRollingSoundSource.Play();
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
