using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoolCommandLine
{
    /// <summary>
    /// Provides the ability to tokenize and classify the inputed argument(s).
    /// </summary>
    public class Argument
    {
        public int Index { get; set; }

        public string Text { get; set; }

        /// <summary>
        /// If it is an argument, extract the text without the dash, otherwise acccept the whole text.
        /// </summary>
        public string TextWithoutDash => IsArgument ? Text.Substring(1, Text.Length - 1) : Text;

        public bool IsArgument => Text?[0] == '-';

        public bool IsData => !IsArgument;

        /// <summary>
        /// Are there more than one character (without the dash). Does not check if it is an arugment or not.
        /// </summary>
        public bool IsMultipleCharacters => TextWithoutDash?.Any() ?? false;

    }
}
