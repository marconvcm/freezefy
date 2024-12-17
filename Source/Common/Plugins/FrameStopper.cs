using Godot;

[GlobalClass]
public partial class FrameStopper : Node
{
   public async void FrameStop(float duration = 0.3f)
   {
      // Slow down the game to a halt
      Engine.TimeScale = 0.0f;

      // Wait for the duration using a timer
      await ToSignal(GetTree().CreateTimer(duration, true, false, true), "timeout");

      // Restore normal game speed
      Engine.TimeScale = 1.0f;
   }
}

