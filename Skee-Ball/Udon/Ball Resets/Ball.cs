
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;

public class Ball : UdonSharpBehaviour
{
    private float speed;
    private Rigidbody rigidBody;
    public AudioSource BallRollingSoundSource;
    public AudioSource BallCollisionSoundSource;

    public void Start()
    {
        rigidBody = (Rigidbody)this.GetComponent(typeof(Rigidbody));
    }

    public void FixedUpdate()
    {
        speed = rigidBody.velocity.magnitude;
    }

    public void OnCollisionStay(Collision other)
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

    public void OnCollisionExit(Collision other)
    {
        if (BallRollingSoundSource.isPlaying && (other.gameObject.name.Contains("Hidden") || other.gameObject.name.Contains("Chassis")))
        {
            BallRollingSoundSource.Pause();
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Contains("SkeeBall-"))
        {
            BallCollisionSoundSource.Play();
        }
    }

    public void Now()
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
