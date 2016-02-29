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
        private DBEngine<int, DBElement<int, String>> db = new DBEngine<int, DBElement<int,String>>();

        void testR2()
        {
            "Demonstrating Requirement #2".title();
            DBElement<int, String> elem = new DBElement<int, string>();
            elem.name = "text element";
            elem.descr = "descr";
            elem.timeStamp = DateTime.Now;
            elem.payload = "elem's payload";
            elem.children.AddRange(new []{ 1, 2, 3 });
            elem.showElement();
            db.insert(1,elem);
            db.showDB();
            WriteLine();
        }

        void testR3()
        {
            "Demonstrating Requirement #3".title();
            WriteLine();
        }
        void testR4()
        {
            "Demonstrating Requirement #4".title();
        }
        void testR5()
        {
            "Demonstrating Requirement #5".title();
        }

        static void Main(string[] args)
        {
            TestExec exec = new TestExec();
            "Demonstrating Requirement #2 Requirements ".title();
            exec.testR2();
            exec.testR3();
            exec.testR4();
            exec.testR5();
            Write("\n\n");
        }
    }
}
