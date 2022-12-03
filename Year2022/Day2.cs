namespace AoC.Year2022
{
   public class Day2 : Day
   {
      #region Types
      private enum Shape
      {
         Rock,
         Paper,
         Scissors,
      }
      private enum Outcome
      {
         Win, Lose, Draw
      }
      private record Round(Shape Elf, Shape Player);
      #endregion

      #region Consts
      private static Dictionary<string, Shape> Elf { get; } = new Dictionary<string, Shape>()
      {
         { "A", Shape.Rock },
         { "B", Shape.Paper },
         { "C", Shape.Scissors },
      };
      private static Dictionary<string, Shape> Player { get; } = new Dictionary<string, Shape>()
      {
         { "X", Shape.Rock },
         { "Y", Shape.Paper },
         { "Z", Shape.Scissors },
      };
      private static Dictionary<string, Outcome> WantedOutcome { get; } = new Dictionary<string, Outcome>()
      {
         { "X", Outcome.Lose },
         { "Y", Outcome.Draw },
         { "Z", Outcome.Win },
      };
      private static Dictionary<Shape, int> ShapeScores { get; } = new Dictionary<Shape, int>()
      {
         { Shape.Rock, 1 },
         { Shape.Paper, 2 },
         { Shape.Scissors, 3 },
      };
      private static Dictionary<Outcome, int> OutcomeScores { get; } = new Dictionary<Outcome, int>()
      {
         { Outcome.Lose, 0 },
         { Outcome.Draw, 3 },
         { Outcome.Win, 6 },
      };

      #endregion
      private List<Round> Rounds { get; set; } = null!;

      public override void Part1Setup()
      {
         static Round Convert(string line)
         {
            string[] parts = line.Split(' ');
            Shape elf = Elf[parts[0]];
            Shape player = Player[parts[1]];

            return new Round(elf, player);
         }

         Rounds = ConvertLines(Convert);
      }

      public override object? Part1() => SimulateRounds();

      public override void Part2Setup()
      {
         static Round Convert(string line)
         {
            string[] parts = line.Split(' ');
            Shape elf = Elf[parts[0]];
            Outcome outcome = WantedOutcome[parts[1]];
            Shape player = NeededShape(elf, outcome);

            return new Round(elf, player);
         }

         Rounds = ConvertLines(Convert);
      }
      public override object? Part2() => SimulateRounds();



      private int SimulateRounds()
      {
         int total = 0;
         foreach (Round round in Rounds)
         {
            Outcome outcome = PlayerOutcome(round.Elf, round.Player);

            int shapeScore = ShapeScores[round.Player];
            int outcomeScore = OutcomeScores[outcome];

            total += shapeScore + outcomeScore;
         }

         return total;
      }
      #region Methods
      private static Shape NeededShape(Shape elf, Outcome outcome)
      {
         if (outcome == Outcome.Draw) return elf;

         if (outcome == Outcome.Win)
         {
            return elf switch
            {
               Shape.Rock => Shape.Paper,
               Shape.Paper => Shape.Scissors,
               Shape.Scissors => Shape.Rock,
               _ => throw new Exception()
            };
         }

         return elf switch
         {
            Shape.Rock => Shape.Scissors,
            Shape.Paper => Shape.Rock,
            Shape.Scissors => Shape.Paper,
            _ => throw new Exception()
         };
      }
      private static Outcome PlayerOutcome(Shape elf, Shape player)
      {
         if (elf == player) return Outcome.Draw;

         bool isPlayerWinner =
            (player == Shape.Paper && elf == Shape.Rock) ||
            (player == Shape.Rock && elf == Shape.Scissors) ||
            (player == Shape.Scissors && elf == Shape.Paper);

         return isPlayerWinner ? Outcome.Win : Outcome.Lose;
      }
      #endregion
   }
}