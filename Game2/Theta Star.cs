using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
        class AStar //Variant on Dijkstra's Algorithm - faster
        {
            public static Node[,] Grid; //Using the data type created below to make a grid, a 2 dimensional array of nodes (squares)

            public static List<Node> FindPath(Point start, Point end, Map map) //Finds the fastest route from the start to end
            {
                Grid = Node.MakeGrid(map);
                //Checks that we aren't trying to make a path from one place to the same place
                if (start.X == end.X && start.Y == end.Y)
                {
                    return null;
                }

                //Resets all the parent variables to clear the paths created last time
                for (int x = 0; x <= Grid.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= Grid.GetUpperBound(1); y++)
                    {
                        Grid[x, y].Parent = null;
                    }
                }

                List<Node> OpenList = new List<Node>(); //Nodes to be considered, ones that may be on the path
                List<Node> ClosedList = new List<Node>(); //Explored nodes
                Point CurrentPoint = new Point(0, 0);
                Node Current = null;
                List<Node> Path = null;

                OpenList.Add(Grid[start.X, start.Y]); //Add the starting point to OpenList

                while (OpenList.Count > 0)
                {
                    //Explores for the "best" choice in the Openlist
                    Current = OpenList[0];
                    CurrentPoint.X = Current.X;
                    CurrentPoint.Y = Current.Y;
                    if (CurrentPoint == end) break; //If we have reached the end

                    OpenList.RemoveAt(0); //Removes the starting point
                    ClosedList.Add(Current);

                    foreach (Node neighbour in GetNeighbours(CurrentPoint)) //Checks all the squares adjacent to the current point
                    {
                        //Skips fully explored nodes which have been explored fully
                        if (ClosedList.Contains(neighbour)) continue;

                        //Skips the node if it's a wall
                        if (neighbour.IsWall) continue;

                        //If parent is null, it's our first visit to the node
                        if (neighbour.Parent == null)
                        {
                            neighbour.G = Current.G + 10; //10 is the cost for each horizontal or vertical node moved
                            neighbour.Parent = Current; //Where it came from, final path can be found by linking parents

                            //The following way of calculating the H value is called the Manhattan method, it ignores any obstacles
                            neighbour.H = Math.Abs(neighbour.X - end.X)
                                + Math.Abs(neighbour.Y - end.Y); //Calculates total cost by combining the X distance by the Y
                            neighbour.H *= 10; //Then multiply H by 10 (The cost movement for each square)
                            OpenList.Add(neighbour);
                        }
                        else
                        {
                            //Is this a more efficient route than last time?
                            if (Current.G + 10 < neighbour.G)
                            {
                                neighbour.Parent = Current;
                                neighbour.G = Current.G + 10;
                            }
                        }
                    }

                    OpenList.Sort(); //This uses the IComparible interface and CompareWith() method to sort
                }

                //If we finished, end will have a parent, otherwise not
                Path = new List<Node>();
                Current = Grid[end.X, end.Y]; //Current = end desination node
                Path.Add(Current);
                while (Current.Parent != null) //Won't run if end doesn't have a parent
                {
                    Path.Add(Current.Parent);
                    Current = Current.Parent;
                }

                //Path.Reverse();
                //.reverse() (Above) is replaced with the below code
                for (int i = 0; i < Path.Count() / 2; i++)
                {
                    Node Temp = Path[i];
                    Path[i] = Path[Path.Count() - i - 1];
                    Path[Path.Count() - i - 1] = Temp;
                }

                //Have we found a path or used all our options?
                return OpenList.Count > 1 ? Path : null;
                //Below replaced by Ternary Statement above
                /*if (OpenList.Count > 1)
                    return Path;
                else
                    return null;*/
            }

            private static List<Node> GetNeighbours(Point p)
            {
                List<Node> Result = new List<Node>();

                if (p.X - 1 >= 0)
                {
                    Result.Add(Grid[p.X - 1, p.Y]);
                }

                if (p.X < Grid.GetUpperBound(0))
                {

                    Result.Add(Grid[p.X + 1, p.Y]);
                }

                if (p.Y - 1 >= 0)
                {
                    Result.Add(Grid[p.X, p.Y - 1]);
                }

                if (p.Y < Grid.GetUpperBound(1))
                {
                    Result.Add(Grid[p.X, p.Y + 1]);
                }

                return Result;
            }
        }
        class Node : IComparable<Node> //Inherits from the interface IComparable, allows the nodes to be sorted using .sort()
        {
            public int X; //Position on the X axis
            public int Y; //Position on the Y axis
            public Node Parent;
            public bool IsWall;
            public int G; //The amount needed to move from the starting node to the given other
            public int H; //The estimated cost to move from that given node to the end point - Called Heuristic (Because it's a guess)
            public int F //G+H
            {
                get { return G + H; } //Automatically calculates the latest value when F is accessed
            }

            /*This must be added due to IComparable (It requires a method called "CompareTo" to work)
                - specifies how to sort the nodes*/
            public int CompareTo(Node other)
            {
                if (this.F < other.F) return -1;
                else if (this.F == other.F) return 0;
                else return 1;
            }



            public Node Clone(bool ResetGHandParent)
            {
                Node Result = new Node();
                Result.X = this.X;
                Result.Y = this.Y;
                Result.IsWall = this.IsWall;
                if (ResetGHandParent)
                {
                    Result.G = 0;
                    Result.H = 0;
                    Result.Parent = null;
                }
                else
                {
                    Result.G = this.G;
                    Result.H = this.H;
                    Result.Parent = this.Parent;
                }
                return Result;
            }

            public Node Clone()
            {
                Node Result = new Node();
                Result.X = this.X;
                Result.Y = this.Y;
                Result.IsWall = this.IsWall;
                Result.G = this.G;
                Result.H = this.H;
                return Result;
            }


            public static Node[,] MakeGrid(Map map)
            {
                Node[,] Result = new Node[map.width, map.height];
                Random r = new Random();

                for (int x = 0; x < map.width; x++)
                {
                    for (int y = 0; y < map.height; y++)
                    {
                        Result[x, y] = new Node();
                        Result[x, y].Parent = null;
                        Result[x, y].X = x;
                        Result[x, y].Y = y;
                        Result[x, y].G = 0;
                        Result[x, y].H = 0;
                        //This takes advantage of the fact that < is a comparison operator, and will give a boolean result
                        if (map[x, y] == 1) Result[x, y].IsWall = true;
                    }
                }

                return Result;
            }
        }
}

