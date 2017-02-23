using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CoolCommandLine
{
    public class CommandLineManager
    {
        #region Variables
        private Dictionary<string, string> _optionDataDictionary = new Dictionary<string, string>();
        #endregion

        #region Consumer Properties
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
        /// Option data storage in KVPs. Such as option `-L` would access it by "L" to retrieve the data. 
        /// </summary>
        /// <param name="key">Key to the data which mirrors the option.</param>
        /// <returns>A value found or string.empty</returns>
        public string this[string key] => _optionDataDictionary.ContainsKey(key) ? _optionDataDictionary[key] : string.Empty;
        #endregion

        #region Option Properties
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

        #endregion

        #region Construction/Initialization

        #endregion

        #region Methods

        public CommandLineManager Parse(string[] args)
        {
            if (args != null)
            {
                var arguments = args.Where(arg => !string.IsNullOrEmpty(arg)) // Sanity check
                                    .Select((arg, index) => new Argument()
                                                           {
                                                               Index      = index,
                                                               Text       = arg,
                                                           })
                                    .ToList();

                var props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .ToList();

                Options.Where(opt => opt.ArgumentFoundCheck(arguments))
                       .ToList()
                       .ForEach(opt =>
                                       {
                                           props.FirstOrDefault(prp => prp.Name == opt.Letter)?.SetValue(this, true, null);

                                           if (opt.HasAssociatedData && string.IsNullOrEmpty(opt.Data) == false)
                                               _optionDataDictionary.Add(opt.Letter, opt.Data);

                                       }); // TODO Ignore Case scenario?);

            }

            HaveArgumentsBeenParsed = true;

            return this;
        }

        /// <summary>
        /// Each option found will be executed. Before execution the validation will be called
        /// and if it reports true (validation ok) the option's execute operation will be done. 
        /// </summary>
        /// <param name="args"></param>
        public void ExecuteOperation(string[] args = null)
        {
            if (HaveArgumentsBeenParsed == false)
                Parse(args);

            Options.Where(opt => opt.ArgumentMatched)
                   .ToList()
                   .ForEach(option =>
                {
                    if (option?.Validation?.Invoke(this) ?? true)
                        option?.Operation?.Invoke(this);
                });

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
        /// When an option needs secondary (assoicated) data this is the one to specify it.
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="description"></param>
        /// <param name="operation"></param>
        /// <param name="isOptional"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public CommandLineManager AddOptionRequiresData(string letter, string description, Action<CommandLineManager> operation = null, bool isOptional = true, bool ignoreCase = true)
        {
            LastOption = new Option(letter, description, operation, isOptional, ignoreCase) { HasAssociatedData = true};
            Options.Add(LastOption);
            return this;
        }


        /// <summary>
        /// The validation step is done when an option is found, but before an execute. If the return value is 'false' the execute will be canceled.
        /// </summary>
        /// <param name="validationFunc"></param>
        /// <returns></returns>
        public CommandLineManager AddValidation(Func<CommandLineManager, bool> validationFunc)
        {
            LastOption.Validation = validationFunc;
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


    #endregion


    }
}
