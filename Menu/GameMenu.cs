using MonGame.Drawing;
using MonGame.ECS;
using MonGame.Input;
using MonGame.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Menu
{
    [FirstProcessor]
    [AfterProcessor(typeof(Input.Input))]
    [AfterProcessor(typeof(UIInput))]
    public class GameMenu : UpdateProcessor
    {
        public bool Paused;

        public UITransform MenuParent;

        public override Dictionary<Type, Func<Event, EventAction>> Events { get; } = [];

        public override void Initialize(Ecs ecs, GameManager game)
        {
            Gui gui = game.Gui;
            // need to set menu parent
            UITransform guiTransform = gui.Entity.GetComponent<UITransform>();

            Entity menuParentEntity = ecs.CreateEntity("Menu Parent");
            MenuParent = new UITransform(menuParentEntity, new(0, 0), 0.95f, guiTransform, Active: false);
            new UIFrame(menuParentEntity, 1000, 1000, 1000, 1000);

            // set background of menu
            Entity menuBackground = ecs.CreateEntity("Menu Background");
            new UITransform(menuBackground, new(0, 0), 0f, MenuParent);
            new UIFrame(menuBackground, 1000, 1000);
            var menuBackgroundTexture = Asset.MenuGreyBackground.Load();
            new UITexture(menuBackground, menuBackgroundTexture);
            new MouseBlock(menuBackground);

            // set unpause buttom of menu
            Entity continueButton = ecs.CreateEntity("Continue Button");
            new UITransform(continueButton, new(250, 250), 0.1f, MenuParent);
            new UIFrame(continueButton, 500, 100);
            var continueButtonTexture = Asset.ContinueButton.Load();
            new UITexture(continueButton, continueButtonTexture);
            new MouseBlock(continueButton);
            new MouseBind(continueButton, [(UIMouseEvent.LeftMouseClick, new UnpauseEvent(null))]);
            new UIColor(continueButton);
            new MouseOverSensitivity(continueButton);
            new DarkenOnHover(continueButton, ColorF.White, new(0.75f, 0.75f, 0.75f, 1f), 0.2f, 0.25f);

            // set quit buttom of menu
            Entity quitButton = ecs.CreateEntity("Quit Button");
            new UITransform(quitButton, new(250, 750), 0.1f, MenuParent);
            new UIFrame(quitButton, 500, 100);
            var quitButtonTexture = Asset.QuitButton.Load();
            new UITexture(quitButton, quitButtonTexture);
            new MouseBlock(quitButton);
            new MouseBind(quitButton, [(UIMouseEvent.LeftMouseClick, new QuitEvent(null))]);
            new UIColor(quitButton);
            new MouseOverSensitivity(quitButton);
            new DarkenOnHover(quitButton, ColorF.White, new(0.75f, 0.75f, 0.75f, 1f), 0.2f, 0.25f);

            // Add in pause event
            Events.Add(typeof(PauseEvent), e => { Pause(); return EventAction.Continue; } );
            Events.Add(typeof(UnpauseEvent), e => { Unpause(); return EventAction.Continue; } );
        }

        public void Pause()
        {
            if (Paused)
                return;

            Paused = true;
            MenuParent.Active = true;
        }

        public void Unpause()
        {
            if (!Paused)
                return;
            Paused = false;
            MenuParent.Active = false;
        }
    }
}
