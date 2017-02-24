#CoolCommandLine 2017
CoolCommandLine is a turnkey C# command line argument processor which adheres to a design of chained method based queries during development and the use of runtime IOC operations specified by the developer. 

*See TurnKey Example below for all those definitions shown in practice.* 

To begin operations one just instantiates the `CommandLineManager` class followed by adding option(s) (`-{Option letter | Option word}`) which relate to specific command line option. Each option provides an optional executable action delegate to be executed as the final step. Finally calling `Execute( {arguments array} )` parses the arguments and executes any actions as setup.

##Turnkey Example: 

```C#
#using CoolCommandLine;
    
static void Main(string[] args)
{
  // Look for the "-L" on the command line and if found simulate an action by writing the event out to the console. 
  (new CommandLineManager()).AddOption("L, List", "List the data.", (clm)=> Console.WriteLine($"Listing operation returned {clm.L} "))
                            .Execute(args);
}
``` 
 Code | What it Does
 ---- | ------------
 `using CoolCommandLine;` | After installation and reference to the project is done, one adds a `#using CoolCommandLine;` to the file.
 `new CommandLineManager()` | Instantiate the CoolCommandLine manager.
 `.AddOption(...)` | Use `AddOption` method to define the command line options which are expected before an action can occur.
 `"L, List"` | Shows the author looking for a command line option of `-L`. This is by done specfying the the `L` (without a dash) and its long option of `List` in a comma seperated fasion. Requiring either a `-L` or a `-List` to be found in the arguments to have a targetted action to be done.
 `"List the data."` | Describe the option for later usage in an operations map. (Not shown here)
`(clm)=> ...` | The optional action delegate lamda to be executed when one of the command line switch options is specified.
`Console.WriteLine($"Listing operation returned {clm.L}")` | The `CommandLineManager` instance is passed in as `clm`. That instance has a property `clm.L` will return `true` indicating that they user wants `L` action done. One can also check other properties such as `clm.[A-Z] for there status within the `L` operation.
`.Execute(args);` | Arguments are to be parsed and if any of the options are found, to execute its action operation.

###Purpose 
To show that once the `-L` is found from any of the command line argument, that a callback/delegate will be executed. During that execution  the whole `CommandLineManager` instance is passed in as an argument to so that  individual option properties can be checked for `true` or `false`.

---

##Needs Associated Data Example

When an option needs to have following data use the `AddOptionRequiresData` method in the same fashion as the `AddOption`. 

```C#
 var clm = new CommandLineManager();

 clm.AddOptionRequiresData("D", 
                           "Directory To Use.", 
                           (clmanager) =>  Console.WriteLine($"D option has this data {clmanager["D"]); );
 
 clm.Execute(args);

```
