using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class MainMenu : MenuItem
    {
        private readonly List<Type> _mainMenuItems;

        public MainMenu(IReadOnlyList<string> arguments)
        {
            Description = "Вы можете настроить время рассылки или получить активность";
            ItemTitle = "Главное Меню";

            _mainMenuItems = new List<Type>
            {
                typeof(SelectCategoryMenu),
                typeof(SendingTimeModeMenu),
                typeof(GetActivityMenu)
            };
        }

        protected override void GenerateButtons() => GenerateButtons(_mainMenuItems);
    }
}