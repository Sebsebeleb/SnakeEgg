using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject tilePrefab;

    public int size = 10;

    private Dictionary<Tuple<int, int>, Tile> tiles = new Dictionary<Tuple<int, int>, Tile>();

    public float displaySize;

    public Transform displayRoot;

    public Color[] groupColors;

    private List<Color> unusedColors;

    private struct tempLevelData
    {
        public int x, y;
        public Tile.State state;
    }

    private void Awake()
    {
        float h = 0;
        float s = 0.09f;
        float v = 0.93f;
        
        unusedColors = new List<Color>();

        int count = 18;
        for (int i = 0; i < count; i++)
        {
            Color c = Color.HSVToRGB((float)i / count, s, v);
            unusedColors.Add(c);
        }

        for (int counter = 0; counter < 3; counter++)
        {
            for (int o = 0; o < count; o+=3)
            {
                Color c = unusedColors[o];
                unusedColors.RemoveAt(0);
                unusedColors.Add(c);
            }    
        }
    }

    public Color takeColor()
    {
        if (unusedColors.Count <= 0)
        {
            return new Color(0.79f, 0.63f, 0.76f);
        }
        Color c = unusedColors[0];

        unusedColors.RemoveAt(0);
        return c;
    }

    public void returnColor(Color c)
    {
        unusedColors.Add(c);
    }

    // Start is called before the first frame update
    void Start()
    {
        tempLevelData[] level = new[]
        {
            new tempLevelData()
            {
                state = Tile.State.Two, x = 1, y = 0
            },
            new tempLevelData()
            {
                state = Tile.State.Snake, x = 3, y = 0
            },
            new tempLevelData()
            {
                state = Tile.State.Snake, x = 2, y = 3
            }
        };

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                GameObject tileDisplay = Instantiate(tilePrefab, displayRoot, false);
                tileDisplay.GetComponent<Tile>().SetPos(x, y);
                tiles.Add(new Tuple<int, int>(x, y), tileDisplay.GetComponent<Tile>());
                tileDisplay.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(x * displaySize, -y * displaySize);
            }
        }

        foreach (tempLevelData data in level)
        {
            var t = tiles[new Tuple<int, int>(data.x, data.y)];
            t.Set(data.state, true);
        }
    }

    public Vector2Int getClicked(float x, float y)
    {
        RectTransform r = (RectTransform) transform;

        float xx = (r.anchoredPosition.x - x) / (displaySize);
        float yy = (r.anchoredPosition.y - y) / (displaySize);
        return new Vector2Int(size - (Mathf.RoundToInt(xx)+4), Mathf.RoundToInt(yy)+3);
    }

    public Tile getTile(int x, int y)
    {
        if (x < 0 || y < 0 || x > size - 1 || y > size - 1)
        {
            return null;
        }
        return tiles[new Tuple<int, int>(x, y)];
    }

    public List<Tile> getAdjacentTiles(int x, int y)
    {
        List<Tile> ret = new List<Tile>();
        
        ret.Add(getTile(x+1, y));
        ret.Add(getTile(x-1, y));
        ret.Add(getTile(x, y-1));
        ret.Add(getTile(x, y+1));

        return ret.Where(t => t != null).ToList();
    }
}