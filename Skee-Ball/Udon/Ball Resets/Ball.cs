
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

namespace Pyralix.SkeeBall
{
    public class Ball : UdonSharpBehaviour
    {
        [SerializeField] private AudioSource BallRollingSoundSource;
        [SerializeField] private AudioSource BallCollisionSoundSource;

        private float speed;
        private Rigidbody rigidBody;

        private void Start()
        {
            rigidBody = (Rigidbody)this.GetComponent(typeof(Rigidbody));
        }

        private void FixedUpdate()
        {
            speed = rigidBody.velocity.magnitude;
        }

        private void OnCollisionStay(Collision other)
        {
            if (!BallRollingSoundSource.isPlaying && speed >= 0.1f && (other.gameObject.name.Contains("Hidden") || other.gameObject.name.Contains("Chassis")))
            {
                BallRollingSoundSource.Play();
            }
            else if (BallRollingSoundSource.isPlaying && speed < 0.1f && (other.gameObject.name.Contains("Hidden") || other.gameObject.name.Contains("Chassis")))
            {
                BallRollingSoundSource.Pause();
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (BallRollingSoundSource.isPlaying && (other.gameObject.name.Contains("Hidden") || other.gameObject.name.Contains("Chassis")))
            {
                BallRollingSoundSource.Pause();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name.Contains("SkeeBall-"))
            {
                BallCollisionSoundSource.Play();
            }
        }

        public void _Now()
        {
            if (Networking.IsOwner(gameObject))
            {
                VRCObjectSync obj = (VRCObjectSync)GetComponent(typeof(VRCObjectSync));
                if (Utilities.IsValid(obj))
                {
                    Networking.SetOwner(Networking.LocalPlayer, obj.gameObject);
                    obj.Respawn();
                }
            }
        }
    }
}
