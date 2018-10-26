using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;


public class Scanner
{
    public static Color32[] WebCamGetColor32Rotate(WebCamTexture source, bool rotateRight = true)
    {
        Color32[] colorSource = source.GetPixels32();
        Color32[] colorResult = new Color32[colorSource.Length];
        int count = 0;
        int index = 0;
        for (int i = 0; i < source.width; i++)
        {
            for (int j = 0; j < source.height; j++)
            {
                if (rotateRight == false)
                    index = (source.width * (source.height - j)) - source.width + i;
                else
                    index = (source.width * (j + 1)) - (i + 1);

                colorResult[count] = colorSource[index];
                count++;
            }
        }
        return colorResult;
    }
    static IBarcodeReader barcodeReader = new BarcodeReader();
    /// <summary>
    /// Decode the specified webCamTexture and portrait.
    /// </summary>
    /// <returns>The decode.</returns>
    /// <param name="webCamTexture">Web cam texture.</param>
    /// <param name="portrait">If set to <c>true</c> portrait.</param>
    public static Result Decode(WebCamTexture webCamTexture, bool portrait = true)
    {
        try
        {
            // decode the current frame
            if (portrait)
            {
                return barcodeReader.Decode(Scanner.WebCamGetColor32Rotate(webCamTexture), webCamTexture.height, webCamTexture.width);
            }else
            {
                return barcodeReader.Decode(webCamTexture.GetPixels32(), webCamTexture.width, webCamTexture.height);
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
            return null;
        }
    }

}
