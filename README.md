# CoolCommandLine
CoolCommandLine 2017 is a turnkey out of the box C# commandline processor which adheres to method based queries. In its basic form one just instantiates the `CommandLineManager` instance, then adds option(s) and provides an optional executable delegate, per each option, to be executed as the final step.

    static void Main(string[] args)
    {

    (new CommandLineManager()).AddOption("L, List", "List the data.", (clm)=> Console.WriteLine("Listing operation"))
                              .Execute(args);
    }
