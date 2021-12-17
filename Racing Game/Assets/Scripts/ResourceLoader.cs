using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceLoader
{
    public static GameObject carPrefab;

    public static string[] carNames = { "Nissan 240SX", "Ford Mustang '65", "Volkswagen Golf MK1", "Subaru BRZ", "Dodge Challenger", "Tesla Model 3" };

    public static Sprite[] carChassisSprites = new Sprite[6];
    public static Sprite[] carWindowSprites = new Sprite[6];
    public static Sprite[] carSpoilerSprites = new Sprite[3];

    public static Color[] carColors = { new Color(0.301f, 0.149f, 0.176f), new Color(0.466f, 0.188f, 0.223f), new Color(0.662f, 0.247f, 0.278f), new Color(0.796f, 0.317f, 0.345f), // 0 - 3
                                        new Color(0.854f, 0.439f, 0.458f), new Color(0.886f, 0.580f, 0.596f), new Color(0.686f, 0.407f, 0.533f), new Color(0.674f, 0.439f, 0.670f), // 4 - 7
                                        new Color(0.486f, 0.309f, 0.478f), new Color(0.321f, 0.200f, 0.317f), new Color(0.200f, 0.125f, 0.215f), new Color(0.074f, 0.113f, 0.184f), // 8 - 11
                                        new Color(0.168f, 0.149f, 0.431f), new Color(0.309f, 0.219f, 0.639f), new Color(0.490f, 0.270f, 0.741f), new Color(0.737f, 0.431f, 0.803f), // 12 - 15
                                        new Color(0.850f, 0.643f, 0.854f), new Color(0.952f, 0.862f, 0.917f), new Color(0.709f, 0.862f, 0.866f), new Color(0.345f, 0.678f, 0.701f), // 16 - 19
                                        new Color(0.305f, 0.603f, 0.670f), new Color(0.262f, 0.490f, 0.596f), new Color(0.196f, 0.364f, 0.466f), new Color(0.121f, 0.215f, 0.305f), // 20 - 23
                                        new Color(0.109f, 0.164f, 0.200f), new Color(0.145f, 0.227f, 0.227f), new Color(0.211f, 0.341f, 0.290f), new Color(0.298f, 0.494f, 0.345f), // 24 - 27
                                        new Color(0.447f, 0.654f, 0.423f), new Color(0.619f, 0.768f, 0.505f), new Color(0.780f, 0.862f, 0.615f), new Color(0.835f, 0.796f, 0.505f), // 28 - 31
                                        new Color(0.784f, 0.674f, 0.356f), new Color(0.792f, 0.592f, 0.286f), new Color(0.756f, 0.474f, 0.247f), new Color(0.752f, 0.403f, 0.243f), // 32 - 35
                                        new Color(0.482f, 0.207f, 0.188f), new Color(0.670f, 0.333f, 0.247f), new Color(0.756f, 0.490f, 0.341f), new Color(0.792f, 0.647f, 0.450f), // 36 - 39
                                        new Color(0.941f, 0.898f, 0.847f), new Color(0.894f, 0.792f, 0.694f), new Color(0.807f, 0.627f, 0.505f), new Color(0.666f, 0.466f, 0.372f), // 40 - 43
                                        new Color(0.486f, 0.313f, 0.266f), new Color(0.329f, 0.203f, 0.192f), new Color(0.203f, 0.137f, 0.141f), new Color(0.098f, 0.078f, 0.098f), // 44 - 47
                                        new Color(0.058f, 0.047f, 0.086f), new Color(0.121f, 0.121f, 0.168f), new Color(0.200f, 0.207f, 0.270f), new Color(0.301f, 0.321f, 0.388f), // 48 - 51
                                        new Color(0.439f, 0.466f, 0.525f), new Color(0.611f, 0.639f, 0.682f), new Color(0.803f, 0.819f, 0.843f), new Color(1.000f, 1.000f, 1.000f), // 52 - 55
                                        new Color(0.705f, 0.705f, 0.705f), new Color(0.521f, 0.521f, 0.521f), new Color(0.364f, 0.364f, 0.364f), new Color(0.239f, 0.239f, 0.239f), // 56 - 59
                                        new Color(0.152f, 0.152f, 0.152f), new Color(0.105f, 0.105f, 0.105f), new Color(0.074f, 0.074f, 0.074f) };                                  // 60 - 62

    public static int[] carUniversalColors = { 55, 59 };
    public static int[,] carStockColors = { { 3, 41 }, { 3, 23 }, { 57, 26 }, { 3, 21 }, { 34, 28 }, { 3, 21 } };

    public static GameObject raceStartpointPrefab;
    public static GameObject raceCheckpointPrefab;
    public static GameObject raceEndpointPrefab;

    public static void LoadAll()
    {
        carPrefab = Resources.Load("Car") as GameObject;

        Sprite[] carSprites = Resources.LoadAll<Sprite>("car_sprites");

        for (int i = 0; i < carSprites.Length - 1; i++)
        {
            if (i < 6)
                carChassisSprites[i % 6] = carSprites[i];
            else if (i < 12)
                carWindowSprites[i % 6] = carSprites[i];
            else
                carSpoilerSprites[i % 3] = carSprites[i];
        }

        raceStartpointPrefab = Resources.Load("Race_Startpoint") as GameObject;
        raceCheckpointPrefab = Resources.Load("Race_Checkpoint") as GameObject;
        raceEndpointPrefab = Resources.Load("Race_Endpoint") as GameObject;
    }

    public static Color getStockColorByIndex(int chassisIndex, int colorIndex)
    {
        switch (colorIndex)
        {
            case 0: return carColors[carUniversalColors[0]];
            case 1: return carColors[carUniversalColors[1]];
            case 2: return carColors[carStockColors[chassisIndex, 0]];
            default: return carColors[carStockColors[chassisIndex, 1]];
        }
    }
}
