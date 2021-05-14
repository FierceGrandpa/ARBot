using System;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{
    public class UnsupportedCommand : Exception
    {
        public UnsupportedCommand(string request) : base($"Команда {request} не поддерживается.") {}
    }
}