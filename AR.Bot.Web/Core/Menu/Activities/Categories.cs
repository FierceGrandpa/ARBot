using System;
using System.Collections.Generic;
using System.Linq;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class SelectCategoryMenu : MenuItem
    {
        private readonly IReadOnlyList<Category> _categories;

        private const byte CategoriesOnPage = 3;
        private const byte Columns = 2;

        private readonly int _currentPage;
        private readonly int _pagesNeeded;

        public SelectCategoryMenu(IReadOnlyList<string> arguments, IRepository<Category> categoryRepository)
        {
            ItemTitle   = "Выбрать категорию";
            Description = "*Вы можете выбрать категорию по которой вам будет выдана активность*";

            Command = nameof(SelectCategoryMenu).ToLowerInvariant();

            if (categoryRepository == null)
            {
                _categories = null;
                _currentPage = 0;
                _pagesNeeded = 0;
                return;
            }

            _categories = categoryRepository.GetAll().Where(e => e.Status).ToList().AsReadOnly();

            // TODO: good
            _currentPage = (arguments?.Count).GetValueOrDefault(0) > 0
                ? byte.Parse(arguments[0])
                : _currentPage = 1;

            _pagesNeeded = (int)Math.Ceiling(_categories.Count / (double)CategoriesOnPage);
        }

        protected override void GenerateButtons()
        {
            // TODO: Good
            var buffer = new List<InlineKeyboardButton>(Columns);

            var previousPage = _currentPage - 1;
            var displayedCategories = previousPage * CategoriesOnPage;
            var categoriesLeft = _categories.Count - displayedCategories;

            var until = _currentPage == _pagesNeeded // TODO: Is this the last page?
                ? displayedCategories + categoriesLeft
                : displayedCategories + CategoriesOnPage;

            for (var i = displayedCategories; i < until; i++)
            {
                if (buffer.Count == Columns)
                {
                    Buttons.Add(buffer.ToList());
                    buffer.Clear();
                }

                var category = _categories[i];
                buffer.Add(new InlineKeyboardButton
                {
                    Text = category.Title,
                    CallbackData = $"switch {typeof(SelectMenu)}#categoryId={category.Id}"
                });
            }

            if (buffer.Count != 0)
            {
                Buttons.Add(buffer.ToList());
                buffer.Clear();
            }

            AddNavigateButtons();

            base.GenerateButtons();
        }

        private void AddNavigateButtons()
        {
            var controlButtons = new List<InlineKeyboardButton>();

            if (_currentPage > 1)
                controlButtons.Add(new InlineKeyboardButton
                    { Text = "⬅", CallbackData = $"switch {typeof(SelectCategoryMenu)}#{_currentPage - 1}" });

            controlButtons.Add(new InlineKeyboardButton { Text = "❌ Back", CallbackData = $"switch {typeof(MainMenu)}" });

            if (_currentPage < _pagesNeeded)
                controlButtons.Add(new InlineKeyboardButton
                    { Text = "➡", CallbackData = $"switch {typeof(SelectCategoryMenu)}#{_currentPage + 1}" });

            Buttons.Add(controlButtons);
        }
    }
}