using UnityEngine;
using ZXing;
using UnityEngine.UI;

namespace Ping9
{
    public class ScanSample : MonoBehaviour
    {

        public Text txtType;
        public Text txtMessage;

        public void ShowPopupScan()
        {
            PopupScan.Instance.Init(OnFinish);

        }
        public void OnFinish(BarcodeFormat format, string message)
        {
            txtMessage.text = message;
            txtType.text = format.ToString();
        }
    }
}