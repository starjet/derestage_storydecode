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
            File.WriteAllText(String.Concat(Path.GetDirectoryName(Path.GetFullPath(srcpath)), Path.DirectorySeparatorChar.ToString(), "out", Path.DirectorySeparatorChar.ToString(), name), line);
        }

        public void serializeDerestageCommand(CommandStruct command)
        {

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
                File.WriteAllBytes(fd.FileName + ".old", Parser.Create().callConvertToByte("おはようございます ! "));
                File.WriteAllBytes(fd.FileName + ".new", Parser.Create().callConvertToByte("Ｍｏｒｎｉｎｇ　　 ! "));
            }
            
            //decodeDerestageStory(srcpath);
            //decodeRaw(srcpath);
            Environment.Exit(0);
        }
    }
}
