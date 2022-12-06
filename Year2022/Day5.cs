using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AoC.Year2022
{
   public class Day5 : Day
   {
      #region Types
      private class Stack : Stack<char> { }
      private record Board(List<Stack> Stacks);
      private record Instruction(int Amount, int From, int To);
      #endregion

      #region Fields
      private Board _board = null!;
      private List<Instruction> _instructions = null!;
      private class Indices : Dictionary<int, int> { }
      #endregion

      #region Part 1
      public override void Part1Setup() => SetBoardAndInstructions();
      public override object? Part1()
      {
         ExecuteInstructions(false);
         string top = "";

         foreach (Stack stack in _board.Stacks)
            top += stack.Peek();

         return top;
      }
      #endregion

      #region Part 2
      public override void Part2Setup() => SetBoardAndInstructions();
      public override object? Part2()
      {
         ExecuteInstructions(true);
         string top = "";
         foreach (Stack stack in _board.Stacks)
            top += stack.Peek();

         return top;
      }
      #endregion

      #region Methods
      private void ExecuteInstructions(bool crateMover9001)
      {
         foreach (Instruction instruction in _instructions)
         {
            if (crateMover9001 && instruction.Amount > 1)
               ExecuteInstruction9001(instruction);
            else
               ExecuteInstruction9000(instruction);
         }
      }
      private void ExecuteInstruction9001(Instruction instruction)
      {
         int amt = instruction.Amount;
         int from = instruction.From - 1;
         int to = instruction.To - 1;

         Stack tempStack = new Stack();
         for (int i = 0; i < amt; i++)
         {
            char cargo = _board.Stacks[from].Pop();
            tempStack.Push(cargo);
         }

         foreach (char cargo in tempStack)
            _board.Stacks[to].Push(cargo);
      }

      private void ExecuteInstruction9000(Instruction instruction)
      {
         for (int i = 0; i < instruction.Amount; i++)
         {
            int from = instruction.From - 1;
            int to = instruction.To - 1;

            char ch = _board.Stacks[from].Pop();
            _board.Stacks[to].Push(ch);
         }
      }
      private void SetBoardAndInstructions()
      {
         string[] parts = RawInput.Split("\n\n");
         Debug.Assert(parts.Length == 2, "Main input must be in 2 parts.");

         SetBoard(parts[0]);
         SetInstructions(parts[1]);
      }
      private void SetBoard(string data)
      {
         List<Stack> stacks = new List<Stack>();
         IEnumerable<string> reverseLines = data
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Reverse();

         string first = reverseLines.First();
         reverseLines = reverseLines.Skip(1);

         Indices indices = GetIndices(first);
         for (int i = 0; i < indices.Count; i++)
            stacks.Add(new Stack());

         foreach (string line in reverseLines)
         {
            MatchCollection matches = Regex.Matches(line, "\\w");
            foreach (Match match in matches)
            {
               Debug.Assert(indices.ContainsKey(match.Index), $"Indices does not contain index {match.Index}.");
               int index = indices[match.Index];

               Debug.Assert(match.Value.Length == 1, $"Each cargo must be one letter, instead got {match.Value}");
               stacks[index].Push(match.Value[0]);
            }
         }

         _board = new Board(stacks);
      }
      private Indices GetIndices(string line)
      {
         MatchCollection matches = Regex.Matches(line, "\\d");
         Indices indices = new Indices();
         for (int i = 0; i < matches.Count; i++)
         {
            Match match = matches[i];
            indices.Add(match.Index, i);
         }

         return indices;
      }
      private void SetInstructions(string data)
      {
         List<Instruction> instructions = new List<Instruction>();

         string[] lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
         foreach (string line in lines)
         {
            MatchCollection matches = Regex.Matches(line, "\\d+");
            Debug.Assert(matches.Count == 3, $"Instruction must have 3 numbers, instead got {matches.Count} in line ({line})");
            int[] nums = matches.Select(m => int.Parse(m.Value)).ToArray();

            Instruction instruction = new Instruction(nums[0], nums[1], nums[2]);
            instructions.Add(instruction);
         }

         _instructions = instructions;
      }
      #endregion
   }
}