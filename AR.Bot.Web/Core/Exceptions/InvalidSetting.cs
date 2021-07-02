using System;
using AR.Bot.Core.Services;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{

    public class InvalidSettingException : Exception
    {
        public InvalidSettingException(Setting setting) : base(setting.ToString())
        {
            
        }
    }
}