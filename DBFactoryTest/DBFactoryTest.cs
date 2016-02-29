///////////////////////////////////////////////////////////////////
// DBFactoryTest.cs - Test Stub for DBFactory                    //
// Ver 1.0                                                       //
// Application: NoSQL Key/Value Database CSE-681, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015                  //
// Platform:    ASUS TP300L, Core-i5, Windows 8                  //
// Author:      Sandesh Shashidhara, Syracuse University         //
//              (315) 751-4826, sbellurs@syr.edu                 //
///////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package replaces DBFactory test stub to remove
 * circular package references.
 *
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBEngineTest.cs,  DBElement.cs, DBEngine.cs, DBItemEditor.cs,QueryEngine.cs
 *   DBExtensions.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 05 Oct 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace NoSQLDB
{
    #if (TEST_TESTDBFACTORY)
    class DBFactoryTest
    {
        static void Main(string[] args)
        {
            DBEngine<int, DBElement<int, string>> dbType1 = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbType2 = new DBEngine<string, DBElement<string, List<string>>>();
            DBItemEditor editor = new DBItemEditor();

            //Demonstrating primitive type
            DBElement<int, string> elem1 = new DBElement<int, string>();
            elem1.name = "Jurassic World";
            elem1.descr = "Story on escape from giant creatures";
            elem1.timeStamp = DateTime.Now;
            elem1.payload = "A giant creature attacks the park and becomes a killing machine";
            editor.addKeyValyePair<int, String>(dbType1, elem1, DBElementExtensions.generate_int_key());

            DBElement<int, string> elem2 = new DBElement<int, string>();
            elem2.name = "Cast Away";
            elem2.descr = "Story of surviving a crash landing on a deserted island.";
            elem2.timeStamp = DateTime.Now;
            elem2.children.AddRange(new List<int> { 4, 5 });
            elem2.payload = "Directed by Robert Zemeckis and written by Willian Broyles Jr.";
            editor.addKeyValyePair<int, String>(dbType1, elem2, DBElementExtensions.generate_int_key());
            dbType1.showDB();

            QueryEngine<int, string> queryEnginePrimitive = new QueryEngine<int, string>(dbType1);
         
            string inputString = "11";
            Write("\n\n \n Input Search String :" + inputString);
            List<int> resultList = queryEnginePrimitive.searchKeyPattern(inputString);
            foreach (int key in resultList)
                Write("\n  found \"{0}\" in key \"{1}\"", inputString, key);

            string inputString2 = "Movie";
            Write("\n\n  Input Search String :" + inputString);
            List<int> resultList2 = queryEnginePrimitive.searchMetadataPattern(inputString2);
            foreach (int key in resultList2)
                Write("\n  found \"{0}\" in \"{1}\"", inputString, key);

            DateTime startDate = new DateTime(2014, DateTime.Today.Month, DateTime.Today.Day, 00, 00, 01);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            List<int> resultList3 = queryEnginePrimitive.searchTimeStamp(startDate);
            foreach (int key in resultList3)
                Write("\n  found key within \"{0}\" within range \"{1}\" {2}", key, startDate, endDate);

        }
    }
    #endif
}
