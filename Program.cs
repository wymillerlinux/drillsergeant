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

        var tagOption = new Option<string>(
                "--tag",
                "Specify the tag to filter by"
                );
        tagOption.AddAlias("-t");

        var rootCommand = new RootCommand("Get a tally of contributors' commits")
        {
            outputOption,
            branchOption,
            tagOption,
        };

        rootCommand.SetHandler((outputOptionValue, branchOptionValue, tagOptionValue) => {
                    CommitDetail commits = new CommitDetail();
                    
                    switch (outputOptionValue) 
                    {
                        case "stdout":
                            StdOutDataService outDataService = new StdOutDataService();
                            DataAccess dataAccess = new DataAccess(outDataService);

                            if (branchOptionValue != null && tagOptionValue != null) {
                                Console.WriteLine("Please specify either a branch or a tag");
                                Environment.Exit(2);
                            } else if (branchOptionValue != null && tagOptionValue == null) {
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
                            } else if (branchOptionValue == null && tagOptionValue != null) {
                                commits.GetCommitsByTag(tagOptionValue);
                                dataAccess.WriteData(commits.CommitDetails);
                            }
                            break;
                        case "xlsx":
                            ExcelDataService excelDataService = new ExcelDataService();
                            DataAccess dataAccessExcelCase = new DataAccess(excelDataService);

                            if (branchOptionValue != null && tagOptionValue != null) {
                                Console.WriteLine("Please specify either a branch or a tag.");
                                Environment.Exit(2);
                            } else if (branchOptionValue != null && tagOptionValue == null) {
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
                            } else if (branchOptionValue == null && tagOptionValue != null) {
                                commits.GetCommitsByTag(tagOptionValue);
                                dataAccessExcelCase.WriteData(commits.CommitDetails);
                            }
                            break;
                        case null:
                            StdOutDataService stdOutDataService = new StdOutDataService();
                            DataAccess dataAccessNullCase = new DataAccess(stdOutDataService);

                            if (branchOptionValue != null && tagOptionValue != null) {
                                Console.WriteLine("Please specify either a branch or a tag.");
                                Environment.Exit(2);
                            } else if (branchOptionValue != null && tagOptionValue == null) {
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
                            } else if (branchOptionValue == null && tagOptionValue != null) {
                                commits.GetCommitsByTag(tagOptionValue);
                                dataAccessNullCase.WriteData(commits.CommitDetails);
                            } else {
                                commits.GetCurrentCommitsByName();
                                dataAccessNullCase.WriteData(commits.CommitDetails);
                            }
                            break;
                        default:
                            System.Console.WriteLine("This should not happen...");
                            Environment.Exit(90);
                            break;
                    }
                },
                outputOption, branchOption, tagOption);

        rootCommand.Invoke(args);
    }
}
