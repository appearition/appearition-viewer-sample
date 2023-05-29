using UnityEngine;
using System.Collections;
using Vuforia;

namespace Appearition.ArDemo.Vuforia
{
    public class CameraFocusController : MonoBehaviour
    {
        void Awake()
        {
            var vuforia = VuforiaARController.Instance;
            vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
            vuforia.RegisterOnPauseCallback(OnPaused);
            vuforia.RegisterVuforiaInitializedCallback(OnVuforiaStarted);
            
            AppearitionArHandler.OnScanStateChanged += AppearitionArHandler_OnScanStateChanged;
        }


        private void AppearitionArHandler_OnScanStateChanged(bool isScanning)
        {
            if (isScanning)
            {
                if (AppearitionArHandler.ProviderHandler != null &&
                    (AppearitionArHandler.ProviderHandler.GetType().IsSubclassOf(typeof(VuforiaCloudProviderHandler))
                    || AppearitionArHandler.ProviderHandler.GetType().IsSubclassOf(typeof(VuforiaMarkerlessProviderHandler))))
                {
                    if(CameraDevice.Instance.IsActive())
                        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
                }
            }
                
        }

        
        private void OnVuforiaStarted()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }



        private void OnPaused(bool paused)
        {
            if (!paused)
            {
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            }
            else
            {
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);

            }
        }
    }
}