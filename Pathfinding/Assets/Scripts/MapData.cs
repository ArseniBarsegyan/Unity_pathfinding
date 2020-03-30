using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    public int width = 10;
    public int height = 5;

    public TextAsset TextAsset;
    public Texture2D TextureMap;
    public string ResourcePath = "Mapdata";

    public Color32 openColor = Color.white;
    public Color32 blockedColor = Color.black;
    public Color32 lightTerrainColor = new Color32(124, 194,78,255);
    public Color32 mediumTerrainColor = new Color32(252, 255, 52, 255);
    public Color32 heavyTerrainColor = new Color32(255, 129, 12, 255);

    private static Dictionary<Color32, NodeType> terrainLookupTable = new Dictionary<Color32, NodeType>();

    void Awake()
    {
        SetupLookupTable();
    }

    void Start()
    {
        string levelName = SceneManager.GetActiveScene().name;

        if (TextureMap == null)
        {
            TextureMap = Resources.Load<Texture2D>(ResourcePath + "/" + levelName);
        }

        if (TextAsset == null)
        {
            TextAsset = Resources.Load<TextAsset>(ResourcePath + "/" + levelName);
        }
    }

    public List<string> GetMapFromTextFile(TextAsset textAsset)
    {
        var lines = new List<string>();

        if (textAsset != null)
        {
            string textData = textAsset.text;
            string[] delimiters = {"\r\n", "\n"};
            lines.AddRange(textData.Split(delimiters, StringSplitOptions.None));
            lines.Reverse();
        }
        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        return GetMapFromTextFile(TextAsset);
    }

    public List<string> GetMapFromTexture(Texture2D texture)
    {
        var lines = new List<string>();

        if (texture != null)
        {
            for (int y = 0; y < texture.height; y++)
            {
                string newLine = string.Empty;

                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);

                    if (terrainLookupTable.ContainsKey(pixelColor))
                    {
                        NodeType nodeType = terrainLookupTable[pixelColor];
                        int nodeTypeNum = (int) nodeType;
                        newLine += nodeTypeNum;
                    }
                    else
                    {
                        newLine += '0';
                    }
                }

                lines.Add(newLine);
            }
        }
        
        return lines;
    }

    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;
        
        foreach (var line in textLines)
        {
            if (line.Length > width)
            {
                width = line.Length;
            }
        }
    }

    public int[,] MakeMap()
    {
        var lines = new List<string>();

        if (TextureMap != null)
        {
            lines = GetMapFromTexture(TextureMap);
        }
        else
        {
            lines = GetMapFromTextFile();
        }

        SetDimensions(lines);

        int[,] map = new int[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)char.GetNumericValue(lines[y][x]);
                }
            }
        }

        return map;
    }

    void SetupLookupTable()
    {
        terrainLookupTable.Add(openColor, NodeType.Open);
        terrainLookupTable.Add(blockedColor, NodeType.Blocked);
        terrainLookupTable.Add(lightTerrainColor, NodeType.LightTerrain);
        terrainLookupTable.Add(mediumTerrainColor, NodeType.MediumTerrain);
        terrainLookupTable.Add(heavyTerrainColor, NodeType.HeavyTerrain);
    }

    public static Color GetColorFromNodeType(NodeType nodeType)
    {
        if (terrainLookupTable.ContainsValue(nodeType))
        {
            Color colorKey = terrainLookupTable.FirstOrDefault(x => x.Value == nodeType).Key;
            return colorKey;
        }

        return Color.white;
    }
}
