using Godot;
using Godot.Collections;

public partial class DebugPanel : Control
{
   Dictionary<string, PanelContainer> watchListPanel = new Dictionary<string, PanelContainer>();

   PanelContainer PanelTemplate => GetNode<PanelContainer>("VBox/Panel");

   VBoxContainer VBox => GetNode<VBoxContainer>("VBox");

   public void Watch(string key, Variant value)
   {
      AddWatchPanel(key, value);
   }

   private void AddWatchPanel(string key, Variant value)
   {
      PanelContainer panel;

      if (!watchListPanel.ContainsKey(key))
      {
         panel = PanelTemplate.Duplicate() as PanelContainer;
         panel.Visible = true;
         watchListPanel[key] = panel;
         VBox.AddChild(panel);
      }
      else
      {
         panel = watchListPanel[key];
      }

      panel.GetNode<Label>("Label").Text = key;
      panel.GetNode<Label>("Value").Text = value.ToString();
   }

   public void Unwatch(string key)
   {
      if (watchListPanel.ContainsKey(key))
      {
         watchListPanel[key].QueueFree();
         watchListPanel.Remove(key);
      }
   }

   public override void _Process(double delta)
   {

   }
}
