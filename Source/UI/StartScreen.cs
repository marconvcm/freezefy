using System;
using System.Collections.Generic;
using Godot;

public partial class StartScreen : Control
{
   private Color activeColor = new Color(1, 1, 1);
   private Color normalColor = new Color(0.5f, 0.5f, 0.5f);

   [Export]
   public Panel CreditPanel;

   [Export]
   public TextureRect Head;

   [Export(PropertyHint.ArrayType)]
   public Label[] Options;

   [Export]
   public FadeOutPlugin FadeOut;

   [Export]
   public int CurrentOption;

   private Dictionary<int, Action> _actions = new Dictionary<int, Action>();

   public override void _Ready()
   {
      _actions.Add(0, () => SceneManager.Instance.ChangeScene(GetTree(), FadeOut, "res://Levels/LVL01.tscn"));
      _actions.Add(1, () => CreditPanel.Visible = !CreditPanel.Visible);
      _actions.Add(2, () => GetTree().Quit());
   }

   public override void _Process(double delta)
   {
      for (int i = 0; i < Options.Length; i++)
      {
         Options[i].Modulate = i == CurrentOption ? activeColor.Lerp(normalColor, 0.2f) : normalColor.Lerp(activeColor, 0.2f);
      }
      Head.GlobalPosition = Options[CurrentOption].GlobalPosition + new Vector2(-Head.Texture.GetWidth() - 16, 0);
   }

   public override void _UnhandledInput(InputEvent @event)
   {
      if (@event is InputEventKey)
      {
         var direction = InputManager.GetDirection();
         if (direction == Vector2.Up && !CreditPanel.Visible)
         {
            CurrentOption = Mathf.Max(0, CurrentOption - 1);
         }
         else if (direction == Vector2.Down && !CreditPanel.Visible)
         {
            CurrentOption = Mathf.Min(Options.Length - 1, CurrentOption + 1);
         }
         else if (InputManager.IsAnyActionPressed())
         {
            _actions[CurrentOption]();
         }
      }
   }
}
