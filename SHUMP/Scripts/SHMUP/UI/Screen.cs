using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.UI {

	public class Screen : Control
	{
        protected LocalisationManager localizationManager = null;

        protected Dictionary<NodePath, string> localTranslationKeys = new Dictionary<NodePath, string>();

        public override void _Ready()
        {
            localizationManager = LocalisationManager.GetInstance();

            Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
            localizationManager.Connect(nameof(LocalisationManager.LanguageChanged), this, nameof(UpdateAllText));
        }

        public virtual void OpenScreen()
        {
			Visible = true;
        }

		public virtual void CloseScreen()
        {
			Visible = false;
        }

        protected void SwitchPanel(Screen pOpeningPanel, Screen pClosingPanel)
        {
            pOpeningPanel.OpenScreen();
            pClosingPanel.CloseScreen();
        }

        protected virtual void UpdateAllText() { }

        protected virtual void Destructor()
        {
            localizationManager.Disconnect(nameof(LocalisationManager.LanguageChanged), this, nameof(UpdateAllText));
            Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
        }

    }
}