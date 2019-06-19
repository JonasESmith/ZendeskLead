// https://developers.getbase.com/docs/rest/reference/leads
// above is a good reference to the sell api Thta I will need to use for sure! :) 

using System;
using System.Reflection;
using System.Diagnostics;

namespace createLead
{
  class Program
  {
    static void Main(string[] args)
    {
       Process terminal = new Process();

      terminal.StartInfo.UseShellExecute = false;
      terminal.StartInfo.RedirectStandardOutput = true;

      string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

      terminal.StartInfo.FileName = path + "\\getLeads.bat"; // "\\getDeals.bat"
      terminal.Start();

      string output = terminal.StandardOutput.ReadToEnd();
      output = output.Replace("\\", "");
      string[] words = output.Split('/');

      terminal.WaitForExit();

      Console.WriteLine(output);

      Console.ReadLine();
    }
  }
}
