using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace CoolCommandLine
{
    public class CommandLineManager
    {
        #region Variables
        private Dictionary<string, string> _optionDataDictionary = new Dictionary<string, string>();
        private Dictionary<string, bool>   _optionSetDictionary  = new Dictionary<string, bool>();
        #endregion

        #region Generic Actions
        /// <summary>
        /// When the user wants to do the titling, this action overrides the default titling when not null.
        /// </summary>
        public Action<CommandLineManager> TitleAction { get; set; } 

        /// <summary>
        /// When Description action is required the user can override the operation. 
        /// </summary>
        public Action<CommandLineManager> DescriptionAction { get; set; }

        /// <summary>
        /// Allows the user to provide text before a description action event.
        /// </summary>
        private Action<CommandLineManager> DescriptionActionBefore { get; set; }

        /// <summary>
        /// Allows the user to provide text after a description action event.
        /// </summary>
        private Action<CommandLineManager> DescriptionActionAfter { get; set; }




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
        /// Title of the application to be presented to the user.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The options which are setup for this operation.
        /// </summary>
        public List<Option> Options { get; set; } = new List<Option>();

        /// <summary>
        /// The last option added so we can associate specific add-on items to this option.
        /// </summary>
        private Option LastOption { get; set; }

        /// <summary>
        /// The user action to execute before the primary action.
        /// </summary>
        private Action<CommandLineManager> PreAction { get; set; }

        /// <summary>
        /// The post action to be executed.
        /// </summary>
        private Action<CommandLineManager> PostAction { get; set; }

        #endregion

        #region Operation Public Properties
        /// <summary>
        /// Used by the execute to determine whether the arguments need to be parsed.
        /// </summary>
        public bool HaveArgumentsBeenParsed { get; set; }

        /// <summary>
        /// Should we execute the titling and information operation when there are no actions?
        /// </summary>
        public bool HasShowTitleAndDescriptionsOnNoAction { get; set; }

        /// <summary>
        /// If after parsing it is determined that a command line option has been selected
        /// and there is an action specifed, this will be true.
        /// </summary>
        public bool HasOperationActionToDo { get; set; }


        #endregion

        #region Construction/Initialization

        public CommandLineManager()
        {
            
        }

        #endregion

        #region Methods

        /// <summary>
        /// Ingest runtime arguments and process them against the user options.
        /// </summary>
        /// <param name="args">Runtime arguments</param>
        /// <returns>A Command Line Manager for reuse.</returns>
        public CommandLineManager Parse(string[] args)
        {

            if (args != null)
            {
                var arguments = args.Where(arg => !string.IsNullOrEmpty(arg))        // Sanity check
                                    .Select((arg, index) => new Argument()           // Project it into an argument instance.
                                                           {
                                                               Index      = index,   // Index is used later to marry an option with data.
                                                               Text       = arg,
                                                           })
                                    .ToList();

                var props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                     .ToList();

                Options.Where(opt => opt.ArgumentFoundCheck(arguments))
                       .ToList()
                       .ForEach(opt =>
                                       {   // Reflect into this object and turn on the target option letter's bool
                                           props.FirstOrDefault(prp => prp.Name == opt.Letter)?.SetValue(this, true, null);

                                           opt.Letters.ForEach(word => _optionSetDictionary.Add(word, true));

                                           if (opt.NeedsAssociatedData && string.IsNullOrEmpty(opt.Data) == false)
                                           {
                                               opt.Letters.ForEach(word => _optionDataDictionary.Add(word, opt.Data)); // Add to all dictionary key possibilities
                                           }

                                           if (opt.Operation != null)
                                               HasOperationActionToDo = true; // Mark this status to true. 
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

            // Do we show the altnerate (user title?)
            if ((HasShowTitleAndDescriptionsOnNoAction) && (HasOperationActionToDo == false))
            {
                TitleAction?.Invoke(this);
                DescriptionActionBefore?.Invoke(this);
                DescriptionAction?.Invoke(this);
                DescriptionActionAfter?.Invoke(this);
            }

            if (HasOperationActionToDo)
            {
                PreAction?.Invoke(this);

                Options.Where(opt => opt.ArgumentMatched)
                       .ToList()
                       .ForEach(option =>
                    {
                        if ((option?.Validation?.Invoke(this) ?? true) && (option?.ValidationExtra?.Invoke(this, option) ?? true))
                            option?.Operation?.Invoke(this);
                    });

                PostAction?.Invoke(this);
            }
        }

        /// <summary>
        /// Instantiate a command line object and pass it on for use. 
        /// </summary>
        /// <returns></returns>
        public static CommandLineManager Instantiation()
        {
            return new CommandLineManager();
        }

        /// <summary>
        /// Take the calling executable and extract the version and append it to the title and output it to the console with a LineBreak
        /// before and after the tite line.
        /// </summary>
        /// <param name="title">Info about the title before the version. If default the name of the calling assembly.</param>
        /// <param name="afterTitle">Optional text to go after the version.</param>
        /// <returns></returns>
        public CommandLineManager DisplayTitleAndVersion(string title = "", string afterVersion = "")
        {
            //  var info = Assembly.GetCallingAssembly().FullName.Split(',');


            var info = new AssemblyInfoCalling();

            if (info != null)
            {

                string version = info.VersionFull;// info[1];

                string product = string.IsNullOrEmpty(title) ? $"{info.Title} (" : title;
                afterVersion = string.IsNullOrEmpty(afterVersion) ? ")" : afterVersion;

                Console.WriteLine($"{Environment.NewLine}{product}{Regex.Replace(version, @"(\sVersion=)", string.Empty)}{afterVersion}{Environment.NewLine}");
            }
            return this;
        }


        public CommandLineManager DisplayProductAndVersion(string product = "", string afterVersion = "")
        {
            //  var info = Assembly.GetCallingAssembly().FullName.Split(',');


            var info = new AssemblyInfoCalling();

            if (info != null)
            {

                string version = info.VersionFull;// info[1];

                product = string.IsNullOrEmpty(product) ? $"{info.Product} (" : product;
                afterVersion = string.IsNullOrEmpty(afterVersion) ? ")" : afterVersion;

                Console.WriteLine($"{Environment.NewLine}{product}{Regex.Replace(version, @"(\sVersion=)", string.Empty)}{afterVersion}{Environment.NewLine}");
            }
            return this;
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
        /// <returns>CommandLineManager</returns>
        public CommandLineManager AddOptionRequiresData(string letter, string description, Action<CommandLineManager> operation = null, bool isOptional = true, bool ignoreCase = true)
        {
            LastOption = new Option(letter, description, operation, isOptional, ignoreCase) { NeedsAssociatedData = true};
            Options.Add(LastOption);
            return this;
        }


        /// <summary>
        /// Define what options are related to the proceeded option declaration. Used for description processing.
        /// </summary>
        /// <param name="subOptions">Target options without dash character.</param>
        /// <returns>CommandLineManager</returns>
        public CommandLineManager AssociateWithSubOptions(params string[] subOptions)
        {
            LastOption.AssociatedWithOptions = subOptions.ToList();
            return this;
        }

        /// <summary>
        /// The validation step is done when an option is found, but before an execute. If the return value is 'false' the execute will be canceled.
        /// </summary>
        /// <param name="validationFunc"></param>
        /// <returns>CommandLineManager</returns>
        public CommandLineManager AddValidation(Func<CommandLineManager, bool> validationFunc)
        {
            LastOption.Validation = validationFunc;
            return this;
        }


        public CommandLineManager AddValidationOption(Func<CommandLineManager, Option, bool> validationFunc)
        {
            LastOption.ValidationExtra = validationFunc;
            return this;
        }


        /// <summary>
        /// Action to be executed before an execute (after validation) occurs.
        /// </summary>
        /// <param name="action">Target operation to be run.</param>
        /// <returns>CommandLineManager</returns>
        public CommandLineManager PreExecuteAction(Action<CommandLineManager> action)
        {
            PreAction = action;
            return this;
        }


        /// <summary>
        /// Action to be executed before an execute (after validation) occurs.
        /// </summary>
        /// <param name="action">Target operation to be run.</param>
        /// <returns>CommandLineManager</returns>
        public CommandLineManager PostExecuteAction(Action<CommandLineManager> action)
        {
            PostAction = action;
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

        /// <summary>
        /// If a multiple letter option is found, return true/false for its status.
        /// </summary>
        /// <remarks>This can work to check for a sigle letter word option can work.</remarks>
        /// <param name="wordOption">The multiple letter option, usually as a word.</param>
        /// <returns>True if set, false if not</returns>
        public bool IsMultipleLetterOptionFound(string wordOption)
        {
            return _optionSetDictionary.ContainsKey(wordOption) && _optionSetDictionary[wordOption];
        }

        /// <summary>
        /// When there is no action done, report the title and options. Selecting this
        /// performs a console.writeline. If another reporting is needed use the alternate reporting 
        /// action operation.
        /// </summary>
        /// <param name="alternateReporting">Used in-leue of console-writeline.</param>
        /// <returns>The operation to allow command chaining.</returns>
        public CommandLineManager ReportTitle(Action<CommandLineManager> alternateTitle = null)
        {
            HasShowTitleAndDescriptionsOnNoAction = true;
            TitleAction = alternateTitle;
            return this;
        }

        /// <summary>
        /// The user has indicated that an operation description should be provided when no actions
        /// are performed.
        /// </summary>
        /// <param name="alternateDescription">User supplied description operation; overrides the default</param>
        /// <returns>Returns the current instance for query chaining.</returns>
        public CommandLineManager DisplayDescriptionOnNoOperation(Action<CommandLineManager> alternateDescription = null)
        {
            HasShowTitleAndDescriptionsOnNoAction = true;
            DescriptionAction = alternateDescription ?? ProvideDescriptionWhenNoOpsDone;
            return this;
        }

        /// <summary>
        /// Provide the user an operation, most likely to write, text before the description when no operation occurs. 
        /// </summary>
        /// <param name="preDescription">User supplied operation</param>
        /// <returns>Returns the current instance for query chaining.</returns>
        public CommandLineManager DisplayBeforeDescription(Action<CommandLineManager> preDescription)
        {
            DescriptionActionBefore = preDescription;
            return this;
        }


        /// <summary>
        /// Provide the user an operation, most likely to write, text after the description when no operation occurs. 
        /// </summary>
        /// <param name="postDescription">User supplied operation</param>
        /// <returns>Returns the current instance for query chaining.</returns>
        public CommandLineManager DisplayAfterDescription(Action<CommandLineManager> postDescription)
        {
            DescriptionActionAfter = postDescription;
            return this;
        }

        /// <summary>
        /// This is the default description provider which will enumerate the options.
        /// </summary>
        /// <param name="obj">Not used main instance.</param>
        private void ProvideDescriptionWhenNoOpsDone(CommandLineManager obj)
        {
            StringBuilder sb = new StringBuilder();

            var PrimaryOptions = Options.Where(opt => opt.AssociatedWithOptions != null).ToList();

            if (PrimaryOptions.Any() == false) // Just show all options, there is not enough info to format with suboptions
            {

                Options.ForEach(opt =>
                {
                  //  if (opt.IsOptional)
                 //       sb.Append("\t");

                    sb.AppendLine(opt.ToString());
                });
            }
            else
            {
                PrimaryOptions.Select(prime => new
                {
                    Prime = prime,
                    SubOptions = Options.Where(opt => prime.AssociatedWithOptions.Contains(opt.Letter))
                                        .ToList()
                })
                .ToList()
                .ForEach(optimus =>
                {
                    sb.AppendLine($"{optimus.Prime.ToString()} using");
                    optimus.SubOptions.ForEach(sub => sb.AppendLine($"\t{sub.ToString()}"));
                    sb.AppendLine(string.Empty);
                });
            }

            Console.WriteLine(sb.ToString());

        }


       
        #endregion

    }
}
