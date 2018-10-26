using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace Ping9
{
    public class PopupScan : MonoBehaviour
    {
        public static PopupScan Instance { get; private set; }
        void Awake()
        {
            Instance = this;
            guiMain.SetActive(false);
        }

        public GameObject guiMain;
        public RawImage rendererCamera;
        WebCamTexture webCamTexture;
        AspectRatioFitter fit;
        bool isScan = false;
        public void Init(Action<BarcodeFormat, string> _finish)
        {
            actionFinish = _finish;
            if (fit == null)
            {
                fit = rendererCamera.gameObject.GetComponent<AspectRatioFitter>();
            }
            webCamTexture = new WebCamTexture();
            rendererCamera.texture = webCamTexture;
            StartScan();
        }

        public void StartScan()
        {
            guiMain.SetActive(true);
            webCamTexture.Play();
            isScan = true;
        }
        void StopScan()
        {
            webCamTexture.Stop();
            isScan = false;
        }
        void Update()
        {
            if (isScan)
            {
                var _result = Scanner.Decode(webCamTexture, true);
                if (_result != null)
                {
                    ShowResult(_result);
                }
                // Fix camera
                if (webCamTexture != null && webCamTexture.isPlaying)
                {
                    float ratio = (float)webCamTexture.width / (float)webCamTexture.height;
                    fit.aspectRatio = ratio; // Set the aspect ratio
                    float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
                    rendererCamera.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera
                    int orient = -webCamTexture.videoRotationAngle;
                    rendererCamera.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
                }
            }
            
        }
        void ShowResult(Result _result)
        {
            result = _result;
            StopScan();
            OnFinish();
            //PopupManager.ShowYesNoPopUp("QR CODE DETECTED", result.Text, OnFinish, StartScan, "Finish", "Try Again");
        }
        Action<BarcodeFormat, string> actionFinish;
        Result result;
        void OnFinish()
        {
            if (actionFinish != null)
            {
                actionFinish(result.BarcodeFormat ,result.Text);
            }
            OnExit();
        }
        void Accept()
        {
            StopScan();
            guiMain.SetActive(false);
            Application.OpenURL(result.Text);
        }
        public void OnExit()
        {
            StopScan();
            guiMain.SetActive(false);
            webCamTexture = null;
        }
    }
}