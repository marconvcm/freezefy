using Godot;
using System;

public partial class TheEnd : Area2D
{
   [Export] public TextFragment noHaveKeyDialog;

   public override void _Ready()
   {
      BodyEntered += (body) =>
      {
         if (body is Player player)
         {
            if (player.HasKey())
            {
               player.sprite.Hide();
               SceneManager.Instance.ChangeScene(GetTree(), GetNode<FadeOutPlugin>("FadeOutPlugin"), "res://UI/TheEnd.tscn");
            }
            else
            {
               Dialog.Instance.Show(noHaveKeyDialog);
            }
         }
      };
   }
}
