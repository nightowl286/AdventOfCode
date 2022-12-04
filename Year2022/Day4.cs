using System.Diagnostics;

namespace AoC.Year2022
{
   public class Day4 : Day
   {
      #region Types
      private record Section(int Start, int End);

      private record Pair(Section Section1, Section Section2);
      #endregion

      #region Fields
      private List<Pair> _pairs = null!;
      #endregion

      #region Part 1
      public override void Part1Setup() => GetPairs();
      public override object? Part1()
      {
         int total = 0;
         foreach (Pair pair in _pairs)
         {
            if (OverlapFully(pair))
               total++;
         }
         return total;
      }
      #endregion

      #region Part 2
      public override void Part2Setup() => GetPairs();
      public override object? Part2()
      {
         int total = 0;
         foreach(Pair pair in _pairs)
         {
            if (OverlapAny(pair))
               total++;
         }

         return total;
      }
      #endregion

      #region Methods
      private static bool OverlapAny(Pair pair) => EitherOverlapAny(pair.Section1, pair.Section2);
      private static bool EitherOverlapAny(Section s1, Section s2) => OverlapAny(s1, s2) || OverlapAny(s2, s1);
      private static bool OverlapAny(Section s1, Section s2)
      {
         bool startOverlaps = (s2.Start >= s1.Start) && (s2.Start <= s1.End);
         bool endOverlaps = (s2.End >= s1.Start) && (s2.End <= s1.End);

         return startOverlaps || endOverlaps;
      }
      private static bool OverlapFully(Pair pair) => EitherOverlapFully(pair.Section1, pair.Section2);
      private static bool EitherOverlapFully(Section s1, Section s2) => OverlapFully(s1, s2) || OverlapFully(s2, s1);
      private static bool OverlapFully(Section s1, Section s2) => s2.Start >= s1.Start && s2.End <= s1.End;
      private void GetPairs()
      {
         static Pair Convert(string line)
         {
            static Section Convert(string part)
            {
               string[] parts = part.Split('-');
               Debug.Assert(parts.Length == 2);

               return new Section(int.Parse(parts[0]), int.Parse(parts[1]));
            }

            string[] parts = line.Split(',');
            Debug.Assert(parts.Length == 2);

            Section s1 = Convert(parts[0]);
            Section s2 = Convert(parts[1]);

            return new Pair(s1, s2);
         }

         _pairs = ConvertLines(Convert);
      }
      #endregion


   }
}