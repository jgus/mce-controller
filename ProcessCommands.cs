//-------------------------------------------------------------------
// Copyright © 2012 Kindel Systems, LLC
// http://www.kindel.com
// charlie@kindel.com
// 
// Published under the MIT License.
// Source control on SourceForge 
//    http://sourceforge.net/projects/mcecontroller/
//-------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;

namespace MCEControl
{
    /// <summary>
    /// Summary description for StartProcessCommands.
    /// </summary>
    public class StartProcessCommand : Command
    {
        [XmlAttribute("File")]
        public string File;
        [XmlAttribute("Arguments")]
        public string Arguments;
        [XmlAttribute("StopCmd")]
        public string StopCommand;
        [XmlAttribute("StopFile")]
        public string StopFile;
        [XmlAttribute("StopArguments")]
        public string StopArguments;

        [DefaultValue(true)]
        [XmlAttribute("LogErrors")]
        public bool LogErrors;

        [XmlElement("StartProcess", typeof(StartProcessCommand))]
        [XmlElement("SendInput", typeof(SendInputCommand))]
        [XmlElement("SendMessage", typeof(SendMessageCommand))]
        [XmlElement(typeof(Command))]
        public Command NextCommand;

        [XmlIgnore]
        private Executor executor;

        public override void Execute(Reply reply)
        {
            Stop(reply);
            if (File != null)
            {
                executor = new Executor(File, Arguments, LogErrors);
                if (!string.IsNullOrWhiteSpace(StopFile))
                {
                    executor.StopExecutor = new Executor(StopFile, StopArguments, LogErrors);
                }
                else if (!string.IsNullOrWhiteSpace(StopArguments))
                {
                    executor.StopExecutor = new Executor(File, StopArguments, LogErrors);
                }
                executor.Run();

                if (NextCommand != null)
                    executor.WaitForInputIdle(TimeSpan.FromSeconds(10)); // TODO: Make this settable
            }

            if (NextCommand != null)
                NextCommand.Execute(reply);
        }

        public void Stop(Reply reply)
        {
            if (executor != null)
            {
                executor.Stop();
                executor = null;
            }
        }

        public override IEnumerable<Command> AutoCommands()
        {
            if (!string.IsNullOrWhiteSpace(StopCommand))
            {
                yield return new StopProcessCommand(this);
            }
        }

        private class Executor
        {
            public Executor StopExecutor = null;

            public Executor(string File, string Arguments, bool logErrors)
            {
                this.logErrors = logErrors;
                var startInfo = new ProcessStartInfo
                {
                    FileName = File,
                    Arguments = Arguments,
                    WorkingDirectory = new System.IO.FileInfo(File).DirectoryName,
                };
                if (logErrors)
                {
                    startInfo.UseShellExecute = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                }
                process = new Process { StartInfo = startInfo };
                if (logErrors)
                {
                    process.OutputDataReceived += OnOutputReceived;
                    process.ErrorDataReceived += OnErrorReceived;
                    process.EnableRaisingEvents = true;
                }
            }

            public void Run()
            {
                new Thread(() =>
                {
                    process.Start();
                    Log($"Started process \"{process.StartInfo.FileName}\" with arguments \"{process.StartInfo.Arguments}\"");
                    if (logErrors)
                    {
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                    }
                    process.WaitForExit();
                    Log($"Process exited with code {process.ExitCode} (0x{process.ExitCode:X})");
                }).Start();
            }

            public void Stop()
            {
                bool exited = false;
                try
                {
                    if (StopExecutor != null)
                    {
                        StopExecutor.Run();
                    }
                    else
                    {
                        process.CloseMainWindow();
                    }
                    exited = WaitForExit(ExitTimeout);
                }
                catch (Exception)
                {
                    // Ignore
                }
                if (!exited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch (Exception)
                    {
                        // Ignore
                    }
                }
                if (StopExecutor != null)
                {
                    StopExecutor.Stop();
                }
            }

            public bool WaitForExit(TimeSpan timeSpan)
            {
                return process.WaitForExit((int)timeSpan.TotalMilliseconds);
            }

            public void WaitForInputIdle(TimeSpan timeSpan)
            {
                process.WaitForInputIdle((int)timeSpan.TotalMilliseconds);
            }

            private Process process;
            private bool logErrors;

            private TimeSpan ExitTimeout { get { return TimeSpan.FromSeconds(5); } }

            private void Log(string message)
            {
                MainWindow.AddLogEntry($"[{process.Id:D5}] {message}");
            }

            private void OnOutputReceived(object sender, DataReceivedEventArgs e)
            {
                if (e == null || e.Data == null) { return; }
                Log($"OUT: {e.Data}");
            }

            private void OnErrorReceived(object sender, DataReceivedEventArgs e)
            {
                if (e == null || e.Data == null) { return; }
                Log($"ERR: {e.Data}");
            }
        }
    }

    public class StopProcessCommand : Command
    {
        private StartProcessCommand startProcessCommand;

        public StopProcessCommand(StartProcessCommand startProcessCommand)
        {
            Key = startProcessCommand.StopCommand;
            this.startProcessCommand = startProcessCommand;
        }

        public override void Execute(Reply reply)
        {
            startProcessCommand.Stop(reply);
        }
    }
}