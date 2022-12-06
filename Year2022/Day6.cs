namespace AoC.Year2022
{
   public class Day6 : Day
   {
      public override object? Part1() => Magic(4);
      public override object? Part2() => Magic(14);

      #region Methods
      private object Magic(int length)
      {
         ReadOnlySpan<char> input = RawInput.AsSpan();
         for (int i = 0; i < input.Length; i++)
         {
            ReadOnlySpan<char> span = input[i..];
            if (Distinct(span, length))
               return i + length;
         }

         throw new Exception("Well this failed.");
      }
      private static bool Distinct(ReadOnlySpan<char> span, int length)
      {
         int len = Math.Min(span.Length, length);
         for (int i = 0; i < len; i++)
         {
            for (int j = i + 1; j < len; j++)
            {
               if (span[i] == span[j])
                  return false;
            }
         }

         return true;
      }
      #endregion
   }
}