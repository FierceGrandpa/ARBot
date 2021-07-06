using System;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{
    public class InvalidSettingValueException : Exception
    {
        public InvalidSettingValueException(string value) : base(value)
        {
            
        }
    }
}