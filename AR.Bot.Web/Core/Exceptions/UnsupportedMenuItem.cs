using System;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{
    public class UnsupportedMenuItem : Exception
    {
        public UnsupportedMenuItem(string name) : base($"Меню {name} не поддерживается.") { }
    }
}