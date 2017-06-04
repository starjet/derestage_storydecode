using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Story.Data
{
    [ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(IParser)), ComVisible(true)]
    public class Parser : IParser
    {
        private byte[] _binaryFileBuffer;

        private int _binaryFileBufferSize;

        private List<CommandStruct> _commandList;

        private Config _config;

        private Regex _headOnly;

        private Regex _tailOnly;

        private Regex _commentOut;

        private Regex _commentMultiOutHead;

        private Regex _commentMultiOutFoot;

        private Regex _commentMultiOut;

        public static Parser Create()
        {
            Parser parser = new Parser();
            parser.Init();
            return parser;
        }

        public void Init()
        {
            this._binaryFileBuffer = new byte[61440];
            this._commandList = new List<CommandStruct>();
            this._config = Config.Create();
            this._headOnly = new Regex("^[\"](.*)");
            this._tailOnly = new Regex("(.*)[\"]$");
            this._commentOut = new Regex("//.*");
            this._commentMultiOutHead = new Regex("/\\*.*");
            this._commentMultiOutFoot = new Regex(".*\\*/");
            this._commentMultiOut = new Regex("/\\*.*\\*/");
        }

        public string GetBinaryBufString()
        {
            return BitConverter.ToString(this._binaryFileBuffer, 0, this._binaryFileBufferSize);
        }

        private void AllocateFileBuffer(int size)
        {
            if (size > this._binaryFileBuffer.Length)
            {
                this._binaryFileBuffer = new byte[size];
            }
        }

        public List<CommandStruct> ConvertBinaryToCommandList(byte[] byteData)
        {
            this._binaryFileBuffer.Initialize();
            this._commandList.Clear();
            this._binaryFileBuffer = byteData;
            this._binaryFileBufferSize = this._binaryFileBuffer.Length;
            return this.Deserialize(ref this._binaryFileBuffer, this._binaryFileBufferSize);
        }

        public List<byte[]> LoadPlaneFile(ref string path)
        {
            StreamReader streamReader = new StreamReader(path, Encoding.UTF8);
            bool flag = false;
            List<string> list = new List<string>();
            while (streamReader.Peek() >= 0)
            {
                string text = streamReader.ReadLine();
                text = this._commentOut.Replace(text, string.Empty);
                text = this._commentMultiOut.Replace(text, string.Empty);
                if (flag)
                {
                    if (this._commentMultiOutFoot.IsMatch(text))
                    {
                        text = this._commentMultiOutFoot.Replace(text, string.Empty);
                        flag = false;
                    }
                    else
                    {
                        text = string.Empty;
                    }
                }
                else if (this._commentMultiOutHead.IsMatch(text))
                {
                    text = this._commentMultiOutHead.Replace(text, string.Empty);
                    flag = true;
                }
                list.Add(text);
            }
            return this.Serialize(ref list);
        }

        public void WriteSerializeData(ref string path, ref List<byte[]> byteList)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            int count = byteList.Count;
            for (int i = 0; i < count; i++)
            {
                byte[] array = byteList[i];
                fileStream.Write(array, 0, array.Length);
            }
            fileStream.Close();
        }

        public void ConvertStoryDataTextToBinary(ref string srcPath, ref string dstPath)
        {
            List<byte[]> list = this.LoadPlaneFile(ref srcPath);
            try
            {
                this.WriteSerializeData(ref dstPath, ref list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<byte[]> Serialize(ref List<string> fileData)
        {
            List<byte[]> list = new List<byte[]>();
            int count = fileData.Count;
            for (int i = 0; i < count; i++)
            {
                try
                {
                    byte[] item = this.SerializeLine(fileData[i]);
                    list.Add(item);
                }
                catch (Exception ex)
                {
                    string message = string.Format("StoryData Convert Error {0} : {1}", i + 1, ex);
                    throw new Exception(message, ex);
                }
            }
            return list;
        }

        private List<CommandStruct> Deserialize(ref byte[] byteList, int arraySize)
        {
            List<List<byte[]>> list = this.SplitCommandByteRow(ref byteList, arraySize);
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                CommandStruct item = this.DeserializeLine(list[i]);
                if (item.Name != null)
                {
                    this._commandList.Add(item);
                }
            }
            return this._commandList;
        }

        private CommandStruct DeserializeLine(List<byte[]> commandByte)
        {
            CommandStruct result = default(CommandStruct);
            result.Args = new List<string>();
            int id = (int)BitConverter.ToInt16(commandByte[0], 0);
            result.Name = this.GetCommandName(id);
            int count = commandByte.Count;
            for (int i = 1; i < count; i++)
            {
                result.Args.Add(this.ConvertStringArgs(commandByte[i]));
            }
            result.Category = this.GetCommandCategory(id);
            return result;
        }

        private List<List<byte[]>> SplitCommandByteRow(ref byte[] byteList, int arraySize)
        {
            List<List<byte[]>> list = new List<List<byte[]>>();
            byte[] array = new byte[4];
            for (int i = 2; i < arraySize; i += 2)
            {
                List<byte[]> list2 = new List<byte[]>();
                byte[] array2 = new byte[2];
                Array.Copy(byteList, i - 2, array2, 0, 2);
                Array.Reverse(array2);
                list2.Add(array2);
                int num = i;
                while (true)
                {
                    Array.Copy(byteList, num, array, 0, 4);
                    Array.Reverse(array);
                    int num2 = BitConverter.ToInt32(array, 0);
                    if (num2 == 0)
                    {
                        break;
                    }
                    byte[] array3 = new byte[num2];
                    Array.Copy(byteList, num + 4, array3, 0, num2);
                    list2.Add(array3);
                    num += 4 + num2;
                }
                i = num + 4;
                list.Add(list2);
            }
            return list;
        }

        private byte[] ByteListAppend(ref byte[] baseList, ref byte[] addList)
        {
            int num = baseList.Length;
            int num2 = addList.Length;
            byte[] array = new byte[num + num2];
            Array.Copy(baseList, array, num);
            Array.Copy(addList, 0, array, num, num2);
            return array;
        }

        private byte[] SerializeLine(string commandLine)
        {
            List<string> list = this.SplitString(ref commandLine);
            List<List<string>> list2 = new List<List<string>>();
            int count = list.Count;
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (list[i] == "<")
                {
                    List<string> range = list.GetRange(num, i - num);
                    num = i + 1;
                    list2.Add(range);
                }
            }
            if (num < count)
            {
                List<string> range2 = list.GetRange(num, count - num);
                list2.Add(range2);
            }
            byte[] result = new byte[0];
            byte[] bytes = BitConverter.GetBytes(0);
            int count2 = list2.Count;
            for (int j = 0; j < count2; j++)
            {
                List<string> list3 = list2[j];
                byte[] array = this.ConvertByteCommand(list3[0]);
                result = this.ByteListAppend(ref result, ref array);
                count = list3.Count;
                string text = list3[0];
                int commandID = this._config.GetCommandID(ref text);
                int num2 = count - 1;
                if (commandID == -1)
                {
                    if (num2 > 0 || text != string.Empty)
                    {
                        throw new Exception("不正なコマンドです : " + text);
                    }
                }
                else
                {
                    int commandMinArgCount = this._config.GetCommandMinArgCount(commandID);
                    int commandMaxArgCount = this._config.GetCommandMaxArgCount(commandID);
                    if (num2 < commandMinArgCount || num2 > commandMaxArgCount)
                    {
                        throw new ArgumentOutOfRangeException("引数の数が合いません");
                    }
                    for (int k = 1; k < count; k++)
                    {
                        byte[] array2 = this.ConvertByteArgs(list3[k]);
                        int value = array2.Length;
                        byte[] bytes2 = BitConverter.GetBytes(value);
                        Array.Reverse(bytes2);
                        result = this.ByteListAppend(ref result, ref bytes2);
                        result = this.ByteListAppend(ref result, ref array2);
                    }
                    result = this.ByteListAppend(ref result, ref bytes);
                }
            }
            if (list.IndexOf("print") >= 0 || list.IndexOf("double") >= 0)
            {
                byte[] array3 = this.ConvertByteCommand("touch");
                result = this.ByteListAppend(ref result, ref array3);
                result = this.ByteListAppend(ref result, ref bytes);
            }
            return result;
        }

        private List<string> SplitString(ref string commandLine)
        {
            List<string> list = new List<string>();
            List<bool> list2 = new List<bool>();
            int length = commandLine.Length;
            int num = 0;
            for (int i = 0; i < length; i++)
            {
                string a = commandLine.Substring(i, 1);
                int index = list2.Count - 1;
                if (a == " " && list2.Count == 0)
                {
                    string item = commandLine.Substring(num, i - num);
                    list.Add(item);
                    num = i + 1;
                }
                else if (a == "\"")
                {
                    if (list2.Count == 0)
                    {
                        list2.Add(true);
                    }
                    else if (!list2[index])
                    {
                        list2.Add(true);
                    }
                    else
                    {
                        list2.RemoveAt(index);
                    }
                }
                else if (a == "<")
                {
                    string item2 = commandLine.Substring(num, i - num);
                    list.Add(item2);
                    num = i + 1;
                    list.Add("<");
                    list2.Add(false);
                }
                else if (a == ">")
                {
                    if (!list2[index])
                    {
                        list2.RemoveAt(index);
                    }
                    string text = commandLine.Substring(num, i - num);
                    List<string> list3 = this.SplitString(ref text);
                    int count = list3.Count;
                    for (int j = 0; j < count; j++)
                    {
                        list.Add(list3[j]);
                    }
                    list.Add("<");
                    int count2 = list.Count;
                    int num2 = 0;
                    while (num2 < count2 && list[num2] != "<")
                    {
                        string item3 = list[num2];
                        list.Add(item3);
                        num2++;
                    }
                    list.RemoveAt(list.Count - 1);
                    num = i + 1;
                }
            }
            if (num < length)
            {
                string item4 = commandLine.Substring(num, length - num);
                list.Add(item4);
            }
            int count3 = list.Count;
            for (int k = 0; k < count3; k++)
            {
                if (list[k] == "print" || list[k] == "double")
                {
                    string text2 = list[k + 2];
                    if (text2.Length == 0 || (text2.Length == 1 && text2 == "\""))
                    {
                        list.RemoveRange(k, 3);
                        if (list.Count > k && list[k] == "<")
                        {
                            list.RemoveAt(k);
                        }
                        count3 = list.Count;
                    }
                }
                else
                {
                    string text3 = this._headOnly.Replace(list[k], "$1");
                    text3 = this._tailOnly.Replace(text3, "$1");
                    list[k] = text3;
                }
            }
            list.Remove(string.Empty);
            return list;
        }

        private int GetCommandID(ref string command)
        {
            return this._config.GetCommandID(ref command);
        }

        private string GetCommandName(int id)
        {
            return this._config.GetCommandName(id);
        }

        private CommandCategory GetCommandCategory(int id)
        {
            return this._config.GetCommandCategory(id);
        }

        private byte[] ConvertByteCommand(string command)
        {
            int commandID = this.GetCommandID(ref command);
            byte[] bytes = BitConverter.GetBytes(commandID);
            Array.Resize<byte>(ref bytes, 2);
            Array.Reverse(bytes);
            return bytes;
        }

        private byte[] ConvertByteArgs(string args)
        {
            Encoding uTF = Encoding.UTF8;
            byte[] bytes = uTF.GetBytes(args);
            string s = Convert.ToBase64String(bytes);
            bytes = uTF.GetBytes(s);
            this.BitInverse(ref bytes);
            return bytes;
        }

        private string ConvertStringArgs(byte[] byteArgs)
        {
            this.BitInverse(ref byteArgs);
            string @string = Encoding.UTF8.GetString(byteArgs);
            byte[] bytes = Convert.FromBase64String(@string);
            return Encoding.UTF8.GetString(bytes);
        }

        private void BitInverse(ref byte[] byteList)
        {
            int num = byteList.Length;
            for (int i = 0; i < num; i++)
            {
                if (i % 3 == 0)
                {
                    byteList[i] = BitConverter.GetBytes((int)(~(int)byteList[i]))[0];
                }
            }
        }
    }
}
