using System.Diagnostics;

namespace AoC.Year2022
{
   public class Day7 : Day
   {
      #region Types
      private record Output;

      // commands
      private record Command : Output;
      private record ChangeDirectory(string Directory) : Command
      {
         public bool MoveOut => Directory == "..";
         public bool ToRoot => Directory == "/";
      }
      private record ListCommand : Command;

      // data
      private record Data : Output;
      private record FileData(string Name, ulong Size) : Data;
      private record DirectoryData(string Name) : Data;
      private Dictionary<string, EntryData> _system = null!;
      private record EntryData(bool IsDirectory, ulong Size);
      #endregion

      #region Fields
      private List<Output> _outputs = null!;
      #endregion

      #region Part 1
      public override void Part1Setup() => ReadOutput();
      public override object? Part1()
      {
         BuildSystem();
         UpdateDirectorySizes();

         ulong total = Sum(_system
            .Where(p => p.Value.IsDirectory)
            .Where(p => p.Value.Size <= 100_000)
            .Select(p => p.Value.Size));
         return total;
      }
      public override void Part2Setup() => ReadOutput();
      public override object? Part2()
      {
         BuildSystem();
         UpdateDirectorySizes();

         const ulong total = 70_000_000;
         const ulong needed = 30_000_000;

         ulong current = _system[Path.DirectorySeparatorChar.ToString()].Size;
         ulong free = total - current;
         ulong toFree = needed - free;

         ulong size = _system
            .Where(p => p.Value.IsDirectory)
            .Where(p => p.Value.Size >= toFree)
            .OrderBy(p => p.Value.Size)
            .Select(p => p.Value.Size)
            .First();

         return size;
      }
      #endregion

      #region Methods
      private static ulong Sum(IEnumerable<ulong> values)
      {
         ulong total = 0;
         foreach (ulong val in values)
            total += val;

         return total;
      }
      private void UpdateDirectorySizes()
      {
         List<string> paths = _system
            .Where(p => p.Value.IsDirectory)
            .Select(p => p.Key)
            .OrderByDescending(k => k.Length)
            .ToList();

         foreach (string path in paths)
         {
            string terminatedPath = Path.EndsInDirectorySeparator(path) ? path : path + Path.DirectorySeparatorChar;

            List<string> allSubFiles = _system
               .Where(p => !p.Value.IsDirectory)
               .Where(p => p.Key.StartsWith(terminatedPath))
               .Select(p => p.Key)
               .ToList();

            ulong total = 0;
            foreach (string subPath in allSubFiles)
               total += _system[subPath].Size;

            _system[path] = new EntryData(_system[path].IsDirectory, total);
         }
      }
      private void BuildSystem()
      {
         _system = new Dictionary<string, EntryData>()
         {
            { Path.DirectorySeparatorChar.ToString(), new EntryData(true, 0) }
         };

         string current = null!; // should be changed by first cd
         foreach (Output output in _outputs)
         {
            if (output is ChangeDirectory cd)
               ProcessCommand(cd, ref current);
            else if (output is ListCommand) // safe to ignore
               continue;
            else if (output is DirectoryData directoryData)
               ProcessCommand(directoryData, current);
            else if (output is FileData fileData)
               ProcessCommand(fileData, current);
         }
      }
      private void ProcessCommand(DirectoryData data, string current)
      {
         string path = Path.Combine(current, data.Name);
         Debug.Assert(_system.ContainsKey(path) == false, $"The directory '{path}' already exists.");

         _system.Add(path, new EntryData(true, 0));
      }
      private void ProcessCommand(FileData data, string current)
      {
         string path = Path.Combine(current, data.Name);
         Debug.Assert(_system.ContainsKey(path) == false, $"The file '{path}' already exists.");

         _system.Add(path, new EntryData(false, data.Size));
      }
      private void ProcessCommand(ChangeDirectory cd, ref string current)
      {
         if (cd.ToRoot) current = Path.DirectorySeparatorChar.ToString();
         else if (cd.MoveOut) current = Path.GetDirectoryName(current) ?? throw new Exception("MoveOut was called when on root.");
         else
         {
            Debug.Assert(cd.Directory.Contains(Path.DirectorySeparatorChar) == false, "CD should not have multiple segments.");
            current = Path.Combine(current, cd.Directory);
         }
      }
      private void ReadOutput()
      {
         static Output Convert(string line)
         {
            if (line.StartsWith("$ "))
               return ConvertCommand(line[2..]);

            return ConvertData(line);
         }

         _outputs = ConvertLines(Convert);
      }
      private static Command ConvertCommand(string command)
      {
         if (command == "ls")
            return new ListCommand(); // could've just completely ignores this but effort.

         Debug.Assert(command.StartsWith("cd"));

         command = command[3..];
         return new ChangeDirectory(command);
      }
      private static Data ConvertData(string data)
      {
         if (data.StartsWith("dir"))
            return new DirectoryData(data[4..]);

         int sp = data.IndexOf(' ');
         string numStr = data[..sp];
         string name = data[(sp + 1)..];

         return new FileData(name, ulong.Parse(numStr));
      }
      #endregion
   }
}