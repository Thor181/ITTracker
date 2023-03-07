using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ITTracker
{
    internal static class Logging
    {
        internal static void Log(string stringToWrite)
        {
            if (Directory.Exists("Logs") == false)
            {
                Directory.CreateDirectory("Logs");
            }
            if (File.Exists("Logs\\Log.txt") == false)
            {
                File.Create("Logs\\Log.txt");
            }
            string existText = "";
            using (StreamReader reader = new StreamReader("Logs\\Log.txt"))
            {
               existText = reader.ReadToEnd();
            }
            using (StreamWriter writer = new StreamWriter("Logs\\Log.txt"))
            {
                writer.Write(existText);
                writer.WriteLine(stringToWrite);
            }
        }
    }
}
