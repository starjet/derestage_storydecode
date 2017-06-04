using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Story.Data;
using System.IO;

namespace storydecode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void decodeRaw(string srcpath)
        {
            Parser parser = Parser.Create();
            byte[] bytes = File.ReadAllBytes(Path.GetFullPath(srcpath));
            string name = Path.GetFileNameWithoutExtension(srcpath);
            List<CommandStruct> commandlist = parser.ConvertBinaryToCommandList(bytes);
            string line = "";
            foreach (CommandStruct command in commandlist)
            {
                if (command.Name.Equals("title"))
                {
                    name = String.Concat(name, " [", command.Args[0], "]-rawdata.txt");
                    string illegalchars = String.Concat(new string(Path.GetInvalidFileNameChars()), new string(Path.GetInvalidPathChars()));
                    foreach (char c in illegalchars)
                    {
                        name = name.Replace(c.ToString(), String.Empty);
                    }
                }
                line = String.Concat(line, command.Name, "\r\n");
                foreach (string s in command.Args)
                {
                    line = String.Concat(line, s, "\r\n");
                }
                line = String.Concat(line, command.Category.ToString(), "\r\n", "\r\n");
            }
            File.WriteAllText(String.Concat(Path.GetDirectoryName(Path.GetFullPath(srcpath)), Path.DirectorySeparatorChar.ToString(), "", Path.DirectorySeparatorChar.ToString(), name), line);
        }

        public void decodeDerestageStory(string srcpath)
        {
            try
            {
                Parser parser = Parser.Create();
                byte[] bytes = File.ReadAllBytes(Path.GetFullPath(srcpath));
                string name = Path.GetFileNameWithoutExtension(srcpath);
                List<CommandStruct> commandlist = parser.ConvertBinaryToCommandList(bytes);
                string line = "";
                int i = 1;
                foreach (CommandStruct command in commandlist)
                {
                    if (command.Name.Equals("title"))
                    {
                        name = String.Concat(name, " [", command.Args[0], "].txt");
                        string illegalchars = String.Concat(new string(Path.GetInvalidFileNameChars()), new string(Path.GetInvalidPathChars()));
                        foreach (char c in illegalchars)
                        {
                            name = name.Replace(c.ToString(), String.Empty);
                        }
                    }
                    if (command.Name.Equals("print"))
                    {
                        int j = 1;
                        foreach (string arg in command.Args)
                        {
                            line = String.Concat(line, i.ToString(), ".", j.ToString(), " ", arg, "\r\n");
                            j++;
                        }
                        i++;
                        line = String.Concat(line, "\r\n");
                    }
                    if (command.Name.Equals("choice") || command.Name.Equals("outline") || command.Name.Equals("situation"))
                    {
                        int j = 1;
                        foreach (string arg in command.Args)
                        {
                            line = String.Concat(line, i.ToString(), ".", j.ToString(), " [" + command.Name + "] ", arg, "\r\n");
                            j++;
                        }
                        i++;
                        line = String.Concat(line, "\r\n");
                    }
                }
                string filepath = String.Concat(Path.GetDirectoryName(Path.GetFullPath(srcpath)), Path.DirectorySeparatorChar.ToString(), "out", Path.DirectorySeparatorChar.ToString(), name);
                try
                {
                    Directory.CreateDirectory("out");
                }
                catch { }
                if (!File.Exists(filepath))
                {
                    File.WriteAllText(filepath, line);
                }
                else
                {
                    File.WriteAllText(filepath + new Random().NextDouble().ToString().Replace(".", "_"), line);
                }
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            string srcpath;
            if (args.Length > 1)
            {
                srcpath = args[1];
            }
            else
            {
                OpenFileDialog fd = new OpenFileDialog();
                if (fd.ShowDialog().Equals(DialogResult.Cancel))
                {
                    Environment.Exit(1);
                }
                srcpath = fd.FileName;
            }

            decodeDerestageStory(srcpath);
            //decodeRaw(srcpath);
            //LoadFromPlaneFile(srcpath);
            //BuildPlaneFile(srcpath);
            Environment.Exit(0);
        }

        public void LoadFromPlaneFile(string srcpath)
        {
            string newpath = srcpath.Replace(".txt", ".bytes");
            Parser.Create().ConvertStoryDataTextToBinary(ref srcpath, ref newpath);
        }

        public void BuildPlaneFile(string srcpath)
        {
            string[] needquotes = new string[] { "title", "outline", "situation", "bgm", "vo" };
            string[] special = new string[] { "print", "log", "choice", "touch" };
            byte[] bytes = File.ReadAllBytes(srcpath);
            List<CommandStruct> commandlist = Parser.Create().ConvertBinaryToCommandList(bytes);
            string content = "";
            bool startedprinting = false;
            foreach (CommandStruct command in commandlist)
            {
                if (!startedprinting && command.Name == "print")
                {
                    startedprinting = true;
                    content += command.Name;
                    content += " \"" + command.Args[0] + "\"";
                    for (int i = 1; i < command.Args.Count; i++)
                    {
                        content += " \"" + command.Args[i];
                    }
                }
                else
                {
                    if (startedprinting && command.Name == "print")
                    {
                        for (int i = 1; i < command.Args.Count; i++)
                        {
                            content += command.Args[i];
                        }
                    }
                }
                if (command.Name == "touch")
                {
                    startedprinting = false;
                    content += "\"\r\n";
                }
                if (command.Name == "log")
                {
                    content += command.Name + " " + command.Args[0] + " ";
                    for (int i = 1; i < command.Args.Count; i++)
                    {
                        content += "\"" + command.Args[i] + "\" ";
                    }
                    content += "\r\n";
                }
                if (command.Name == "choice")
                {
                    content += command.Name + " \"" + command.Args[0] + "\" ";
                    for (int i = 1; i < command.Args.Count; i++)
                    {
                        content += command.Args[i] + " ";
                    }
                    content += "\r\n";
                }
                if (!startedprinting && !special.Contains(command.Name))
                {
                    if (needquotes.Contains(command.Name))
                    {
                        content += command.Name;
                        for (int i = 0; i < command.Args.Count; i++)
                        {
                            content += " \"" + command.Args[i] + "\"";
                        }
                    }
                    else
                    {
                        content += command.Name;
                        for (int i = 0; i < command.Args.Count; i++)
                        {
                            content += " " + command.Args[i];
                        }
                    }
                    content += "\r\n";
                }
                if (startedprinting && !special.Contains(command.Name))
                {
                    if (needquotes.Contains(command.Name))
                    {
                        content += "<" + command.Name;
                        for (int i = 0; i < command.Args.Count; i++)
                        {
                            content += " \"" + command.Args[i] + "\"";
                        }
                        content += ">";
                    }
                    else
                    {
                        content += "<" + command.Name;
                        for (int i = 0; i < command.Args.Count; i++)
                        {
                            content += " " + command.Args[i];
                        }
                        content += ">";
                    }
                }
            }
            File.WriteAllText(srcpath + "-plane.txt", content);
        }
    }
}
