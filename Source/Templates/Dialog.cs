using Godot;
using System;

public partial class Dialog : Control
{
   #region Singleton
   private static Dialog _instance;

   public static Dialog Instance
   {
      get
      {
         return _instance;
      }

      set
      {
         if (_instance == null)
         {
            _instance = value;
         }
         else
         {
            GD.Print("There is already an instance of Dialog");
         }
      }
   }

   #endregion

   [Export]
   public Node2D gameNode;

   public Label dialogText;

   public Label dialogBanner;

   public PanelContainer panelBanner;

   public PanelContainer panelText;

   public TextureRect phoebeFace;

   public TextureRect catFace;

   public TextureRect manFace;

   public Label spaceHint;

   private Timer timer;

   private bool isInputAllowed = false;

   private TextFragment currentFragment;


   public override void _Ready()
   {
      timer = GetNode<Timer>("Timer");
      timer.Timeout += () => isInputAllowed = true;

      dialogText = GetNode<Label>("PanelText/DialogText");
      dialogBanner = GetNode<Label>("PanelBanner/DialogBanner");
      panelBanner = GetNode<PanelContainer>("PanelBanner");
      panelText = GetNode<PanelContainer>("PanelText");
      phoebeFace = GetNode<TextureRect>("PhoebeFace");
      catFace = GetNode<TextureRect>("CatFace");
      manFace = GetNode<TextureRect>("ManFace");
      spaceHint = GetNode<Label>("Space Hint");
      Instance = this;
      hide();

   }

   public void Display(string bannerText = null, string message = "", string face = null, bool pauseGame = true)
   {
      isInputAllowed = false;
      dialogBanner.Text = bannerText;
      dialogText.Text = message;
      dialogText.VisibleRatio = 0.0f;
      Visible = true;
      panelBanner.Visible = !string.IsNullOrEmpty(bannerText);
      panelText.Visible = !string.IsNullOrEmpty(message);

      if (face != null)
      {
         phoebeFace.Visible = face.Contains("forgotten", StringComparison.InvariantCultureIgnoreCase);
         catFace.Visible = face.Contains("cat", StringComparison.InvariantCultureIgnoreCase);
         manFace.Visible = face.Contains("man", StringComparison.InvariantCultureIgnoreCase);
      }
      else
      {
         phoebeFace.Visible = false;
         catFace.Visible = false;
         manFace.Visible = false;
      }

      if(pauseGame) 
      {
         gameNode.ProcessMode = ProcessModeEnum.Disabled;
      }
      
      timer.Start();
   }

   public void Show(TextFragment fragment, bool allLines = true)
   {
      currentFragment = fragment;

      if (fragment.ShowAllLines)
      {
         Display(fragment.Speaker, fragment.Text, fragment.Speaker, fragment.PauseGame);
      }
      else
      {
         DisplayNextLine();
      }
   }

   public void DisplayNextLine()
   {
      var line = currentFragment.GetNextLine();
      if (line.HasValue)
      {
         Display(line.Value.Speaker, line.Value.Text, line.Value.Speaker);
      }
   }

   public override void _UnhandledInput(InputEvent @event)
   {
      if (isInputAllowed && @event is InputEventKey keyEvent)
      {
         if (keyEvent.IsActionPressed("ui_select"))
         {
            if (currentFragment != null && currentFragment.HasNextLine)
            {
               DisplayNextLine();
            }
            else
            {
               hide();
               gameNode.ProcessMode = ProcessModeEnum.Always;
            }
         }
      }
   }

   public override void _Process(double delta)
   {
      if (currentFragment != null && !currentFragment.ShowAllLines)
      {
         dialogText.VisibleRatio = Mathf.Clamp(dialogText.VisibleRatio + (float)delta * currentFragment.TypingSpeed, 0, 1);
      }
   }

   public void hide()
   {
      this.Visible = false;
      this.dialogBanner.Text = "";
      this.dialogText.Text = "";
      this.manFace.Visible = false;
      this.catFace.Visible = false;
      this.phoebeFace.Visible = false;
   }
}
