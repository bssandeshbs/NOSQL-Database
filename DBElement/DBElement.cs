﻿///////////////////////////////////////////////////////////////
// DBElement.cs - Define element for noSQL database          //
// Ver 1.0                                                   //
// Application: Demonstration for CSE687-OOD, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell XPS2700, Core-i7, Windows 10            //
// Author:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements the DBElement<Key, Data> type, used by 
 * DBEngine<key, Value> where Value is DBElement<Key, Data>.
 *
 * The DBElement<Key, Data> state consists of metadata and an
 * instance of the Data type.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBElement.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 13 Sep 15
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Project2Starter
{
    /////////////////////////////////////////////////////////////////////
    // DBElement<Key, Data> class
    // - Instances of this class are the "values" in our key/value 
    //   noSQL database.
    // - Key and Data are unspecified classes, to be supplied by the
    //   application that uses the noSQL database.
    //   See the teststub below for examples of use.


    public class DBElement<Key, Data>
    {
        public string name { get; set; }          // metadata    |
        public string descr { get; set; }         // metadata    |
        public DateTime timeStamp { get; set; }   // metadata   value
        public List<Key> children { get; set; }   // metadata    |
        public Data payload { get; set; }         // data        |

        public DBElement(string Name = "unnamed", string Descr = "undescribed")
        {
            name = Name;
            descr = Descr;
            timeStamp = DateTime.Now;
            children = new List<Key>();
        }
    }

#if (TEST_DBELEMENT)

    /////////////////////////////////////////////////////////////////////
    // Extension methods class 
    // - Extension methods are static methods of a static class
    //   that extend an existing class by adding functionality
    //   not part of the original class.
    // - These methods are all extending the DBElement<Key, Data> class
    //   and used just in this test.  Look at DBExtensions for more
    //   flexible methods.
    //
    public static class LocalExtensions
    {
        public static string showElemMetaData<Key, Data>(this DBElement<Key, Data> elem)
        {
            StringBuilder accum = new StringBuilder();
            accum.Append(String.Format("\n  name: {0}", elem.name));
            accum.Append(String.Format("\n  desc: {0}", elem.descr));
            accum.Append(String.Format("\n  desc: {0}", elem.timeStamp));
            if (elem.children.Count() > 0)
            {
                accum.Append(String.Format("\n  Children: "));
                bool first = true;
                foreach (var key in elem.children)
                {
                    if (first)
                    {
                        accum.Append(String.Format("{0}", key.ToString()));
                        first = false;
                    }
                    else
                        accum.Append(String.Format(", {0}", key.ToString()));
                }
            }
            return accum.ToString();
        }
        public static string showElementWithTestType1<Key>(this DBElement<Key, string> elem)
        {
            StringBuilder accum = new StringBuilder();
            accum.Append(elem.showElemMetaData<Key, string>());
            if (elem.payload != null)
                accum.Append(String.Format("\n  payload: {0}", elem.payload.ToString()));
            return accum.ToString();
        }
        public static string showElementWithTestType2<Key>(this DBElement<Key, List<string>> elem)
        {
            StringBuilder accum = new StringBuilder();
            accum.Append(elem.showElemMetaData<Key, List<string>>());
            if (elem.payload != null)
            {
                bool first = true;
                accum.Append(String.Format("\n  payload:\n  "));
                foreach (var item in elem.payload)
                {
                    if (first)
                    {
                        accum.Append(String.Format("{0}", item));
                        first = false;
                    }
                    else
                        accum.Append(String.Format(", {0}", item));
                }
            }
            return accum.ToString();
        }
    }
    ///////////////////////////////////////////////////////////////////////
    // TestDBElement class
    // - Demonstrates creation and use of DBElement<int, string> and of
    //   DBElement<string, List<string>>.

    class TestDBElement
    {
        static void Main(string[] args)
        {
            WriteLine();

            Write("\n --- Test TestType1 = DBElement<int,string> ---");
            Write("\n  -- use showElementWithTestType1<int>() --");

            DBElement<int, string> elem1 = new DBElement<int, string>();
            Write(elem1.showElementWithTestType1<int>());
            WriteLine();

            DBElement<int, string> elem2 = new DBElement<int, string>("Darth Vader", "Evil Overlord");
            elem2.payload = "The Empire strikes back!";
            Write(elem2.showElementWithTestType1<int>());
            WriteLine();

            var elem3 = new DBElement<int, string>("Luke Skywalker", "Young HotShot");
            elem3.children = new List<int> { 1, 2, 7 };
            elem3.payload = "X-Wing fighter in swamp - Oh oh!";
            Write(elem3.showElementWithTestType1<int>());
            WriteLine();

            Write("\n --- Test TestType2 = DBElement<string,List<string>> ---");
            Write("\n  -- use showElementWithTestType2<string>() --");

            DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
            newelem1.name = "newelem1";
            newelem1.descr = "test new type";
            newelem1.payload = new List<string> { "one", "two", "three" };
            Write(newelem1.showElementWithTestType2<string>());
            WriteLine();

            DBElement<string, List<string>> newerelem1 = new DBElement<string, List<string>>();
            newerelem1.name = "newerelem1";
            newerelem1.descr = "same stuff";
            newerelem1.children.Add("first_key");
            newerelem1.children.Add("second_key");
            newerelem1.payload = new List<string> { "alpha", "beta", "gamma" };
            newerelem1.payload.AddRange(new[] { "delta", "epsilon" });
            Write(newerelem1.showElementWithTestType2<string>());
            WriteLine();

            Write("\n\n");
        }
    }
#endif
}
