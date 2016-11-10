using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ServiceRunner.Args
{
    internal class ArgumentParser
    {
        public static Dictionary<string, Option> Parse(string[] args)
        {
            var options = AppOptionsFactory.GetAppOptions();
            foreach (var argument in args)
            {
                foreach (var option in options)
                {
                    var regex = new Regex($"-{option.Name}:(('.+')|([^ -]+))");
                    var match = regex.Match(argument);
                    if (!match.Success) continue;

                    var value = match.Groups[1];
                    option.Value = value.Value;
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
                if (option.IsRequired && String.IsNullOrWhiteSpace(option.Value)) return false;
            }
            return true;
        }
    }
}