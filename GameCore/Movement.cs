namespace Game2048
{
    internal class Movement
    {
        public delegate void UpdateScore(int score);
        public static event UpdateScore EvnUpdateScore;

        public static void MoveTilesLeft(Tile[,] tiles, ref bool isBlock)
        {
            for (var j = 0; j < Game.GRID_SIZE; j++)
            {
                for (var i = 1; i < Game.GRID_SIZE; i++)
                {
                    Tile tile = tiles[i, j];
                    if (tile.Value > 0)
                    {
                        var targetX = i;
                        while (targetX > 0 && tiles[targetX - 1, j].Value == 0)
                        {
                            targetX--;
                        }
                        //Проверка на объединение
                        if (targetX > 0 && tiles[targetX - 1, j].Value == tile.Value)
                        {
                            //Объединение
                            tiles[targetX - 1, j].Value *= 2;
                            tiles[i, j].Value = 0;
                            isBlock = false;
                            EvnUpdateScore?.Invoke(tiles[targetX - 1, j].Value);
                        }
                        else if (targetX != i)
                        {
                            tiles[targetX, j] = tile;
                            isBlock = true;
                            tiles[i, j] = new Tile();
                        }
                    }
                }
            }
        }

        public static void MoveTilesRight(Tile[,] tiles, ref bool isBlock)
        {
            for (var j = 0; j < Game.GRID_SIZE; j++)
            {
                for (var i = Game.GRID_SIZE - 2; i >= 0; i--)
                {
                    Tile tile = tiles[i, j];
                    if (tile.Value > 0)
                    {
                        var targetX = i;
                        while (targetX < Game.GRID_SIZE - 1 && tiles[targetX + 1, j].Value == 0)
                        {
                            targetX++;
                        }
                        //Проверка на объединение
                        if (targetX < Game.GRID_SIZE - 1 && tiles[targetX + 1, j].Value == tile.Value)
                        {
                            //Объединение
                            tiles[targetX + 1, j].Value *= 2;
                            tiles[i, j].Value = 0;
                            isBlock = false;
                            EvnUpdateScore?.Invoke(tiles[targetX + 1, j].Value);
                        }
                        else if (targetX != i)
                        {
                            tiles[targetX, j] = tile;
                            tiles[i, j] = new Tile();
                            isBlock = true;
                        }
                    }
                }
            }
        }

        public static void MoveTilesUp(Tile[,] tiles, ref bool isBlock)
        {
            for (var i = 0; i < Game.GRID_SIZE; i++)
            {
                for (var j = 1; j < Game.GRID_SIZE; j++)
                {
                    Tile tile = tiles[i, j];
                    if (tile.Value > 0)
                    {
                        var targetY = j;
                        while (targetY > 0 && tiles[i, targetY - 1].Value == 0)
                        {
                            targetY--;
                        }
                        //Проверка на объединение
                        if (targetY > 0 && tiles[i, targetY - 1].Value == tile.Value)
                        {
                            //Объединение
                            tiles[i, targetY - 1].Value *= 2;                            
                            tiles[i, j].Value = 0;
                            isBlock = false;
                            EvnUpdateScore?.Invoke(tiles[i, targetY - 1].Value);
                        }
                        else if (targetY != j)
                        {
                            tiles[i, targetY] = tile;
                            tiles[i, j] = new Tile();
                            isBlock = true;
                        }
                    }
                }
            }
        }

        public static void MoveTilesDown(Tile[,] tiles, ref bool isBlock)
        {
            for (var i = 0; i < Game.GRID_SIZE; i++)
            {
                for (var j = Game.GRID_SIZE-2; j >= 0; j--)
                {
                    Tile tile = tiles[i, j];
                    if (tile.Value > 0)
                    {
                        var targetY = j;
                        while (targetY < Game.GRID_SIZE-1 && tiles[i, targetY + 1].Value == 0)
                        {
                            targetY++;
                        }
                        //Проверка на объединение
                        if (targetY < Game.GRID_SIZE - 1 && tiles[i, targetY + 1].Value == tile.Value)
                        {
                            //Объединение
                            tiles[i, targetY + 1].Value *= 2;                            
                            tiles[i, j].Value = 0;
                            isBlock = false;
                            EvnUpdateScore?.Invoke(tiles[i, targetY + 1].Value);
                        }
                        else if (targetY != j)
                        {
                            tiles[i, targetY] = tile;
                            tiles[i, j] = new Tile();
                            isBlock = true;
                        }
                    }
                }
            }
        }
    }
}