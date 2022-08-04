using System.CommandLine;

static class Program
{
    internal static void Main(string[] args)
    {
        var outputOption = new Option<string>(
                "--output",
                "Specify the output given to the user"
                ).FromAmong("stdout", "xlsx");
        outputOption.AddAlias("-o");

        var branchOption = new Option<string>(
                "--branch",
                "Specify the branch to filter by"
                );
        branchOption.AddAlias("-b");

        var rootCommand = new RootCommand("Get a tally of contributors' commits")
        {
            outputOption,
            branchOption,
        };

        rootCommand.SetHandler((outputOptionValue, branchOptionValue) => {
                    CommitDetail commits = new CommitDetail();
                    
                    switch (outputOptionValue) 
                    {
                        case "stdout":
                            StdOutDataService outDataService = new StdOutDataService();
                            DataAccess dataAccess = new DataAccess(outDataService);

                            switch (branchOptionValue) 
                            {
                                case null:
                                    commits.GetCurrentCommitsByName();
                                    dataAccess.WriteData(commits.CommitDetails);
                                    break;
                                default:
                                    commits.GetCommitsByBranch(branchOptionValue);
                                    dataAccess.WriteData(commits.CommitDetails);
                                    break;
                            }
                            break;
                        case "xlsx":
                            ExcelDataService excelDataService = new ExcelDataService();
                            DataAccess dataAccessExcelCase = new DataAccess(excelDataService);

                            switch (branchOptionValue)
                            {
                                case null:
                                    commits.GetCurrentCommitsByName();
                                    dataAccessExcelCase.WriteData(commits.CommitDetails);
                                    break;
                                default:
                                    commits.GetCommitsByBranch(branchOptionValue);
                                    dataAccessExcelCase.WriteData(commits.CommitDetails);
                                    break;
                            }
                            break;
                        case null:
                            StdOutDataService stdOutDataService = new StdOutDataService();
                            DataAccess dataAccessNullCase = new DataAccess(stdOutDataService);

                            switch (branchOptionValue)
                            {
                                case null:
                                    commits.GetCurrentCommitsByName();
                                    dataAccessNullCase.WriteData(commits.CommitDetails);
                                    break;
                                default:
                                    commits.GetCommitsByBranch(branchOptionValue);
                                    dataAccessNullCase.WriteData(commits.CommitDetails);
                                    break;
                            }
                            break;
                        default:
                            System.Console.WriteLine("This should not happen...");
                            Environment.Exit(90);
                            break;
                    }
                },
                outputOption, branchOption);

        rootCommand.Invoke(args);
    }
}
