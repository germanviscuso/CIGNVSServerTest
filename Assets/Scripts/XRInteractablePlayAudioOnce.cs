using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRInteractablePlayAudioOnce : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private XRGrabInteractable grabbable;

    private void Awake()
    {
        grabbable = GetComponent<XRGrabInteractable>();
        if (grabbable != null)
        {
            grabbable.selectEntered.AddListener(OnGrab); // grabbed event
        }
        else
        {
            Debug.LogWarning("No XRGrabInteractable found on this GameObject.");
        }
    }

    private void OnGrab(SelectEnterEventArgs arg) // Corrected parameter type
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Audio is now playing on grab.");
        }
        else
        {
            Debug.LogWarning("No AudioSource assigned or audio was already playing.");
        }
    }

    private void OnActivate(ActivateEventArgs arg)
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

}
