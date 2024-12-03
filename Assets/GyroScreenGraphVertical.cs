using UnityEngine;
using UnityEngine.UI;

public class GyroScreenGraphVertical : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private Sprite circleSprite;
    private GameObject circleObject;
    private RectTransform circleRectTransform;
    private float xMin, xMax;

    float Sensitive;
    float sensitivityval;

    public Text Leveldisplay;

    private float angleGyro = 0f;
    private float previousTime;
    private float alpha = 0.6f;

    void Start()
    {

        Input.gyro.enabled = true;

        Sensitive = PlayerPrefs.GetFloat("Sensitivity");

        string formattedTime = returnsensitivity(Sensitive);
        Leveldisplay.text = formattedTime;
        PlayerPrefs.SetString("Levelv", formattedTime);
        PlayerPrefs.Save();



        CreateCircle();
        SetGraphBoundaries();
    }

    void Update()
    {
        if (circleRectTransform != null)
        {
            float currentTime = Time.time;
            float dt = currentTime - previousTime;
            previousTime = currentTime;

            float xGyro = 2 * Input.gyro.rotationRateUnbiased.x;

            //Vector3 gyroValue = Input.gyro.rotationRateUnbiased; // Gyroscope data in radians per second
            //angleGyro += gyroValue.y * dt;
            Vector3 accel = Input.acceleration;
            float angleAccel = Mathf.Atan2(accel.y, Mathf.Sqrt(accel.x * accel.x + accel.z * accel.z));
            //float angle = alpha * angleGyro + (1 - alpha) * angleAccel;
            float finalangle = angleAccel * 10;
            MoveCircle(finalangle);
        }
    }

    private void CreateCircle()
    {
        circleObject = new GameObject("Circle", typeof(Image));
        circleObject.transform.SetParent(graphContainer, false);
        circleObject.GetComponent<Image>().sprite = circleSprite;

        circleRectTransform = circleObject.GetComponent<RectTransform>();
        circleRectTransform.sizeDelta = new Vector2(50, 50); // Adjust size as needed
        circleRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        circleRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        circleRectTransform.anchoredPosition = new Vector2(0, 0); // Start position at center
    }

    private void SetGraphBoundaries()
    {
        xMin = -graphContainer.sizeDelta.y / 2;
        xMax = graphContainer.sizeDelta.y / 2;
    }

    private void MoveCircle(float xGyro)
    {
        //float sensitivity = 100f; // Adjust sensitivity as needed
        float sensitivityvalue = Sensitivity(Sensitive);
        Vector2 newPosition = circleRectTransform.anchoredPosition;
        newPosition.y = xGyro * sensitivityvalue;
        //newPosition.x += xGyro * sensitivityvalue * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, xMin, xMax);

        circleRectTransform.anchoredPosition = newPosition;
    }

    private float Sensitivity(float sensitive)
    {

        if (sensitive == 0)
        {
            sensitivityval = 20;
        }
        if (sensitive == 1)
        {
            sensitivityval = 40;
        }
        if (sensitive == 2)
        {
            sensitivityval = 60;
        }
        if (sensitive == 3)
        {
            sensitivityval = 80;
        }
        if (sensitive == 4)
        {
            sensitivityval = 100;
        }
        if (sensitive == 5)
        {
            sensitivityval = 120;
        }
        return sensitivityval;
    }

    public string returnsensitivity(float sensitivereturn)
    {
        return string.Format("Level {0}", sensitivereturn);
    }
}
