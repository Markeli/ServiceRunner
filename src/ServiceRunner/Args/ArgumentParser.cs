using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ServiceRunner.Args
{
    internal class ArgumentParser
    {
        
        public static Dictionary<string, Option> Parse(string[] args)
        {
            var options = AppOptionsFactory.GetAppOptions();
            for (var i =0; i < args.Length; ++i)
            {
                foreach (var option in options)
                {
                    if (!String.Equals(args[i], $"-{option.Name}")) continue;

                    if (!option.IsFlag)
                    {
                        option.Value = args[++i];
                    }
                    option.IsSetted = true;
                }
            }

            var result = new Dictionary<string, Option>(options.Count);
            foreach (var option in options)
            {
                result.Add(option.Name, option);
            }
            return result;
        }

        public static bool IsValid(List<Option> options)
        {
            foreach (var option in options)
            {
                if (option.IsRequired && !option.IsSetted) return false;

                if (!option.IsFlag && String.IsNullOrWhiteSpace(option.Value)) return false;
            }
            return true;
        }
    }
}