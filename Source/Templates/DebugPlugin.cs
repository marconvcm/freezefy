using Godot;
using System.Linq;
using System.Threading.Tasks;

[GlobalClass]
public partial class DebugPlugin : Node
{
   public static DebugPlugin Instance { get; private set; }

   [Export]
   public Node3D World;

   [Export]
   public DebugPanel Panel;

   private PackedScene FloorTagScene;

   [Export]
   public bool DebugEnabled = true;

   [Export]
   public bool EnableFloorTags = false;

   [Export]
   public float FloorTagGap = 5.0f;

   public void Start()
   {
      if (DebugEnabled)
      {
         if (Instance == null)
         {
            Instance = this;
         }
         else
         {
            GD.PrintErr("DebugPlugin instance already exists! parent: " + Instance.GetParent());
            return;
         }
      }
   }

   public override void _Ready()
   {
      Start();
   }
}

public static class VDebug
{
   public static void Watch(string key, Variant value)
   {
      if (DebugPlugin.Instance != null)
      {
         DebugPlugin.Instance.Panel.Watch(key, value);
      }
   }
}