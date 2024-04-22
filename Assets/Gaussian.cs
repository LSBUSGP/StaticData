using System.IO;
using UnityEngine;

[System.Serializable]
public class Gaussian
{
    public float mean;
    public float standardDeviation;

    public float GetRandomNumber()
    {
        float u1;
        // ensure that u1 is at least > 0
        do
        {
            u1 = Random.value;
        } 
        while (u1 == 0.0f);
        float u2 = Random.value;
        float z = standardDeviation * Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2) + mean;
        return z;
    }

    // to test the random number generator write out a file with the frequencies
    public void TestOutput(string filename)
    {
        // with 32 bit numbers it will not produce more than 7 * standardDeviation
        int [] numbers = new int[Mathf.RoundToInt(mean + 7.0f * standardDeviation)];
        for (int i = 0; i < 1000; i++)
        {
            numbers[Mathf.RoundToInt(GetRandomNumber())] += 1;
        }
        File.WriteAllText(filename, "Frequencies\n" + string.Join("\n", numbers));
    }
}
