#CoolCommandLine 2017
CoolCommandLine is a turnkey C# commandline processor which adheres to method based queries. In its basic form one just instantiates the `CommandLineManager` instance followed by adding option(s). Each option provides an optional executable delegate to be executed as the final step.

##Turnkey example: 

```C#
 #using CoolCommandLine;
    
 static void Main(string[] args)
 {
    // Look for the "-L" on the command line and if found simulate an action by writing the event out to the console. 
    (new CommandLineManager()).AddOption("L, List", "List the data.", (clm)=> Console.WriteLine($"Listing operation returned {clm.L} "))
                              .Execute(args);
 }
```
##Demos
 - After installation and reference to the project is done, one adds a `#using CoolCommandLine;` to the file.
 - Shows the author looking for a command line option of `-L` by specfying the short `L` and its long option of `List` in a comma seperated fasion. 
 - Requiring either a `-L` or a `-List` found in the arguments to execute the action..
 - Shows that once the `-L` is found from the command line arguments that a callback/delegate will be executed and the whole `CommandLineManager` instance is returned to so that  individual option properties can be checked for `true` or `false`.
