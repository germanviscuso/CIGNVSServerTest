using UnityEngine;
using UnityEngine.UI; // For RawImage
using System.Collections;

#if WEBRTC_ENABLED
using SimpleWebRTC;
#endif

namespace CignvsLab
{
    public class WebRTCController : MonoBehaviour
    {
        //[Header("XR Camera Setup")]
        //[Tooltip("Assign your main OpenXR camera here. This camera's output will be streamed.")]
        //[SerializeField] private Camera xrCamera;

        //[Tooltip("Assign a RenderTexture asset here, or one will be created at runtime. This texture will receive the xrCamera's output.")]
        //[SerializeField] private RenderTexture videoRenderTexture;

        //[Tooltip("Dimensions to use if creating RenderTexture at runtime.")]
        //[SerializeField] private Vector2Int renderTextureSize = new Vector2Int(1280, 720);
        //[SerializeField] private int renderTextureDepth = 24; // Depth buffer bits

        [Header("UI")]
        [Tooltip("Optional: Assign a RawImage to display a local preview of what's being streamed.")]
        [SerializeField] private RawImage canvasRawImage;

        [Header("WebRTC")]
        [Tooltip("The GameObject that has the WebRTCConnection component.")]
        [SerializeField] private GameObject connectionGameObject;

#if WEBRTC_ENABLED
        private WebRTCConnection _webRTCConnection;
        private Texture _streamTexture; // This will be our RenderTexture


        private void Start()
        {
            _webRTCConnection = connectionGameObject.GetComponent<WebRTCConnection>();
            canvasRawImage.texture = _streamTexture;
        }

        // private void Start()
        // {
        //     // 1. Validate XR Camera
        //     if (xrCamera == null)
        //     {
        //         Debug.LogError("XR Camera is not assigned in the Inspector. Disabling WebRTCController.");
        //         enabled = false;
        //         return;
        //     }

        //     // 2. Setup RenderTexture
        //     if (videoRenderTexture == null)
        //     {
        //         Debug.LogWarning($"Video RenderTexture is not assigned. Creating one with size {renderTextureSize.x}x{renderTextureSize.y}.");
        //         videoRenderTexture = new RenderTexture(renderTextureSize.x, renderTextureSize.y, renderTextureDepth);
        //         if (!videoRenderTexture.Create()) // Create the GPU resource
        //         {
        //             Debug.LogError("Failed to create RenderTexture. Disabling WebRTCController.");
        //             enabled = false;
        //             return;
        //         }
        //         _didCreateRenderTextureInternally = true;
        //     }
        //     else
        //     {
        //         // If a RenderTexture asset is assigned, ensure it's created (it usually is, but good practice)
        //         if (!videoRenderTexture.IsCreated())
        //         {
        //             if (!videoRenderTexture.Create())
        //             {
        //                 Debug.LogError($"Assigned RenderTexture '{videoRenderTexture.name}' could not be created. Disabling WebRTCController.");
        //                 enabled = false;
        //                 return;
        //             }
        //         }
        //         _didCreateRenderTextureInternally = false;
        //     }

        //     // Assign the RenderTexture to the camera's target texture
        //     xrCamera.targetTexture = videoRenderTexture;
        //     // Ensure the camera is active to render to the texture.
        //     // If your camera is managed by an XR rig, it should already be enabled when the rig is active.
        //     // xrCamera.enabled = true; // Usually not needed if camera is part of an active rig

        //     _streamTexture = videoRenderTexture;

        //     // 3. Setup Local Preview (Optional)
        //     if (canvasRawImage != null)
        //     {
        //         canvasRawImage.texture = _streamTexture;
        //     }
        //     else
        //     {
        //         Debug.Log("Canvas RawImage for local preview is not assigned. Preview will not be shown.");
        //     }

        //     // 4. Get WebRTCConnection
        //     if (connectionGameObject == null)
        //     {
        //         Debug.LogError("Connection GameObject is not assigned. Disabling WebRTCController.");
        //         enabled = false;
        //         return;
        //     }
        //     _webRTCConnection = connectionGameObject.GetComponent<WebRTCConnection>();
        //     if (_webRTCConnection == null)
        //     {
        //         Debug.LogError("WebRTCConnection component not found on the connection GameObject. Disabling WebRTCController.");
        //         enabled = false;
        //         return;
        //     }

        //     // 5. Configure WebRTC Video Source (CRITICAL STEP)
        //     // This part depends heavily on how `WebRTCConnection` and `SimpleWebRTC` are set up.
        //     // Typically, SimpleWebRTC uses a component like `VideoTrackSender` to manage the video source.
        //     // You need to find this component and set its source to our `_streamTexture`.



        //     var videoSender = _webRTCConnection.GetComponentInChildren<VideoTrackSender>(); // Or connectionGameObject.GetComponentInChildren...
        //     if (videoSender != null)
        //     {
        //         videoSender.source = VideoSource.Texture; // Tell SimpleWebRTC to use a Texture
        //         videoSender.texture = _streamTexture;     // Assign our RenderTexture
        //         Debug.Log("VideoTrackSender found and configured to use the XR Camera's RenderTexture.");
        //     }
        //     else
        //     {
        //         Debug.LogWarning("VideoTrackSender component not found as a child of the WebRTCConnection GameObject. " +
        //                          "Video streaming might not work. Ensure SimpleWebRTC is set up to send a texture and that this script can find and assign the VideoTrackSender's texture.");
        //         // IMPORTANT: If the `WebRTCConnection` script itself has a public field or method
        //         // to set the texture (e.g., `_webRTCConnection.SetSourceTexture(_streamTexture)`),
        //         // use that instead of directly accessing VideoTrackSender.
        //     }

        //     Debug.Log("WebRTCController initialized. Ready to stream from XR Camera.");
        // }

        private void Update()
        {
            if (_webRTCConnection == null || _streamTexture == null)
            {
                return; // Not initialized properly
            }

            // Check for input to start transmission
            // The original script used OVRInput.Get. GetDown is usually preferred for single actions.
            // For a more generic OpenXR setup, you'd use Unity's Input System.
            //if (OVRInput.GetDown(OVRInput.Button.Start)) // Or your preferred OpenXR input
            //{
                Debug.Log("Start button pressed. Attempting to start video transmission.");
                _webRTCConnection.StartVideoTransmission();
                // Note: Ensure StartVideoTransmission() in your WebRTCConnection script
                // is now expecting the video source to be configured as a Texture
                // (e.g., via the VideoTrackSender setup in Start() method here).
                // If it needs the texture passed directly, you might need:
                // _webRTCConnection.StartVideoTransmission(_streamTexture);
                // But this depends on your WebRTCConnection script's design.
            //}
        }

        // private void OnDestroy()
        // {
        //     // Clean up the camera's target texture
        //     if (xrCamera != null && xrCamera.targetTexture == videoRenderTexture)
        //     {
        //         xrCamera.targetTexture = null;
        //     }

        //     // Clean up the RenderTexture only if this script created it
        //     if (videoRenderTexture != null && _didCreateRenderTextureInternally)
        //     {
        //         if (RenderTexture.active == videoRenderTexture)
        //         {
        //             RenderTexture.active = null;
        //         }
        //         videoRenderTexture.Release(); // Release the GPU resource
        //         Destroy(videoRenderTexture);  // Destroy the C# RenderTexture object
        //         _streamTexture = null;
        //         Debug.Log("Internally created RenderTexture has been released and destroyed.");
        //     }
        // }
#endif // WEBRTC_ENABLED
    }
}