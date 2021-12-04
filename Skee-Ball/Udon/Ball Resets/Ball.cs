
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

        private Rigidbody rigidBody;
        private VRCObjectSync objectSync;
        private bool isColliding;

        private const string
            HiddenPrefix = "Hidden",
            ChassisPrefix = "Chassis",
            SkeeBallPrefix = "SkeeBall-";

        private void Start()
        {
            rigidBody = (Rigidbody)GetComponent(typeof(Rigidbody));
            objectSync = (VRCObjectSync)GetComponent(typeof(VRCObjectSync));
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
            if (Utilities.IsValid(other) && other != null)
            {
                string otherName = other.gameObject.name;

                if (otherName.Contains(SkeeBallPrefix))
                {
                    BallCollisionSoundSource.Play();
                }

                if (otherName.Contains(HiddenPrefix) || otherName.Contains(ChassisPrefix))
                {
                    isColliding = false;

                    BallRollingSoundSource.Pause();
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if(Utilities.IsValid(other) && other != null)
            {
                string otherName = other.gameObject.name;

                if (otherName.Contains(SkeeBallPrefix))
                {
                    BallCollisionSoundSource.Play();
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
