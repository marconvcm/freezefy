using Godot;
using System;

public partial class SceneManager : Node
{
   private static SceneManager _instance;
   public static SceneManager Instance
   {
      get
      {
         if (_instance == null)
         {
            _instance = new SceneManager();
         }
         return _instance;
      }
   }

   public async void ChangeScene(SceneTree tree, FadeOutPlugin fadeOutPlugin, string path)
   { 
      await fadeOutPlugin.FadeOut();
      tree.ChangeSceneToFile(path);
   }

   public void Quit(SceneTree tree)
   {
      tree.Quit();
   }
}
