using Godot;
using System;

public partial class Init : Node
{
   [Export] TextFragment initialDialog;

   public override void _Ready()
   {
      Dialog.Instance.Show(initialDialog);
   }
}
