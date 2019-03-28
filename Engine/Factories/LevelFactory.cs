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
            int channelMinLength = 3;
            int channelMaxWidth = 3;
            int channelMinWidth = 1;

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
            LinkedList<Rectangle> acceptedRooms = new LinkedList<Rectangle>();
            LinkedList<Rectangle> acceptedChannels = new LinkedList<Rectangle>();

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
                Rectangle currentRoom = rooms.Dequeue();
                // 0 - Вверх, 1 - вправо, 2 - вниз, 3 - влево
                for (int direction = 0; direction < 4; direction++)
                {
                    int trysCount = 0;
                    int successCount = 0;

                    while (trysCount < makeBranchMaxTrys && successCount < branchesPerRoomSide)
                    {
                        int channelLength = RandomSingle.Instanse.Next(channelMinLength, channelMaxLength + 1);
                        int channelWidth = RandomSingle.Instanse.Next(channelMinWidth, channelMaxWidth + 1);

                        Rectangle newChannel = Rectangle.Empty;
                        switch (direction)
                        {
                            case 0:
                                newChannel = new Rectangle(
                                    RandomSingle.Instanse.Next(currentRoom.Left, currentRoom.Right - channelWidth),
                                    currentRoom.Top - channelLength,
                                    channelWidth,
                                    channelLength
                                );
                                break;
                            case 1:
                                newChannel = new Rectangle(
                                    currentRoom.Right,
                                    RandomSingle.Instanse.Next(currentRoom.Top, currentRoom.Bottom - channelWidth),
                                    channelLength,
                                    channelWidth
                                );
                                break;
                            case 2:
                                newChannel = new Rectangle(
                                    RandomSingle.Instanse.Next(currentRoom.Left, currentRoom.Right - channelWidth),
                                    currentRoom.Bottom,
                                    channelWidth,
                                    channelLength
                                );
                                break;
                            case 3:
                                newChannel = new Rectangle(
                                    currentRoom.Left - channelLength,
                                    RandomSingle.Instanse.Next(currentRoom.Top, currentRoom.Bottom - channelWidth),
                                    channelLength,
                                    channelWidth
                                );
                                break;
                        }

                        if (acceptedRooms.Any(c => c.Intersects(newChannel)) 
                            || acceptedChannels.Any(c => c.Intersects(newChannel))
                            || newChannel.Top < levelBounds.Top
                            || newChannel.Bottom > levelBounds.Bottom
                            || newChannel.Left < levelBounds.Left
                            || newChannel.Right > levelBounds.Right)
                        {
                            trysCount++;
                            continue;
                        }

                        Point roomSize = new Point(
                                RandomSingle.Instanse.Next(roomSideMinLenght, roomSideMaxLength + 1),
                                RandomSingle.Instanse.Next(roomSideMinLenght, roomSideMaxLength + 1)
                            );

                        Rectangle newRoom = Rectangle.Empty;
                        switch(direction)
                        {
                            case 0:
                                newRoom = new Rectangle(
                                    RandomSingle.Instanse.Next(newChannel.Left - (roomSize.X - channelWidth), newChannel.Left),
                                    newChannel.Top - roomSize.Y,
                                    roomSize.X,
                                    roomSize.Y
                                );
                                break;
                            case 1:
                                newRoom = new Rectangle(
                                    newChannel.Right,
                                    RandomSingle.Instanse.Next(newChannel.Top - (roomSize.Y - channelWidth), newChannel.Top),
                                    roomSize.X,
                                    roomSize.Y
                                );
                                break;
                            case 2:
                                newRoom = new Rectangle(
                                    RandomSingle.Instanse.Next(newChannel.Left - (roomSize.X - channelWidth), newChannel.Left),
                                    newChannel.Bottom,
                                    roomSize.X,
                                    roomSize.Y
                                );
                                break;
                            case 3:
                                newRoom = new Rectangle(
                                    newChannel.Left - roomSize.X,
                                    RandomSingle.Instanse.Next(newChannel.Top - (roomSize.Y - channelWidth), newChannel.Top),
                                    roomSize.X,
                                    roomSize.Y
                                );
                                break;
                        }

                        if (acceptedRooms.Any(r => r.Intersects(newRoom)) 
                            || acceptedChannels.Any(c => c.Intersects(newRoom))
                            || newRoom.Top < levelBounds.Top
                            || newRoom.Bottom > levelBounds.Bottom
                            || newRoom.Left < levelBounds.Left
                            || newRoom.Right > levelBounds.Right)
                        {
                            trysCount++;
                            continue;
                        }

                        trysCount = 0;
                        successCount++;

                        rooms.Enqueue(newRoom);
                        acceptedChannels.AddLast(newChannel);
                        acceptedRooms.AddLast(newRoom);
                    }
                }
            }

            foreach (Rectangle room in acceptedRooms)
            {
                FillRectangle(level, "testAtlas", new Color(
                    RandomSingle.Instanse.Next(0, 255),
                    RandomSingle.Instanse.Next(0, 255),
                    RandomSingle.Instanse.Next(0, 255)
                ), room, 1);
            }

            foreach (Rectangle channel in acceptedChannels)
            {
                FillRectangle(level, "testAtlas", new Color(
                    RandomSingle.Instanse.Next(0, 255),
                    RandomSingle.Instanse.Next(0, 255),
                    RandomSingle.Instanse.Next(0, 255)
                ), channel, 1);
            }
        }
    }
}
