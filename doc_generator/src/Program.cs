using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using UndertaleModLib;
using UndertaleModLib.Compiler;

namespace GMDocsGenerator;

public class Program
{
    static string templatesDirectory;
    static string functionsTemplate;
    static string argumentTemplate;
    static string functionsMenuTemplate;
    static string functionsEntry;

    public static void Main(string[] args)
    {
        DeleteDirectory_ExcludeFiles("./wysce", true, new string[] { ".git" });
        DeleteDirectory_ExcludeFiles("./wys_original", true, new string[] { ".git" });
        CopyDirectory_Recursive("./template_docs_extraFiles", "./", true);

        templatesDirectory = "./templates";
        functionsTemplate = File.ReadAllText(Path.Combine(templatesDirectory, "functions.md"));
        argumentTemplate = File.ReadAllText(Path.Combine(templatesDirectory, "arguments.md"));
        functionsMenuTemplate = File.ReadAllText(Path.Combine(templatesDirectory, "functionsMenu.md"));
        functionsEntry = File.ReadAllText(Path.Combine(templatesDirectory, "functionsMenuEntry.md"));


        string dataWinPath_ce = args[0];
        GenerateDocsForGame(dataWinPath_ce, "./wysce", "ce");

        string dataWinPath_og = args[1];
        GenerateDocsForGame(dataWinPath_og, "./wys_original", "og");
    }

    public static void GenerateDocsForGame(string dataWinPath, string outputDirectory, string fileEndingIdentifier)
    {

        Documentation docOverrides = JsonSerializer.Deserialize<Documentation>(File.ReadAllText($"./manual_overrides_{fileEndingIdentifier}.json"));
        Dictionary<string, FunctionDoc> functionOverrides = new Dictionary<string, FunctionDoc>();
        foreach (FunctionDoc funcOverride in docOverrides.functions)
        {
            functionOverrides.Add(funcOverride.name, funcOverride);
        }

        var config = new ConfigFile()
        {
            parent = "gml2",
            name = "Will You Snail?",
            indexingMode = "directory",
        };

        string existingFunctionsPath = $"./existingFunctions_{fileEndingIdentifier}.json";

        var stream = File.OpenRead(dataWinPath);
        var data = UndertaleIO.Read(stream);
        stream.Dispose();

        List<FunctionDoc> functionDocs = new List<FunctionDoc>();


        foreach (var codeEntry in data.Code)
        {
            string nameWithoutBeginning = codeEntry.Name.Content.Replace("gml_Script_", "");
            if (functionOverrides.ContainsKey(nameWithoutBeginning))
            {
                functionOverrides.TryGetValue(nameWithoutBeginning, out FunctionDoc curFuncOverride);
                bool outdated = false;
                int oldArgCount = curFuncOverride.argCount;
                if (oldArgCount != codeEntry.ArgumentsCount)
                {
                    Console.WriteLine("WARNING: " + nameWithoutBeginning + " has an outdated argument count, and may need to be updated.");
                    curFuncOverride.outdated = true;
                }
                functionDocs.Add(curFuncOverride);
            }
            else
            {
                if (codeEntry.Name.Content.StartsWith("gml_Script_") && !codeEntry.Name.Content.Contains("___struct___") && !codeEntry.Name.Content.Contains("gml_GlobalScript_") && !codeEntry.Name.Content.Replace("gml_Script_", "").Replace("gml_Script_", "").StartsWith("anon"))
                {
                    string funcName = nameWithoutBeginning;
                    List<ArgumentDoc> codeArgs = new List<ArgumentDoc>();
                    for (int i = 0; i < codeEntry.ArgumentsCount; i++)
                    {
                        codeArgs.Add(new ArgumentDoc
                        {
                            name = $"argument{i}{(i != codeEntry.ArgumentsCount - 1 ? "" : "")}",
                            description = "",
                            optional = false
                        });
                    }

                    FunctionDoc newFunction = new FunctionDoc
                    {
                        name = funcName,
                        argCount = codeEntry.ArgumentsCount,
                        arguments = codeArgs.ToArray(),
                        description = ""
                    };

                    functionDocs.Add(newFunction);
                }
            }
        }

        Documentation documentation = new Documentation
        {
            functions = functionDocs.ToArray()
        };

        CreateGithubDocs(documentation, outputDirectory);

        File.WriteAllText(
            existingFunctionsPath,
            JsonSerializer.Serialize<Documentation>(documentation));
    }


