 2017# CoolCommandLine
CoolCommandLine 2017 is a turnkey C# commandline processor which adheres to method based queries. In its basic form one just instantiates the `CommandLineManager` instance followed by adding option(s). Each option provides an optional executable delegate to be executed as the final step.

    #using CoolCommandLine;
    
    static void Main(string[] args)
    {
    // Look for the "-L" on the command line and if found simulate an action by writing the event out to the console. 
    (new CommandLineManager()).AddOption("L, List", "List the data.", (clm)=> Console.WriteLine($"Listing operation returned {clm.L} "))
                              .Execute(args);
    }
