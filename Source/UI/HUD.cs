using System;
using Godot;

public partial class HUD : Control
{
   [Export] public ProgressBar HealthBar;

   [Export] public ProgressBar SkillBar;

   [Export] public StatPlugin HealthPoints;

   [Export] public StatPlugin SkillPoints;

   public override void _Ready()
   {
      HealthPoints.ValueChanged += UpdateHealthBar;
      SkillPoints.ValueChanged += UpdateSkillBar;

      HealthBar.MaxValue = HealthPoints.MaxValue;
      SkillBar.MaxValue = SkillPoints.MaxValue;
      HealthBar.MinValue = HealthPoints.MinValue;
      SkillBar.MinValue = SkillPoints.MinValue;
      HealthBar.Value = HealthPoints.CurrentValue;
      SkillBar.Value = SkillPoints.CurrentValue;
   }

   private void UpdateSkillBar(float value)
   {
      SkillBar.Value = value;
   }

   private void UpdateHealthBar(float value)
   {
      HealthBar.Value = value;
   }
}
