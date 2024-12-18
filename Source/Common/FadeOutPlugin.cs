using System.Threading.Tasks;
using Godot;

[GlobalClass]
public partial class FadeOutPlugin : Node
{
   private readonly Color Fade = new Color(0, 0, 0, 0);
   private readonly Color Black = new Color(0, 0, 0, 1);

   [Export]
   public float Duration = 1.0f;

   [Export]
   public TextureRect Overlay;

   public delegate void FadeOutCallback();

   public override void _Ready()
   {
      Overlay.Modulate = Fade;
      Overlay.Visible = true;
   }
   public async Task FadeOut(float? duration = null)
   {
      var tween = GetTree().CreateTween();
      tween.TweenProperty(Overlay, "modulate", Black, duration ?? Duration);
      tween.SetEase(Tween.EaseType.InOut);
      await ToSignal(tween, Tween.SignalName.Finished);
   }
}