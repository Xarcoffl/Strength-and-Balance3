using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System.Linq;

public class Window_Graph_Horizontal : MonoBehaviour
{
    private float[] gyroDataArray;
    private List<float> Gyrovaluelist = new List<float>();
    private RectTransform graphcontainer;
    [SerializeField] private Sprite circlesprite;
    public Text valueoutput;

    private void Awake()
    {
        graphcontainer = transform.Find("GraphContainer").GetComponent<RectTransform>();
        string jsonData = PlayerPrefs.GetString("GyroDatah");
        if (!string.IsNullOrEmpty(jsonData))
        {
            GyroDataArrayWrapper dataWrapper = JsonUtility.FromJson<GyroDataArrayWrapper>(jsonData);
            gyroDataArray = dataWrapper.gyroData;
        }
        else
        {
            Debug.LogWarning("No gyro data found in PlayerPrefs.");
        }


        ShowGraph(gyroDataArray);
    }

    [System.Serializable]
    private class GyroDataArrayWrapper
    {
        public float[] gyroData;
    }

    private GameObject Createcircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("Circle", typeof(Image));
        gameObject.transform.SetParent(graphcontainer, false);
        gameObject.GetComponent<Image>().sprite = circlesprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.anchorMax = new Vector2(0.5f, 0);
        return gameObject;
    }

    private void ShowGraph(float[] gyroDataArray)
    {
        float graphHeight = graphcontainer.sizeDelta.y;
        float graphbreath = graphcontainer.sizeDelta.x;
        float yMaximum = 100f;
        float xMaximum = 100f;
        float xSize = 30f;

        List<float> yList = new List<float>();
        
        Gyrovaluelist = gyroDataArray.ToList();
        float[] absolutegyroDataArray = gyroDataArray.Select(Mathf.Abs).ToArray();
        float XmaxVal = absolutegyroDataArray.Max();


        GameObject lastcirclegameObject = null;

        float GyroValueListCount = Gyrovaluelist.Count;

        float Factor = 100f / GyroValueListCount;

        for (int i = 0; i < GyroValueListCount; i++)
        {
            valueoutput.text += gyroDataArray[i].ToString() + "\t";
            //The if condition denotes the size of the distance between the points
            //if (PlayerPrefs.GetFloat("timervalue") == 30)
            //{
            //    yList.Add(i * 5);
            //}
            //if (PlayerPrefs.GetFloat("timervalue") == 60)
            //{
            //    yList.Add(i * 2);
            //}
            //if (PlayerPrefs.GetFloat("timervalue") == 90)
            //{
            //    yList.Add(i * 1.5f);
            //}
            //if (PlayerPrefs.GetFloat("timervalue") == 120)
            //{
            //    yList.Add(i * 1);
            //}
            //if (PlayerPrefs.GetFloat("timervalue") == 150)
            //{
            //    yList.Add(i * 0.7f);
            //}

            yList.Add(i * Factor);

            //float xPosition = i * xSize;
            //float yPosition = (valueList[i] / yMaximum) * graphHeight;
            float xPosition = (gyroDataArray[i] / XmaxVal) * (graphbreath/2);
            
            //float xPosition = gyroDataArray[i] * xSize;
            //if(xPosition > 100)
            //{
            //    xPosition = 100;
            //}
            float yPosition = (yList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = Createcircle(new Vector2(xPosition, yPosition));
            if (lastcirclegameObject != null)
            {
                CreateDotConnection(lastcirclegameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastcirclegameObject = circleGameObject;
        }  

    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameobject = new GameObject("dotConnection", typeof(Image));
        gameobject.transform.SetParent(graphcontainer, false);
        gameobject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameobject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.anchorMax = new Vector2(0.5f, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
}