    public static void CreateGithubDocs(Documentation documentation, string outputDirectory)
    {
        string functionsDirectory = Path.Combine(outputDirectory, "functions");
        Directory.CreateDirectory(functionsDirectory);

        string functionsList = "";
        foreach (FunctionDoc funcDoc in documentation.functions)
        {
            string newDocument = functionsTemplate;
            if (funcDoc.outdated) newDocument = newDocument.Replace("# [FUNCTION_NAME]", funcDoc.name + "\nNOTE: This documemntation has an outdated argument count, and may need to be updated.");
            newDocument = newDocument.Replace("[FUNCTION_NAME]", funcDoc.name);

            string argsString = "";
            foreach (ArgumentDoc argDoc in funcDoc.arguments)
            {
                string argumentStr = argumentTemplate.Replace("[ARGUMENT_NAME]", argDoc.name);
                string argumentDescription = argDoc.description;
                if (argDoc.optional) argumentStr += "\n(Optional)";

                argumentStr = argumentStr.Replace("[DESCRIPTION]", argumentDescription);
                argsString += argumentStr + "  \n";
            }

            if (!string.IsNullOrEmpty(funcDoc.description)) newDocument = newDocument.Replace("[DESCRIPTION]", funcDoc.description);
            else newDocument = newDocument.Replace("[DESCRIPTION]", "(No description provided. Feel free to make a pull request!)");

            newDocument = newDocument.Replace("[ARGUMENTS]", argsString);

            string funcOutPath = Path.Combine(functionsDirectory, funcDoc.name + ".md");
            File.WriteAllText(funcOutPath, newDocument);

            functionsList += functionsEntry.Replace("[FUNCTION_NAME]", funcDoc.name);
        }

        File.WriteAllText(Path.Combine(outputDirectory, "functions.md"), functionsMenuTemplate.Replace("[FUNCTION_LIST]", functionsList));
    }

    public static void CopyDirectory_Recursive(string oldDirectory, string newDirectory, bool overwrite = false)
    {
        if (!Directory.Exists(newDirectory))
        {
            Directory.CreateDirectory(newDirectory);
        }
        foreach (string file in Directory.GetFiles(oldDirectory))
        {
            File.Copy(file, Path.Combine(newDirectory, Path.GetFileName(file)), overwrite);
        }
        foreach (string directory in Directory.GetDirectories(oldDirectory))
        {
            CopyDirectory_Recursive(directory, Path.Combine(newDirectory, Path.GetFileName(directory)));
        }
    }
    public static void DeleteDirectory_ExcludeFiles(string directory, bool recursive, string[] excludedFiles, bool deleteTopDirectory = false, int curDepth = 0)
    {
        foreach (string file in Directory.GetFiles(directory))
        {
            if (!excludedFiles.Contains(Path.GetFileName(file), StringComparer.OrdinalIgnoreCase) && !excludedFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
            {
                File.Delete(file);
            }
        }
        if (recursive)
        {
            foreach (string subdirectory in Directory.GetDirectories(directory))
            {
                if (!excludedFiles.Contains(Path.GetFileName(subdirectory), StringComparer.OrdinalIgnoreCase) && !excludedFiles.Contains(subdirectory, StringComparer.OrdinalIgnoreCase))
                {
                    DeleteDirectory_ExcludeFiles(Path.Combine(directory, Path.GetFileName(subdirectory)), true, excludedFiles, deleteTopDirectory = false, curDepth + 1);
                }
            }
        }

        if (!deleteTopDirectory && curDepth == 0) return;
        if (!excludedFiles.Contains(Path.GetFileName(directory), StringComparer.OrdinalIgnoreCase) && !excludedFiles.Contains(directory, StringComparer.OrdinalIgnoreCase))
        {
            Directory.Delete(directory);
        }
    }
}
