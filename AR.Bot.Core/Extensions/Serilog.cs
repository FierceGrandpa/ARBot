using Serilog;
using Serilog.Filters;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{
    public static class SerilogExtensions
    {
        public static LoggerConfiguration Ignore(this LoggerConfiguration config, string[] sources)
        {
            foreach (var source in sources)
            {
                config.Filter.ByExcluding(Matching.FromSource(source));
            }

            return config;
        }
    }
}