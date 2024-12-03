using UnityEngine;
using XCharts.Runtime;

public class LineChartScript : MonoBehaviour
{
    public int text;
    void Start()
    {
        
        // Ensure the GameObject has a BarChart component
        var chart = GetComponent<BarChart>();
        if (chart == null)
        {
            Debug.LogError("No BarChart component found on the GameObject.");
            return;
        }
        Debug.LogError("BarChart is present.");
        // Clear default data
        chart.RemoveData();

        // Add a new Bar type Serie
        var barSerie = chart.AddSerie<Bar>("bar");
        float[] retrievedArray = RetrieveArrayFromPlayerPrefs("stabilityarray");
        Debug.LogError(retrievedArray);

        // Add X-axis data and corresponding Y-axis values
        for (int i = 0; i < 5; i++)
        {
            chart.AddXAxisData("x" + (i + 1));
            var value = retrievedArray[i];
            chart.AddData(0, value);
        }

        // Modify some properties of the serie
        barSerie.barWidth = 60;
        barSerie.barMaxWidth = 100;

        // Modify some properties of individual data points if needed
        for (int i = 0; i < barSerie.data.Count; i++)
        {
            var serieData = barSerie.data[i];
            var itemStyle = serieData.EnsureComponent<ItemStyle>();
            itemStyle.color = Color.white;
        }
    }

    float[] RetrieveArrayFromPlayerPrefs(string key)
    {
        string serializedArray = PlayerPrefs.GetString(key);
        Debug.LogWarning(serializedArray);
        if (!string.IsNullOrEmpty(serializedArray))
        {
            return StringToArray(serializedArray);
        }
        return null;
    }

    // Method to serialize a string to an array
    public static float[] StringToArray(string arrayString)
    {
        string[] stringArray = arrayString.Split(',');
        float[] intArray = new float[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            if (float.TryParse(stringArray[i], out float result))
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
}
