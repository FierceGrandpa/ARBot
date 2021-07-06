using System;
using System.Security.Cryptography;

namespace AR.Bot.Web.Core.Utils
{
    public class SecureRandomGenerator
    {
        private readonly RNGCryptoServiceProvider _csp;

        public SecureRandomGenerator()
        {
            _csp = new RNGCryptoServiceProvider();
        }
        
        public int Next(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue)
                throw new ArgumentOutOfRangeException(
                    "minValue must be lower than maxExclusiveValue");

            var diff = (long) maxExclusiveValue - minValue;
            var upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            } while (ui >= upperBound);

            return (int) (minValue + (ui % diff));
        }
        
        public int Next(int maxExclusiveValue)
        {
            return Next(0, maxExclusiveValue);
        }

        private uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));
            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            var buffer = new byte[bytesNumber];
            _csp.GetBytes(buffer);
            return buffer;
        }
        
    }
}