using System;
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
        /// Holds the letter(s) option. Can be multiple letters.
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
        public string Description { get; set; }
        public Action<CommandLineManager> Operation { get; private set; }


        #endregion

        #region Construction/Initialization

        public Option()
        {
            
        }

        public Option(string lettersDefinition,  string description, Action<CommandLineManager> operation, bool isOptional = true, bool ignoreCase = true)
        {
            InitialLetterDefinition = lettersDefinition;

            Letters = Regex.Matches(lettersDefinition, @"[^,\t\s]+")
                           .OfType<Match>()
                           .Select(mt => mt.Value)
                           .ToList();

            // If no Letter is found....extract the first letter from the first token to use at that letter.
            Letter = Letters.FirstOrDefault(token => token.Length == 1)?.ToUpper() ?? Letters[0].Substring(0, 1).ToUpper();

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
        /// <param name="arguments"></param>
        /// <returns></returns>
        public bool ArgumentFoundCheck(List<string> arguments)
        {

            var split = arguments.GroupBy(arg => arg.Length == 1)
                                 .ToList();

            return 
            ArgumentMatched = (split.FirstOrDefault(groups => groups.Key)?.Any(arg => LettersSingle.Contains(arg[0])) ?? false) ||
                              (split.FirstOrDefault(groups => groups.Key == false)?.Any(arg => LettersMultiple.Contains(arg)) ?? false);

        }


        #endregion

        #region Methods

        #endregion



    }
}
