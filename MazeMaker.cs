using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Text;

namespace Maze
{
    class Maze
    {
        public List<List<Cell>> cells; //Map of all cells in maze
        public int size; //User input square root of map
        public enum Direction { UP, DOWN, LEFT, RIGHT, NULL } //All possible directions of travel from any cell

        //Constructor: create maze of size*size. Initialize map of cells
        public Maze(int s)
        {
            size = s;
            cells = new List<List<Cell>>();
            for(int i = 0; i < size; i++)
            {
                cells.Add(new List<Cell>());
                for(int j = 0; j < size; j++)
                {
                    Cell cell = new Cell(i, j);
                    cells[i].Add(cell);
                }
            }

            makeMaze();
        }

        // Algorithmically create maze
        public void makeMaze()
        {
            Direction direction = Direction.NULL; // direction to the next cell
            Direction traveledFrom = Direction.NULL; // direction from previous cell
            List<Direction> openDirections = new List<Direction>(); //list of all possible directions from present cell
            Random random = new Random();
            int numberOfVisitedCells = 1;
            int x = random.Next(size-1);
            int y = random.Next(size-1);
            cells[x][y].visited = true;
            Cell currentCell = cells[x][y];
            
            
            // Until every cell is accessible in the maze, loop.
            while (numberOfVisitedCells < size*size-1)
            {
                
                if(currentCell.X != 0)
                {
                    //can go left
                    if(!cells[x - 1][y].visited)
                        openDirections.Add(Direction.LEFT);
                }
                if(currentCell.X != size - 1)
                {
                    //can go right
                    if (!cells[x + 1][y].visited)
                        openDirections.Add(Direction.RIGHT);
                }
                if(currentCell.Y != 0)
                {
                    //can go down
                    if (!cells[x][y - 1].visited)
                        openDirections.Add(Direction.DOWN);
                }
                if(currentCell.Y != size - 1)
                {
                    //can go up
                    if (!cells[x][y + 1].visited)
                        openDirections.Add(Direction.UP);
                }

                //Make sure not to revisit the same cell
                openDirections.Remove(traveledFrom);

                //If there are no possible moves, find another unvisited space to start from
                if(openDirections.Count == 0)
                {
                    bool escape = false;
                    for(int i = 0; i < size; i++)
                    {
                        if (escape)
                            break;
                        for(int j = 0; j < size; j++)
                        {
                            if (escape)
                                break;
                            if (!cells[i][j].visited)
                            {
                                x = i;
                                y = j;
                                cells[x][y].visited = true;
                                numberOfVisitedCells++;
                                currentCell = cells[x][y];
                                escape = true;
                                if (y != size - 1)
                                    openDirections.Add(Direction.UP);
                                if (y != 0)
                                    openDirections.Add(Direction.DOWN);
                                if (x != size - 1)
                                    openDirections.Add(Direction.RIGHT);
                                if (x != 0)
                                    openDirections.Add(Direction.LEFT);
                            }
                        }
                    }
                }
                if (openDirections.Count > 1)
                    direction = openDirections[random.Next(1, openDirections.Count)];
                else if (openDirections.Count == 1)
                {
                    direction = openDirections[0];
                }
                else
                    continue;
                
                switch (direction)
                {
                    case Direction.UP:
                        cells[x][y].walls[0].openYN = true;
                        x = currentCell.X;
                        y = currentCell.Y + 1;
                        cells[x][y].visited = true;
                        numberOfVisitedCells++;
                        cells[x][y].walls[2].openYN = true;
                        traveledFrom = Direction.DOWN;
                        break;
                    case Direction.DOWN:
                        cells[x][y].walls[2].openYN = true;
                        x = currentCell.X;
                        y = currentCell.Y - 1;
                        cells[x][y].visited = true;
                        numberOfVisitedCells++;
                        cells[x][y].walls[0].openYN = true;
                        traveledFrom = Direction.UP;
                        break;
                    case Direction.LEFT:
                        cells[x][y].walls[3].openYN = true;
                        x = currentCell.X - 1;
                        y = currentCell.Y;
                        cells[x][y].visited = true;
                        numberOfVisitedCells++;
                        cells[x][y].walls[1].openYN = true;
                        traveledFrom = Direction.RIGHT;
                        break;
                    case Direction.RIGHT:
                        cells[x][y].walls[1].openYN = true;
                        x = currentCell.X + 1;
                        y = currentCell.Y;
                        cells[x][y].visited = true;
                        numberOfVisitedCells++;
                        cells[x][y].walls[3].openYN = true;
                        traveledFrom = Direction.LEFT;
                        break;
                }
                currentCell = cells[x][y];
                openDirections.Clear();
            }
        }

        public Wall[] GetWalls(int x, int y)
        {
            return cells[x][y].walls;
        }
    }

    class Wall
    {
        public bool openYN { get; set; }

        public Wall()
        {
            openYN = false;
        }
    }

    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public bool visited { get; set; }

        public Wall[] walls;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            walls = new Wall[4];
            walls[0] = new Wall();
            walls[1] = new Wall();
            walls[2] = new Wall();
            walls[3] = new Wall();
            visited = false;
        }
    }
}
