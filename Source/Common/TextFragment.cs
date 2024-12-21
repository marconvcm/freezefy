using Godot;
using System;

[GlobalClass]
public partial class TextFragment : Resource
{
   public struct Line
   {
      public string Speaker;
      public string Text;
   }

   [Export(PropertyHint.MultilineText)]
   public string Text { get; set; }

   [Export(PropertyHint.EnumSuggestion, "Forgotten")]
   public string Speaker { get; set; }

   [Export]
   public bool ShowAllLines { get; set; } = true;

   [Export]
   public float TypingSpeed { get; set; } = 1f;

   [Export]
   public bool PauseGame { get; set; } = true;

   public Line? GetNextLine()
   {
      try
      {
         var line = Text.Split('\n')[currentLine++];
         return new Line()
         {
            Speaker = GetLineSpeaker(line) ?? Speaker,
            Text = GetLineText(line)
         };
      }
      catch (IndexOutOfRangeException)
      {
         return null;
      }
   }

   [Export]
   public TextFragment Next { get; set; }

   public bool HasNextLine => ShowAllLines ? false : currentLine < Text.Split('\n').Length;
   private int currentLine = 0;

   private string GetLineSpeaker(string line)
   {
      var parts = line.Split(':');
      if (parts.Length > 1)
      {
         return parts[0].Trim();
      }
      return null;
   }

   private string GetLineText(string line)
   {
      if (line.Split(':').Length < 2)
      {
         return line.Trim();
      }
      return line.Split(':')[1].Trim();
   }
}
