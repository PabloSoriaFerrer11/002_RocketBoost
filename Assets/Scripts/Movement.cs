using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotate;

    [SerializeField] private float thrustForce = 20f;
    [SerializeField] float rotationStrength = 100f;

    [SerializeField] private ParticleSystem leftThrusterParticles;
    [SerializeField] private ParticleSystem rightThrusterParticles;
    [SerializeField] private ParticleSystem mainThrusterParticles;
    

    [SerializeField] private AudioClip thrustAudioClip;


    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        thrust.Enable();
        rotate.Enable();
    }


    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if(thrust.IsPressed())
        {
            StartThusting();
        }
        else
        {
            StopThusting();
        }
    }

    private void StartThusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustForce * Time.fixedDeltaTime);
        mainThrusterParticles.Play();
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustAudioClip);
        }
    }

    private void StopThusting()
    {
        leftThrusterParticles.Stop();
        rightThrusterParticles.Stop();
        mainThrusterParticles.Stop();
        audioSource.Stop();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotate.ReadValue<float>();
        if(rotationInput < 0f)
        {
            TurnRight();
        }
        else if(rotationInput > 0f)
        {
            TurnLeft();
        }
    }

    private void TurnRight()
    {
        rightThrusterParticles.Play();
        leftThrusterParticles.Stop();
        ApplyRotation(rotationStrength);
    }

        private void TurnLeft()
    {
        leftThrusterParticles.Play();
        rightThrusterParticles.Stop();
        ApplyRotation(-rotationStrength);
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        //rb.freezeRotation = true; // freezing rotation so we can manually rotate
        rb.AddRelativeTorque(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);
        //rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }
}
