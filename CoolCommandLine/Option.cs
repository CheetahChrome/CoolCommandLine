using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CoolCommandLine
{
    /// <summary>
    /// When a user has a specific command line operation, this object holds that information.
    /// </summary>
    public class Option
    {

        #region Variables

        #endregion

        #region Properties

        public bool IgnoreCase { get; set; }

        public bool IsOptional { get; set; }


        /// <summary>
        /// Is this option a viable one to another option? Used for description.
        /// </summary>
        public List<string> AssociatedWithOptions { get; set; }


        /// <summary>
        /// Has this option been setup to have a single letter as an option?
        /// </summary>
        public bool IsSingleLetterOption { get; set; }

        /// <summary>
        /// Holds the letter option to be indexed and accessed by the command line processor. Such a "-L" is "L"
        /// </summary>
        public string Letter { get; set; }


        /// <summary>
        /// If the initial string is comma seperated then break it out into the individual tokens we need to find.
        /// </summary>
        public List<string> Letters { get; set; }


        private List<char> LettersSingle { get; set; }


        private List<string> LettersMultiple { get; set; }

        /// <summary>
        /// What the consumer has set us up with.
        /// </summary>
        public string InitialLetterDefinition { get; set; }


        public bool ArgumentMatched { get; set; }


        public string LongName { get; set; }

        /// <summary>
        /// Holds the descriptive text to be shown when there is no output.
        /// </summary>
        public string Description { get; set; }


        public Action<CommandLineManager> Operation { get; private set; }

        /// <summary>
        /// The validation step is done when an option is found, but before an execute. If the return value is 'false' the execute will be canceled.
        /// </summary>
        public Func<CommandLineManager, bool> Validation { get; set; }


        /// <summary>
        /// The validation step is done when an option is found, but before an execute. If the return value is 'false' the execute will be canceled.
        /// This operation returns this object in the function as well.
        /// </summary>
        public Func<CommandLineManager, Option, bool> ValidationExtra { get; set; }

        /// <summary>
        /// This holds the associated data.
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Is this option have a need for associated data?
        /// </summary>
        public bool HasAssociatedData => string.IsNullOrEmpty(Data);


        /// <summary>
        /// Is this option have a need for associated data?
        /// </summary>
        public bool NeedsAssociatedData { get; set; }


        #endregion

        #region Construction/Initialization

        public Option()
        {
            
        }

 

        public Option(string lettersDefinition, string description, Action<CommandLineManager> operation, bool isOptional = true, bool ignoreCase = true)
        {
            InitialLetterDefinition = lettersDefinition;

            Letters = Regex.Matches(lettersDefinition, @"[^,\t\s]+")
                           .OfType<Match>()
                           .Select(mt => mt.Value)
                           .ToList();

            Letter = Letters.FirstOrDefault(token => token.Length == 1)?.ToUpper();

            IsSingleLetterOption = Letter != null;

            //if (IsSingleLetterOption)
            //    Letter = Letters.FirstOrDefault(token => token.Length == 1)?.ToUpper() ?? Letters[0].Substring(0, 1).ToUpper();

            var grouped = Letters.GroupBy(lt => lt.Length == 1)
                                 .ToList();

            LettersSingle = grouped.Where(gr => gr.Key)
                                   .SelectMany(grp => grp.Select(letter => letter [0])
                                                         .ToList())
                                   .ToList();

            LettersMultiple = grouped.Where(gr => gr.Key == false)
                                     .SelectMany(grp => grp)
                                     .ToList();

            Description = description;
            Operation   = operation;
            IsOptional  = isOptional;
        }


        /// <summary>
        /// Check a set of arguments to determine if there is a match.
        /// </summary>
        /// <param name="args">The tokenized argument list.</param>
        /// <returns></returns>
        public bool ArgumentFoundCheck(List<Argument> args )
        {

            var split = args.Where(arg => arg.IsArgument)
                            .GroupBy(arg => arg.TextWithoutDash.Length == 1)
                            .ToList();

            var argumentMatched = (split.FirstOrDefault(groups => groups.Key)?.FirstOrDefault(arg => LettersSingle.Contains(arg.TextWithoutDash[0])))         // Check single char list
                                ?? split.FirstOrDefault(groups => groups.Key == false)?.FirstOrDefault(arg => LettersMultiple.Contains(arg.TextWithoutDash)); // Check multiple char list


            ArgumentMatched = argumentMatched != null;

            if (ArgumentMatched && NeedsAssociatedData)
            {
               var data = args.SkipWhile(arg => arg.Index != argumentMatched?.Index + 1 ).FirstOrDefault();

                if ((data != null) && (data.IsData))
                    Data = data.Text;
            }

            return ArgumentMatched;
        }


        #endregion

        #region Methods

        /// <summary>
        /// Provide a formatted output of this description.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"-{Letter}" + (NeedsAssociatedData ? " { XXX } " : string.Empty) + $"\t{Description}";
        }

        #endregion



    }
}
