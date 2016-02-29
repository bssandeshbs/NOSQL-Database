using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Project2Starter
{
  class TestExec
  {
    private DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();

    void TestR2()
    {
      "Demonstrating Requirement #2".title();
      DBElement<int, string> elem = new DBElement<int, string>();
      elem.name = "element";
      elem.descr = "test element";
      elem.timeStamp = DateTime.Now;
      elem.children.AddRange(new List<int>{ 1, 2, 3 });
      elem.payload = "elem's payload";
      elem.showElement();
      db.insert(1, elem);
      db.showDB();
      WriteLine();
    }
    void TestR3()
    {
      "Demonstrating Requirement #3".title();
      WriteLine();
    }
    void TestR4()
    {
      "Demonstrating Requirement #4".title();
      WriteLine();
    }
    void TestR5()
    {
      "Demonstrating Requirement #5".title();
      WriteLine();
    }
    void TestR6()
    {
      "Demonstrating Requirement #6".title();
      WriteLine();
    }
    void TestR7()
    {
      "Demonstrating Requirement #7".title();
      WriteLine();
    }
    void TestR8()
    {
      "Demonstrating Requirement #8".title();
      WriteLine();
    }
    static void Main(string[] args)
    {
      TestExec exec = new TestExec();
      "Demonstrating Project#2 Requirements".title('=');
      WriteLine();
      exec.TestR2();
      exec.TestR3();
      exec.TestR4();
      exec.TestR5();
      exec.TestR6();
      exec.TestR7();
      exec.TestR8();
      Write("\n\n");
    }
  }
}
