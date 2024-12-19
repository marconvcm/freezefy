using Godot;

[GlobalClass]
public partial class PickableResource : Resource
{
   [Export]
   public string Label;

   [Export]
   public string Description;

   [Export]
   public Texture2D Icon;
}
