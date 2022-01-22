using System.Diagnostics;

namespace Cosmology.Extensions;

internal static class ProcessExtensions
{
    public static async Task<int> CompleteAsync(
        this Process process,
        CancellationToken? cancellationToken = null) =>
        await Task.Run(() =>
        {
            process.WaitForExit();

            return Task.FromResult(process.ExitCode);
        }, cancellationToken ?? CancellationToken.None);

    public static Process StartProcess(
        string command,
        string args,
        string? workingDir = null,
        Action<string>? stdOut = null,
        Action<string>? stdErr = null,
        params (string key, string value)[] environmentVariables)
    {
        args ??= "";

        var process = new Process
        {
            StartInfo =
            {
                Arguments = args,
                FileName = command,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            }
        };

        if (!string.IsNullOrWhiteSpace(workingDir))
        {
            process.StartInfo.WorkingDirectory = workingDir;
        }

        if (environmentVariables.Length > 0)
        {
            for (var i = 0; i < environmentVariables.Length; i++)
            {
                var (key, value) = environmentVariables[i];
                process.StartInfo.Environment.Add(key, value);
            }
        }

        if (stdOut != null)
        {
            process.OutputDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data != null)
                {
                    stdOut(eventArgs.Data);
                }
            };
        }

        if (stdErr != null)
        {
            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                if (eventArgs.Data != null)
                {
                    stdErr(eventArgs.Data);
                }
            };
        }

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }
}