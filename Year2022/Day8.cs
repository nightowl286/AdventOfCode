using System.Diagnostics;

namespace AoC.Year2022
{
   public class Day8 : Day
   {
      #region Types
      private record Tree(Position Position, int Height)
      {
         #region Properties
         public bool Visible { get; set; }
         public int ViewScore { get; set; }
         #endregion
      }
      private record Position(int X, int Y)
      {
         public Position Add(Position other) => new Position(X + other.X, Y + other.Y);
      }
      private class Grid
      {
         #region Fields
         private readonly Tree[,] _trees;
         #endregion

         #region Indexer
         public Tree this[int x, int y]
         {
            get => _trees[x, y];
            set => _trees[x, y] = value;
         }
         public Tree this[Position position]
         {
            get => _trees[position.X, position.Y];
            set => _trees[position.X, position.Y] = value;
         }
         #endregion
         public Grid(Tree[,] trees)
         {
            _trees = trees;
         }

         #region Methods
         public IEnumerable<IEnumerable<Tree>> AsRows()
         {
            int height = _trees.GetLength(1);
            for (int y = 0; y < height; y++)
               yield return Row(y);
         }
         private IEnumerable<Tree> Row(int y)
         {
            int width = _trees.GetLength(0);
            for (int x = 0; x < width; x++)
               yield return this[x, y];
         }
         public IEnumerable<Tree> All()
         {
            int width = _trees.GetLength(0);
            int height = _trees.GetLength(1);
            for (int x = 0; x < width; x++)
            {
               for (int y = 0; y < height; y++)
               {
                  yield return this[x, y];
               }
            }
         }
         public IEnumerable<Tree> ToLeft(Tree tree) => InLine(tree.Position, -1, 0);
         public IEnumerable<Tree> ToRight(Tree tree) => InLine(tree.Position, 1, 0);
         public IEnumerable<Tree> ToTop(Tree tree) => InLine(tree.Position, 0, -1);
         public IEnumerable<Tree> ToBottom(Tree tree) => InLine(tree.Position, 0, 1);
         public IEnumerable<Tree> Edges() => All().Where(t => OnEdge(t.Position));
         public bool OnEdge(Position position)
         {
            bool edgeX = position.X == 0 || position.X == _trees.GetLength(0) - 1;
            bool edgeY = position.Y == 0 || position.Y == _trees.GetLength(1) - 1;

            return edgeX || edgeY;
         }
         private bool InGrid(Position position)
         {
            bool inX = position.X >= 0 && position.X < _trees.GetLength(0);
            bool inY = position.Y >= 0 && position.Y < _trees.GetLength(1);

            return inX && inY;
         }
         private IEnumerable<Tree> InLine(Position start, int dirX, int dirY)
            => InLine(start, new Position(dirX, dirY));
         private IEnumerable<Tree> InLine(Position start, Position direction)
         {
            Position position = start.Add(direction);
            while (InGrid(position))
            {
               yield return this[position];
               position = position.Add(direction);
            }
         }
         public IEnumerable<IEnumerable<Tree>> Directions(Tree tree)
         {
            yield return ToLeft(tree);
            yield return ToTop(tree);
            yield return ToRight(tree);
            yield return ToBottom(tree);
         }
         #endregion
      }
      #endregion

      #region Fields
      private Grid _grid = null!;
      #endregion

      #region Part 1
      public override void Part1Setup() => LoadGrid();
      public override object? Part1()
      {
         CalculateVisibility();

         int visible = _grid.All().Count(t => t.Visible);

         return visible;
      }
      #endregion

      #region Part 2
      public override void Part2Setup() => LoadGrid();
      public override object? Part2()
      {
         CalculateViewScore();
         int best = _grid.All().Max(t => t.ViewScore);

         return best;
      }
      #endregion

      #region Methods
      #region Debug
      [Conditional("DEBUG")]
      private void PrintHeight() => PrintTree(t => t.Height.ToString());
      [Conditional("DEBUG")]
      private void PrintVisibility() => PrintTree(t => t.Visible ? "t" : "_");

      [Conditional("DEBUG")]
      private void PrintTree(Func<Tree, string> selector)
      {
         foreach (IEnumerable<Tree> row in _grid.AsRows())
         {
            foreach (Tree tree in row)
            {
               string ch = selector(tree);
               Debug.Write(ch);
            }
            Debug.WriteLine("");
         }
      }
      #endregion
      private void CalculateViewScore()
      {
         foreach (Tree tree in _grid.All())
         {
            int score = 1;
            foreach(IEnumerable<Tree> direction in _grid.Directions(tree))
            {
               int distance = 0;
               foreach (Tree subTree in direction)
               {
                  distance++;
                  if (subTree.Height >= tree.Height)
                     break;
               }
               score *= distance;
            }

            tree.ViewScore = score;
         }
      }
      private void CalculateVisibility()
      {
         foreach (Tree edge in _grid.Edges())
            edge.Visible = true;

         foreach (Tree tree in _grid.All().Where(t => !t.Visible))
         {
            foreach (IEnumerable<Tree> direction in _grid.Directions(tree))
            {
               int highest = direction.Max(t => t.Height);
               if (highest < tree.Height)
               {
                  tree.Visible = true;
                  break;
               }
            }
         }
      }
      private void LoadGrid()
      {
         int height = Input.Count;
         Debug.Assert(height > 0);
         int width = Input[0].Length;
         Debug.Assert(width > 0);
         Tree[,] trees = new Tree[width, height];

         int y = 0;
         foreach (string line in Input)
         {
            int x = 0;
            foreach (char ch in line)
            {
               int treeHeight = int.Parse(ch.ToString());
               Position pos = new Position(x, y);
               Tree tree = new Tree(pos, treeHeight);
               trees[x, y] = tree;

               x++;
            }
            y++;
         }

         _grid = new Grid(trees);
      }
      #endregion

   }
}