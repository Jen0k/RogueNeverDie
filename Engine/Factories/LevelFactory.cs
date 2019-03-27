using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RogueNeverDie.Engine.GameObjects;
using RogueNeverDie.Engine.Common;

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

        public void FillRectangle(Level level, string atlas, Color color, Rectangle field, int layer, bool outerBorder = false)
        {
            IReadOnlyDictionary<Point, Tile> tileGrid = level.TileGrid;

            for (int x = field.Left; x < field.Right; x++) {
                for (int y = field.Top; y < field.Bottom; y++)
                {
                    if (outerBorder)
                    {
                        if (x == field.Left && y == field.Top)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                false, false, false, 
                                                                                false, true, true, 
                                                                                false, true, true));
                        }
                        else if (x == field.Right - 1 && y == field.Top)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                false, false, false, 
                                                                                true, true, false, 
                                                                                true, true, false));
                        }
                        else if (x == field.Left && y == field.Bottom - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                false, true, true, 
                                                                                false, true, true, 
                                                                                false, false, false));
                        }
                        else if (x == field.Right - 1 && y == field.Bottom - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                true, true, false, 
                                                                                true, true, false, 
                                                                                false, false, false));
                        }
                        else if (x == field.Left)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                false, true, true, 
                                                                                false, true, true, 
                                                                                false, true, true));
                        }
                        else if (x == field.Right - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                true, true, false, 
                                                                                true, true, false, 
                                                                                true, true, false));
                        }
                        else if (y == field.Top)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                false, false, false, 
                                                                                true, true, true, 
                                                                                true, true, true));
                        }
                        else if (y == field.Bottom - 1)
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color, 
                                                                                true, true, true,
                                                                                true, true, true, 
                                                                                false, false, false));
                        }
                        else
                        {
                            level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color));
                        }
                    }
                    else
                    {
                        level.TileGrid[new Point(x, y)].SetLayer(layer, _tileFactory.CreateLayer(atlas, color));
                    }
                }
            }
        }

        public void GenerateNonRegularDungeon(Level level)
        {
            int roomSideMaxLength = 25;
            int roomSideMinLenght = 10;
            int channelMaxLength = 20;
            int channelMaxWidth = 3;

            int makeBranchMaxTrys = 5;
            int branchesPerRoomSide = 3;

            int levelMinX = level.TileGrid.Keys.Min(x => x.X);
            int levelMinY = level.TileGrid.Keys.Min(y => y.Y);
            Rectangle levelBounds = new Rectangle(
                    levelMinX,
                    levelMinY,
                    level.TileGrid.Keys.Max(x => x.X) - levelMinX + 1,
                    level.TileGrid.Keys.Max(y => y.Y) - levelMinY + 1
                );

            Dictionary<Point, bool> pathMatrix = new Dictionary<Point, bool>(level.TileGrid.Count);

            Queue<Rectangle> rooms = new Queue<Rectangle>();

            int newRoomWidth = Math.Min(levelBounds.Width, RandomSingle.Instanse.Next(roomSideMinLenght, roomSideMaxLength));
            int newRoomHeigth = Math.Min(levelBounds.Height, RandomSingle.Instanse.Next(roomSideMinLenght, roomSideMaxLength));
            Rectangle firstRoom = new Rectangle(
                    RandomSingle.Instanse.Next(0, levelBounds.Right - newRoomWidth),
                    RandomSingle.Instanse.Next(0, levelBounds.Bottom - newRoomHeigth),
                    newRoomWidth,
                    newRoomHeigth
                );
            rooms.Enqueue(firstRoom);

            while(rooms.Count > 0)
            {

            }
        }
    }
}
