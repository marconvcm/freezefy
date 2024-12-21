using Godot;
using System;
using System.Threading.Tasks;

public partial class GameOver : Control
{
   FadeOutPlugin fadeOutPlugin;

   public override void _Ready()
   {
      fadeOutPlugin = GetNode<FadeOutPlugin>("FadeOutPlugin");
      Play();
   }

   public async void Play()
   {
      await Delay(3);
      SceneManager.Instance.ChangeScene(GetTree(), fadeOutPlugin, "res://UI/StartScreen.tscn");
   }

   public async Task Delay(float seconds)
   {
      var timer = new Timer();
      timer.WaitTime = seconds;
      timer.OneShot = true;
      AddChild(timer);
      timer.Start();
      await ToSignal(timer, "timeout");
      RemoveChild(timer);
      timer.QueueFree();
      timer = null;
   }
}
