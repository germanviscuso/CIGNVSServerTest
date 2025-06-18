using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CignvsLab
{
    public class CommsXRGrabListener : MonoBehaviour
    {
        private CommsManager commsManager;
        private string grabTopic = "xr/grab";
        private string releaseTopic = "xr/release";
        private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabbable;

        [System.Obsolete]
        void Start()
        {
            commsManager = FindObjectOfType<CommsManager>();
            if (commsManager == null)
            {
                Debug.LogError("❌ CommsManager not found!");
                return;
            }
            Debug.Log($"✅ Subscribing to grab and release topics.");
            commsManager.SubscribeToChannel(grabTopic, OnGrabFeedback);
            commsManager.SubscribeToChannel(releaseTopic, OnReleaseFeedback);
        }

        void OnEnable()
        {
            grabbable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            grabbable.selectEntered.AddListener(OnGrab);
            grabbable.selectExited.AddListener(OnRelease);
        }

        public void OnGrab(SelectEnterEventArgs arg)
        {
            Debug.Log($"🟢 Sending grab event.");
            commsManager.PublishToChannel(grabTopic, new { eventType = "grab", objectName = gameObject.name });
        }

        public void OnRelease(SelectExitEventArgs arg)
        {
            Debug.Log($"🔴 Sending release event.");
            commsManager.PublishToChannel(releaseTopic, new { eventType = "release", objectName = gameObject.name });
        }

        private void OnGrabFeedback(string message) => Debug.Log($"📩 Grab feedback: {message}");
        private void OnReleaseFeedback(string message) => Debug.Log($"📩 Release feedback: {message}");
    }
}
