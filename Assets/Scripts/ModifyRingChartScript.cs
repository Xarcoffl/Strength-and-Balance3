using UnityEngine;
using System.Collections.Generic; // Add this line for List<T>
using XCharts.Runtime;
using System.Linq;

public class modifyringchartScript : MonoBehaviour
{
    private float[] Testingarray = new float[7];
    private float[] percentageDeviationArray;
    private float[] gyroDataArray;
    float percentageDeviation;


    private double[] stabilityvaluesstore = new double[5];
    private int currentIndex;


    void Start()
    {
        // Ensure the GameObject has a RingChart component
        var chart = GetComponent<RingChart>();
        if (chart == null)
        {
            Debug.LogError("No RingChart component found on the GameObject.");
            return;
        }

        string jsonData = PlayerPrefs.GetString("GyroData");
        if (!string.IsNullOrEmpty(jsonData))
        {
            GyroDataArrayWrapper dataWrapper = JsonUtility.FromJson<GyroDataArrayWrapper>(jsonData);
            gyroDataArray = dataWrapper.gyroData;
            Debug.Log("gyro data found in ring chart");
            //for (int i = 0; i < gyroDataArray.Length; i++)
            //{
            //    Debug.LogWarning(gyroDataArray[i]);
            //}

            Testingarray[0] = -1.3f;
            Testingarray[1] = 10.3f;
            Testingarray[2] = 21.3f;
            Testingarray[3] = -42.3f;
            Testingarray[4] = 01.3f;
            Testingarray[5] = 54.3f;
            Testingarray[6] = 12.3f;

            //for (int i = 0; i < 7; i++)
            //{
            //   Debug.LogWarning(Testingarray[i]);
            //}

            float[] absoluteValues = gyroDataArray.Select(Mathf.Abs).ToArray();
            Debug.Log("absoluteValues below");
            //for (int i = 0; i < absoluteValues.Length; i++)
            //{
            //    Debug.LogWarning(absoluteValues[i]);
            //}

            float mad = absoluteValues.Average();
            //Debug.LogWarning(mad);
            float maxAbsoluteValue = absoluteValues.Max();
            ////percentageDeviationArray = new float[absoluteValues.Length];

            ////for (int i = 0; i < absoluteValues.Length; i++)
            ////{
            ////    percentageDeviationArray[i] = (absoluteValues[i] / mad) * 100;
            ////    Debug.LogError(percentageDeviationArray[i]);
            ////}
            //Debug.LogWarning(maxAbsoluteValue);
            //percentageDeviation = (mad / maxAbsoluteValue) * 100f;
            ////percentageDeviation = percentageDeviationArray.Average();
            percentageDeviation = 10 * (10 - mad);


            currentIndex = PlayerPrefs.GetInt("stabilityindex");
            Debug.LogWarning(currentIndex);
            StoreValue(percentageDeviation);

            PlayerPrefs.SetFloat("percentageDeviation", percentageDeviation);

            // Ensure PlayerPrefs is saved
            PlayerPrefs.Save();
        }
        // Clear default data
        chart.RemoveData();

        // Add a new Ring type Serie
        var ringSerie = chart.AddSerie<Ring>("Ring");

        // Add custom data for the Ring chart
        var data = new List<double>();  // Create a list of double values
        data.Add(percentageDeviation);
        data.Add(100.0);
        ringSerie.AddData(data, "Category 1"); // Add the data to the series
        Debug.LogError("data added");

        // Ensure series data exists and set custom colors for each segment
        if (ringSerie.data.Count >= 1)
        {
            Debug.LogError("value displayed");
            var itemStyle1 = ringSerie.data[0].EnsureComponent<ItemStyle>();
            itemStyle1.color = Color.white;
        }
        else
        {
            Debug.LogError("Not enough data entries in the Ring series.");
        }

        // Modify some properties of the series to turn it into a RingChart
        ringSerie.radius[0] = 0.3f; // Inner radius
        ringSerie.radius[1] = 0.35f; // Outer radius
        ringSerie.gap = 10;


        var label = ringSerie.data[0].EnsureComponent<LabelStyle>();
        label.show = true;
        label.position = LabelStyle.Position.Inside;
        label.formatter = "{d:f0}%"; // Display the percentage
        label.textStyle.color = Color.white; // Set label color if needed

    }

    [System.Serializable]
    private class GyroDataArrayWrapper
    {
        public float[] gyroData;
    }

    void StoreValue(double value)
    {
        stabilityvaluesstore = RetrieveArrayFromPlayerPrefs("stabilityarray");
        stabilityvaluesstore[currentIndex] = value;
        string serializedArray = ListToString(stabilityvaluesstore);
        PlayerPrefs.SetString("stabilityarray", serializedArray);
        PlayerPrefs.Save();
        currentIndex = (currentIndex + 1) % stabilityvaluesstore.Length;
        PlayerPrefs.SetInt("stabilityindex", currentIndex);
        PlayerPrefs.Save();
    }

    string ListToString(double[] array)
    {

        string serializedList = string.Join(",", array);
        return serializedList;
    }

    public static double[] StringToArray(string arrayString)
    {
        string[] stringArray = arrayString.Split(',');
        double[] intArray = new double[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            if (double.TryParse(stringArray[i], out double result))
            {
                intArray[i] = result;
            }
            else
            {
                Debug.LogError($"Invalid integer '{stringArray[i]}' in serialized array. Replacing with 0.");
                intArray[i] = 0; // Replace invalid integers with a default value
            }
        }
        return intArray;
    }

    // Method to retrieve the array from PlayerPrefs
    public static double[] RetrieveArrayFromPlayerPrefs(string key)
    {
        string serializedArray = PlayerPrefs.GetString(key);
        if (!string.IsNullOrEmpty(serializedArray))
        {
            Debug.Log("Serialized array: " + serializedArray);
            return StringToArray(serializedArray);
        }
        Debug.LogWarning("Serialized array is empty or not found for key: " + key);
        return null;
    }
}
