using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueNeverDie.Engine.GameObjects;

namespace RogueNeverDie.Engine.Factories
{
    public class LevelFactory
    {
        public LevelFactory(ResourceManager ResourceManager, TileFactory TileFactory)
        {
            _resourceManager = ResourceManager;
            _tileFactory = TileFactory;
        }

        protected ResourceManager _resourceManager;
        protected TileFactory _tileFactory;

        protected void MakeRoom(Point size, int wallWidth, bool outerBorder)
        {

        }

        public void FillRectangle(Level level, string atlas, Rectangle field, int layer, bool outerBorder = false)
        {
            IReadOnlyDictionary<Point, Tile> tileGrid = level.TileGrid;

            for (int x = field.Left; x < field.Right; x++) {
                for (int y = field.Top; y < field.Bottom; y++)
                {
                    if (outerBorder)
                    {
                        if (x == field.Left && y == field.Top)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, false, false, false, false, true, true, false, true, true));
                        }
                        else if (x == field.Right - 1 && y == field.Top)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, false, false, false, true, true, false, true, true, false));
                        }
                        else if (x == field.Left && y == field.Bottom - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, false, true, true, false, true, true, false, false, false));
                        }
                        else if (x == field.Right - 1 && y == field.Bottom - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, true, true, false, true, true, false, false, false, false));
                        }
                        else if (x == field.Left)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, false, true, true, false, true, true, false, true, true));
                        }
                        else if (x == field.Right - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, true, true, false, true, true, false, true, true, false));
                        }
                        else if (y == field.Top)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, false, false, false, true, true, true, true, true, true));
                        }
                        else if (y == field.Bottom - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f, true, true, true, true, true, true, false, false, false));
                        }
                        else
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f));
                        }
                    }
                    else
                    {
                        level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, Color.Red, 0.5f));
                    }
                }
            }
        }
    }
}
