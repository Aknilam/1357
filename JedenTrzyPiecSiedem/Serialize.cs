using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JedenTrzyPiecSiedem
{
    public static class Serialize
    {
        public static void Write(List<InfoLine> ils, string writeTo = "cases.txt")
        {
            StreamWriter file = new StreamWriter(writeTo);
            file.WriteLine(1);
            Write(ils, file);

            file.Close();
        }
        public static void Append(List<InfoLine> ils, string writeTo = "cases.txt")
        {
            if (File.Exists(writeTo))
            {
                AppendFile(ils, writeTo);
            } else
            {
                Write(ils, writeTo);
            }
        }
        private static void AppendFile(List<InfoLine> ils, string writeTo)
        {
            StreamWriter file = File.AppendText(writeTo);
            Write(ils, file);
            file.Close();

            string[] arrLine = File.ReadAllLines(writeTo);
            int count;
            if (int.TryParse(arrLine[0], out count))
            {
                arrLine[0] = (count + 1).ToString();
                File.WriteAllLines(writeTo, arrLine);
            }
        }
        private static void Write(List<InfoLine> ils, StreamWriter file)
        {
            file.WriteLine(ils.Count);
            ils.ForEach(il => file.WriteLine(il.Row + " " + il.Col));
        }
    }
}
