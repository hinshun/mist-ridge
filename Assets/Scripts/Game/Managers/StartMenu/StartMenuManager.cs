using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Zenject;

namespace MistRidge
{
    public class StartMenuManager : IInitializable
    {
        [SerializeField]
        private readonly Settings settings;

        private int selectionIndex;

        public StartMenuManager(Settings settings)
        {
            this.settings = settings;
        }

        public void Initialize()
        {
            ResetMenuItems();

            for (int i = 0; i < settings.menuItems.Length; ++i)
            {
                MenuItem menuItem = settings.menuItems[i];

                if (menuItem.item == settings.startMenuItem)
                {
                    selectionIndex = i;
                    break;
                }
            }

            SelectMenuItem(0);
        }

        public void SelectMenuItem(int indexDiff)
        {
            MenuItem selectedMenuItem = settings.menuItems[selectionIndex];
            selectedMenuItem.textItem.text = selectedMenuItem.textContent;

            selectionIndex = (selectionIndex + indexDiff) % settings.menuItems.Length;
            if (selectionIndex < 0)
            {
                selectionIndex += settings.menuItems.Length;
            }
            selectedMenuItem = settings.menuItems[selectionIndex];
            selectedMenuItem.textItem.text = "> " + selectedMenuItem.textContent;
        }

        private void ResetMenuItems()
        {
            foreach(MenuItem menuItem in settings.menuItems)
            {
                menuItem.textItem.text = menuItem.textContent;
            }
        }

        [Serializable]
        public class Settings
        {
            public StartMenuItem startMenuItem;
            public MenuItem[] menuItems;
        }

        [Serializable]
        public struct MenuItem
        {
            public Text textItem;
            public string textContent;
            public StartMenuItem item;
        }

        public enum StartMenuItem
        {
            NewGame,
            Quit,
        }
    }
}
