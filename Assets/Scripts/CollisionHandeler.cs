using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandeler : MonoBehaviour
{
    [SerializeField] private int delay = 3;

    [SerializeField] private AudioClip crashAudioClip;
    [SerializeField] private AudioClip successAudioClip;

    [SerializeField] private ParticleSystem successParticles;
    [SerializeField] private ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isControllable = true;
    bool isCollidable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespontToDebugKeys();
    }

    private void RespontToDebugKeys()
    {
        // Es necesario InputSystem para poder usar el teclado, no funciona sin el
        // Importante el wasPressedThisFrame
        if(Keyboard.current.zKey.wasPressedThisFrame)
        {
            nextScene();
            Debug.Log("Z PRESS: Next Scene");
        }
        else if(Keyboard.current.xKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
            Debug.Log("X PRESS: Collisions: " + (isCollidable ? "ON" : "OFF"));
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(!isControllable || !isCollidable) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            case "Finish":
                StartNextScene();
                break;
            default:
                StartCrash();
                break;
        }
    }
    // Invoke no es tan bueno como las corrutinas
    private void StartCrash()
    {
        // Forma simple, vamos a desactivar el script de movimiento 
        GetComponent<Movement>().enabled = false;
        isControllable = false;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        crashParticles.Play();
        audioSource.PlayOneShot(crashAudioClip);
        Invoke("reloadScene", delay);
    }

    private void StartNextScene()
    {
        GetComponent<Movement>().enabled = false;
        isControllable = false;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        successParticles.Play();
        audioSource.PlayOneShot(successAudioClip);
        Invoke("nextScene", delay);
    }

    private void reloadScene()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }


    private void nextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }

    }
}
