using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoolCommandLine
{
    /// <summary>
    /// Provides the ability to tokenize the inputed arguments.
    /// </summary>
    public class Argument
    {
        public int Index { get; set; }
        public string Text { get; set; }

        public string TextWithoutDash => Text.Substring(1, Text.Length - 1);

        public bool IsArgument => Text[0] == '-';

        public bool IsData => !IsArgument;

    }
}
