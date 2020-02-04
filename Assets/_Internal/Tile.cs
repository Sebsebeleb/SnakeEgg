
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using _Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;
    
    [SerializeField]
    private Image _image;
    
    public enum State
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Snake,
        Number,
        Empty,
    }

    private class Group
    {
        public List<Tile> tiles = new List<Tile>();

        public Color color;

        public Group()
        {
            this.color = game.takeColor();
        }

        public void Add(Tile tile)
        {
            tiles.Add(tile);
            tile.grp = this;
        }

        public void Remove(Tile tile)
        {
            tiles.Remove(tile);
            tile.grp = null;
            
            splitCheck();

            if (this.tiles.Count == 0)
            {
                game.returnColor(this.color);
            }
        }

        private void splitCheck()
        {
            var all = tiles.ToArray().ToList();

            if (all.Count == 0)
            {
                return;
            }

            var unseen = all.ToArray().ToList();

            unseen.Remove(all[0]);
            var unvisited = new List<Tile>(){all[0]};

            while (unvisited.Count > 0)
            {
                Tile next = unvisited[0];
                unvisited.RemoveAt(0);

                foreach (var check in game.getAdjacentTiles(next.x, next.y))
                {
                    if (unseen.Contains(check))
                    {
                        unseen.Remove(check);
                        
                        unvisited.Add(check);
                    }
                }
            }

            if (unseen.Count > 0)
            {
                Group g = new Group();
                foreach (Tile tile1 in unseen)
                {
                    this.Remove(tile1);
                    g.Add(tile1);
                }
                g.splitCheck();
            }
            

        }

        public void Merge(Group g)
        {
            if (g == this)
            {
                return;
            }
            foreach (var t in g.tiles.ToArray())
            {
                g.Remove(t);
                this.Add(t);
            }
        }
    }

    private Group grp;

    private bool Fixed;

    private State current;

    private static Game game;

    private int x, y;

    private void Start()
    {
        if (game == null)
        {
            game = FindObjectOfType<Game>();
        }
    }

    private void Update()
    {
        if (this.grp != null && this.current == State.Number)
        {
            this._text.text = this.grp.tiles.Count.ToString();
            this._image.color = this.grp.color;
        }
    }

    public void SetPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Set(int number, bool fxed = false)
    {
        doSet((State) number, fxed);
    }

    public void Set(State s, bool fxed = false)
    {
        doSet(s, fxed);
    }

    private void doSet(State s, bool fxed)
    {
        if (!fxed)
        {
            if (this.Fixed)
            {
                return;
            }
        }
        this.Fixed = fxed;


        if (s != current)
        {
            SfxSystem.TriggerSetTile();    
        }

        current = s;

        if (s == State.Snake)
        {
            
            _text.text = "";
            _image.color = Color.grey;
            if (this.Fixed)
            {
                _image.color = new Color(0.62f, 0.56f, 0.64f);
            }

            if (this.grp != null)
            {
                this.grp.Remove(this);
            }
        }
        else if (s == State.Empty)
        {
            _text.text = "";
            _image.color = Color.white;
            
            if (this.grp != null)
            {
                this.grp.Remove(this);
            }
        }
        else
        {
            _text.text = ((int)s+1).ToString();
            _image.color = Color.white;
            if (this.Fixed)
            {
                _text.color = new Color(0.42f, 0.37f, 0.43f);
            }

            if (game == null)
            {
                return;
            }
            
            var adj = game.getAdjacentTiles(this.x, this.y);

            foreach (var a in adj)
            {
                if (a.grp != null)
                {
                    if (this.grp != null && this.grp != a.grp)
                    {
                        this.grp.Merge(a.grp);
                    }

                    if (this.grp == null)
                    {
                        a.grp.Add(this);    
                    }
                }
            }

            if (this.grp == null)
            {
                this.grp = new Group();
                this.grp.Add(this);
            }
        }
    }
}
