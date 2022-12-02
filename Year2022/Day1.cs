namespace AoC.Year2022
{
   public class Day1 : Day
   {
      private int[] _sums = null!;

      public override void Part1Setup() => SetSums();
      public override object? Part1()
      {
         int max = _sums
            .OrderByDescending(n => n)
            .First();

         return max;
      }

      public override void Part2Setup() => SetSums();
      public override object? Part2()
      {
         int top3Total = _sums
            .OrderByDescending(n => n)
            .Take(3)
            .Sum();

         return top3Total;
      }

      private void SetSums()
      {
         string[] lineGroups = RawInput.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
         List<int[]> groups = new List<int[]>();
         foreach (string group in lineGroups)
         {
            int[] nums = group.Split('\n', StringSplitOptions.RemoveEmptyEntries)
               .Select(int.Parse).ToArray();

            groups.Add(nums);
         }

         List<int> sums = new List<int>();
         foreach (int[] group in groups)
            sums.Add(group.Sum());

         _sums = sums.ToArray();
      }
   }
}