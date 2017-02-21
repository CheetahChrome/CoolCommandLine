﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CoolCommandLine
{
    public class CommandLineManager
    {

        public bool A { get; set; }
        public bool B { get; set; }
        public bool C { get; set; }
        public bool D { get; set; }
        public bool E { get; set; }
        public bool F { get; set; }
        public bool G { get; set; }
        public bool H { get; set; }
        public bool I { get; set; }
        public bool J { get; set; }
        public bool K { get; set; }
        public bool L { get; set; }
        public bool M { get; set; }
        public bool N { get; set; }
        public bool O { get; set; }
        public bool P { get; set; }
        public bool Q { get; set; }
        public bool R { get; set; }
        public bool S { get; set; }
        public bool T { get; set; }
        public bool U { get; set; }
        public bool V { get; set; }
        public bool W { get; set; }
        public bool X { get; set; }
        public bool Y { get; set; }
        public bool Z { get; set; }

        /// <summary>
        /// If a dash is not required, the an option can be designated without the dash such as `-L` can also be `L`. Hence it is "FreeForm".
        /// </summary>
        public bool IsFreeFormAllowed { get; set; }

        /// <summary>
        /// Used by the execute to determine whether the arguments need to be parsed.
        /// </summary>
        public bool HaveArgumentsBeenParsed { get; set; }


        public List<Option> Options { get; set; } = new List<Option>();

        private Option LastOption { get; set; }

        public CommandLineManager Parse(string[] args)
        {
            if (args != null)
            {
                var argsToTest = args.Where( arg => IsFreeFormAllowed || arg[0] == '-')
                                     .Select(arg => arg[0] == '-' ? arg.Substring(1, arg.Length -1) : arg)
                                     .ToList();

                var props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .ToList();

                Options.Where(opt => opt.ArgumentFoundCheck(argsToTest))
                       .ToList()
                       .ForEach(opt => props.FirstOrDefault(prp => prp.Name == opt.Letter)?.SetValue(this, true, null)); // TODO Ignore Case scenario?);

            }

            HaveArgumentsBeenParsed = true;

            return this;
        }

        public void ExecuteOperation(string[] args = null)
        {
            if (HaveArgumentsBeenParsed == false)
                Parse(args);

            Options.Where(opt => opt.ArgumentMatched)
                   .ToList()
                   .ForEach(option => option?.Operation?.Invoke(this));

        }


        /// <summary>
        /// Add an option such as `-L` to check for.
        /// </summary>
        /// <param name="letter">Target letter or letters </param>
        /// <param name="description"></param>
        /// <param name="operation"></param>
        /// <param name="isOptional"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public CommandLineManager AddOption(string letter,  string description, Action<CommandLineManager> operation = null, bool isOptional = true, bool ignoreCase = true)
        {
            LastOption = new Option(letter, description, operation, isOptional, ignoreCase);
            Options.Add(LastOption);
            return this;
        }

        /// <summary>
        /// When an option needs secondary data to be valid, this is the one to specify it.
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="description"></param>
        /// <param name="operation"></param>
        /// <param name="isOptional"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public CommandLineManager AddOptionRequiresData(string letter, string description, Action<CommandLineManager> operation = null, bool isOptional = true, bool ignoreCase = true)
        {
            LastOption = new Option(letter, description, operation, isOptional, ignoreCase);
            Options.Add(LastOption);
            return this;
        }


        /// <summary>
        /// Is the dash required? When it is *not* call AllowFreeForm().
        /// </summary>
        /// <returns>Sets <see cref="IsFreeFormAllowed"/> to true.</returns>
        public CommandLineManager AllowFreeForm()
        {
            IsFreeFormAllowed = true;
            return this;
        }


    }
}