using System.Diagnostics;

namespace AoC.Year2022
{
   public class Day3 : Day
   {
      #region Types
      private record Sack(char[] Compartment1, char[] Compartment2);
      #endregion

      #region Fields
      private List<Sack> _sacks = null!;
      #endregion

      #region Part 1
      public override void Part1Setup() => LoadSacks();
      public override object? Part1()
      {
         int total = 0;

         foreach (Sack sack in _sacks)
         {
            HashSet<char> first = sack.Compartment1.ToHashSet();
            HashSet<char> second = sack.Compartment2.ToHashSet();

            total += first
              .Concat(second)
              .GroupBy(k => k)
              .ToDictionary(g => g.Key, g => g.Count())
              .Where(p => p.Value > 1)
              .Select(p => p.Key)
              .Select(Priority)
              .Sum();
         }

         return total;
      }
      #endregion

      #region Part 2
      public override void Part2Setup() => LoadSacks();
      public override object? Part2()
      {
         int total = 0;

         IEnumerable<Sack> left = _sacks;
         while (left.Any())
         {
            IEnumerable<Sack> current = left.Take(3).ToArray();
            left = left.Skip(3).ToList();

            List<char[]> group = current
               .Select(
                  s => s
                  .Compartment1
                  .Concat(s.Compartment2)
                  .Distinct()
                  .ToArray())
               .ToList();

            total += group
               .SelectMany(s => s)
               .GroupBy(k => k)
               .ToDictionary(g => g.Key, g => g.Count())
               .Where(p => p.Value == 3)
               .Select(p => p.Key)
               .Select(Priority)
               .Single();
         }

         return total;
      }
      #endregion

      #region Methods
      private void LoadSacks()
      {
         static Sack Convert(string line)
         {
            char[] all = line.ToCharArray();

            Debug.Assert((all.Length & 1) == 0); // Even
            int half = all.Length / 2;

            char[] first = all.Take(half).ToArray();
            char[] second = all.TakeLast(half).ToArray();

            return new Sack(first, second);
         }

         _sacks = ConvertLines(Convert);
      }
      private static int Priority(char ch)
      {
         int priority;
         if (char.IsLower(ch))
         {
            priority = ch - 'a' + 1;
            Debug.Assert(priority >= 1 && priority <= 26, $"Lower priority was {priority}");
         }
         else
         {
            priority = ch - 'A' + 27;
            Debug.Assert(priority >= 27 && priority <= 52, $"Upper priority was {priority}");
         }

         return priority;
      }
      #endregion

   }
}