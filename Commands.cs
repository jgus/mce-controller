//-------------------------------------------------------------------
// Copyright © 2012 Kindel Systems, LLC
// http://www.kindel.com
// charlie@kindel.com
// 
// Published under the MIT License.
// Source control on SourceForge 
//    http://sourceforge.net/projects/mcecontroller/
//-------------------------------------------------------------------

//#define SERIALIZE

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using WindowsInput;
using WindowsInput.Native;

namespace MCEControl
{
    // Base class for all Command types
    public abstract class Command
    {
        [XmlAttribute("Cmd")]
        public string Key;

        public abstract void Execute(Reply reply);
        public virtual IEnumerable<Command> AutoCommands() { yield break; }
    }

    // Note, do not change the namespace or your will break existing installations
    [XmlType(Namespace = "http://www.kindel.com/products/mcecontroller", TypeName = "MCEController")]
    public class CommandTable
    {
        [XmlIgnore]
        private readonly Dictionary<string, Command> _hashTable = new Dictionary<string, Command>();

        [XmlArray("Commands")]
        [XmlArrayItem("StartProcess", typeof(StartProcessCommand))]
        [XmlArrayItem("SendInput", typeof(SendInputCommand))]
        [XmlArrayItem("SendMessage", typeof(SendMessageCommand))]
        [XmlArrayItem("SetForegroundWindow", typeof(SetForegroundWindowCommand))]
        [XmlArrayItem("Shutdown", typeof(ShutdownCommand))]
        [XmlArrayItem(typeof(Command))]
        public Command[] List;

        public int CommandCount
        {
            get { return _hashTable.Count; }
        }

        public void Execute(Reply reply, string cmd)
        {
            if (!MainWindow.MainWnd.Settings.DisableInternalCommands)
            {
                if (cmd.StartsWith(McecCommand.CmdPrefix))
                {
                    var command = new McecCommand(cmd);
                    command.Execute(reply);
                    return;
                }

                if (cmd.StartsWith("chars:"))
                {
                    // "chars:<chars>
                    string chars = Regex.Unescape(cmd.Substring(6, cmd.Length - 6));
                    MainWindow.AddLogEntry(string.Format("Cmd: Sending {0} chars: {1}", chars.Length, chars));
                    var sim = new InputSimulator();
                    sim.Keyboard.TextEntry(chars);
                    return;
                }

                if (cmd.StartsWith("api:"))
                {
                    // "api:API(params)
                    // TODO: Implement API stuff
                    return;
                }

                if (cmd.StartsWith("shiftdown:"))
                {
                    // Modifyer key down
                    SendInputCommand.ShiftKey(cmd.Substring(10, cmd.Length - 10), true);
                    return;
                }

                if (cmd.StartsWith("shiftup:"))
                {
                    // Modifyer key up
                    SendInputCommand.ShiftKey(cmd.Substring(8, cmd.Length - 8), false);
                    return;
                }

                if (cmd.StartsWith(MouseCommand.CmdPrefix))
                {
                    // mouse:<action>[,<parameter>,<parameter>]
                    var mouseCmd = new MouseCommand(cmd);
                    mouseCmd.Execute(reply);
                    return;
                }

                if (cmd.StartsWith("stopall:"))
                {
                    StopAll(reply);
                    return;
                }

                if (cmd.Length == 1)
                {
                    // It's a single character, just send it
                    // must be upper case (VirtualKeyCode codes are for upper case)
                    cmd = cmd.ToUpper();
                    char c = cmd.ToCharArray()[0];

                    var sim = new InputSimulator();

                    MainWindow.AddLogEntry("Cmd: Sending keydown for: " + cmd);
                    sim.Keyboard.KeyPress((VirtualKeyCode)c);
                    return;
                }
            }

            // Command is in MCEControl.commands
            {
                Command command;
                if (_hashTable.TryGetValue(cmd.ToUpper(), out command))
                {
                    command.Execute(reply);
                }
                else
                {
                    MainWindow.AddLogEntry($"Unknown command: \"{cmd}\"");
                }
            }
        }

        private void StopAll(Reply reply)
        {
            foreach (var stopCommand in _hashTable.Values.OfType<StopProcessCommand>())
            {
                stopCommand.Execute(reply);
            }
        }

        private void AddCommand(Command command)
        {
            var key = command.Key.ToUpper();
            //MainWindow.AddLogEntry($"Adding command \"{key}\": {command.GetType().Name}");
            _hashTable[key] = command;
            foreach (var autoCommand in command.AutoCommands())
            {
                AddCommand(autoCommand);
            }
        }

        public static CommandTable Deserialize(bool DisableInternalCommands)
        {
            CommandTable cmds = null;
            CommandTable userCmds = null;

            if (!DisableInternalCommands)
            {
                // Load the built-in commands from an assembly resource
                try
                {
                    var serializer = new XmlSerializer(typeof(CommandTable));
                    XmlReader reader =
                        new XmlTextReader(
                            Assembly.GetExecutingAssembly()
                                .GetManifestResourceStream("MCEControl.Resources.MCEControl.commands"));
                    cmds = (CommandTable)serializer.Deserialize(reader);
                    foreach (var cmd in cmds.List)
                    {
                        cmds.AddCommand(cmd);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("No commands loaded. Error parsing built-in commands. {0}",
                        ex.Message));
                    MainWindow.AddLogEntry(
                        string.Format("MCEC: No commands loaded. Error parsing built-in commands. {0}",
                            ex.Message));
                    Util.DumpException(ex);
                    return null;
                }
                // Populate default VK_ codes
                foreach (VirtualKeyCode vk in Enum.GetValues(typeof(VirtualKeyCode)))
                {
                    string s;
                    if (vk > VirtualKeyCode.HELP && vk < VirtualKeyCode.LWIN)
                        s = vk.ToString();  // already have VK_
                    else
                        s = "VK_" + vk.ToString();
                    var cmd = new SendInputCommand(s, false, false, false, false);
                    if (!cmds._hashTable.ContainsKey(s))
                        cmds._hashTable.Add(s, cmd);
                }
            }
            else
            {
                cmds = new CommandTable();
            }
            // Load any over-rides from a text file
            FileStream fs = null;
            try
            {
                var serializer = new XmlSerializer(typeof(CommandTable));
                // A FileStream is needed to read the XML document.
                fs = new FileStream("MCEControl.commands", FileMode.Open, FileAccess.Read);
                XmlReader reader = new XmlTextReader(fs);
                userCmds = (CommandTable)serializer.Deserialize(reader);
                foreach (var cmd in userCmds.List)
                {
                    cmds.AddCommand(cmd);
                }
                MainWindow.AddLogEntry(string.Format("MCEC: User defined commands loaded."));
            }
            catch (FileNotFoundException ex)
            {
                MainWindow.AddLogEntry("MCEC: No user defined commands loaded; MCEControl.commands was not found.");
                Util.DumpException(ex);
            }
            catch (InvalidOperationException ex)
            {
                MainWindow.AddLogEntry(
                    string.Format("MCEC: No commands loaded. Error parsing MCEControl.commands file. {0} {1}", ex.Message,
                                  ex.InnerException.Message));
                Util.DumpException(ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("No commands loaded. Error parsing MCEControl.commands file. {0}",
                                              ex.Message));
                MainWindow.AddLogEntry(string.Format("MCEC: No commands loaded. Error parsing MCEControl.commands file. {0}",
                                                     ex.Message));
                Util.DumpException(ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return cmds;
        }
    }
}