using UnityEngine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;

public class CaptureCanvas : MonoBehaviour
{
    public Canvas canvas;
    public Camera uiCamera; // Assign the camera used to render the Canvas

    public string GetDownloadsFolderPath()
    {
        string downloadsPath = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject environment = new AndroidJavaClass("android.os.Environment"))
                {
                    using (AndroidJavaObject downloadsDir = environment.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory", environment.GetStatic<string>("DIRECTORY_DOWNLOADS")))
                    {
                        downloadsPath = downloadsDir.Call<string>("getAbsolutePath");
                    }
                }
            }
        }
#else
        // If you are testing in the Unity Editor, you can set a default path here
        downloadsPath = Path.Combine(Application.persistentDataPath, "Report.pdf");
         // Example for Windows
#endif

        return downloadsPath;
    }

    public Texture2D Capture()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        uiCamera.targetTexture = renderTexture;
        RenderTexture.active = renderTexture;
        uiCamera.Render();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        uiCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        return texture;
    }

    public static string SaveTextureAsPNG(Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, "CanvasCapture.png");
        File.WriteAllBytes(filePath, bytes);
        return filePath;
    }

    public static void GeneratePDF(string imagePath)
    {
        string pdfPath;
        CaptureCanvas captureCanvas = new CaptureCanvas();
        string pdfPath2 = "/storage/emulated/0/Download/Pain And Stroke";//captureCanvas.GetDownloadsFolderPath();
        if (!Directory.Exists(pdfPath2))
        {
            Directory.CreateDirectory(pdfPath2);
        }
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string name = PlayerPrefs.GetString("regname");
        string Id = PlayerPrefs.GetString("regid");
        pdfPath = pdfPath2 + "/" + Id + "-" + name + "-" + timestamp + ".pdf";
        //string pdfPath = GetDownloadsFolderPath();

        Document document = new Document();
        PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));
        document.Open();

        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
        img.ScaleToFit(document.PageSize.Width - 20, document.PageSize.Height - 60);
        img.Alignment = Element.ALIGN_CENTER;

        document.Add(img);
        document.Close();

        Debug.Log("PDF created at: " + pdfPath);
        string newmessage = "PDF Created At" + pdfPath;
        captureCanvas.messageinfo(newmessage);
    }

    public void OnCaptureButtonClicked()
    {
        Texture2D capturedTexture = Capture();
        string imagePath = SaveTextureAsPNG(capturedTexture);
        GeneratePDF(imagePath);
    }

    public void messageinfo(string message)
    {
        SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
    }
}
