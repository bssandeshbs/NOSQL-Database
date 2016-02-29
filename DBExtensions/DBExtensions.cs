///////////////////////////////////////////////////////////////////
// DBExtensions.cs - define extension methods for Display        //
// Ver 1.3                                                       //
// Application: NoSQL Key/Value Database CSE-681, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015                  //
// Platform:    ASUS TP300L, Core-i5, Windows 8                  //
// Author:      Sandesh Shashidhara, Syracuse University         //
//              (315) 751-4826, sbellurs@syr.edu                 //
//  Original Author:Jim Fawcett,  Syracuse University            //
//              (315) 443-3948, jfawcett@twcny.rr.com            //
///////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements extensions methods to support 
 * displaying DBElements and DBEngine instances.
 *
 * Public Interface:
 * =================
 *   DBElementExtension ---
 *
 * public static generate_int_key() 
 *   - generates a int key
 *  public static string generate_string_key()
 *   - generates a string key
 *  public static string showMetaData<Key, Data>(this DBElement<Key, Data> elem)
 *  - show metadata of DB element
 *  public static string showElement<Key, Data>(this DBElement<Key, Data> elem)
 * - write details of element with simple Data to string
 *  public static string showElement<Key, Data, T>(this DBElement<Key, Data> elem)
 *         where Data : IEnumerable<T> 
 * -write details of element with enumerable Data to string 
 *
 * DBEngineExtensions ----
 *  public static void show<Key, Value, Data>(this DBEngine<Key, Value> db)
 *     -write simple db elements to console 
 *  public static void showkeys<Key>(this List<Key> keys)
 *  - write keys to console
 *  public static void show<Key, Value, Data, T>(this DBEngine<Key, Value> db)
 *     where Data : IEnumerable<T>
 *  - write enumerable db elements to console
 *    public static bool IsBetween(this DateTime time, DateTime startTime, DateTime endTime)
 *  - utility function to check if time is between two intervals    
 *
 * Maintenance:
 * ------------
 * Required Files: 
 *   DBExtensions.cs, DBEngine.cs, DBElement.cs, UtilityExtensions
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.3 : 5  Oct 15
 * - added new methods
 * ver 1.2 : 24 Sep 15
 * - reduced the number of methods and simplified
 * ver 1.1 : 15 Sep 15
 * - added a few comments
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

namespace NoSQLDB
{
    /////////////////////////////////////////////////////////////////////////
    // Extension methods class 
    // - Extension methods are static methods of a static class
    //   that extend an existing class by adding functionality
    //   not part of the original class.
    // - These methods are all extending the DBElement<Key, Data> class.
    //
    public static class DBElementExtensions
    {
        //----< generate int key >-------------------------------------
        private static int key = 112;
        public static int generate_int_key()
        {
            Func<int> keyGen = () => { ++key; return key; };  // anonymous function to generate keys
            return keyGen();
        }

        //----< generate string key >-------------------------------------
        public static string generate_string_key()
        {
            string skey = key.ToString();
            Func<string> skeyGen = () =>
            {
                ++key;
                skey = "string" + key.ToString();
                skey = skey.GetHashCode().ToString();
                return skey;
            };
            return skeyGen();
        }

        //----< show metadata of DBElement>----------
        public static string showMetaData<Key, Data>(this DBElement<Key, Data> elem)
        {
            StringBuilder accum = new StringBuilder();
            accum.Append(String.Format("\n  name: {0}", elem.name));
            accum.Append(String.Format("\n  desc: {0}", elem.descr));
            accum.Append(String.Format("\n  time: {0}", elem.timeStamp));
            if (elem.children.Count() > 0)
            {
                accum.Append(String.Format("\n  Children: "));
                bool first = true;
                foreach (Key key in elem.children)
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
        //----< write details of element with simple Data to string >----------

        public static string showElement<Key, Data>(this DBElement<Key, Data> elem)
        {
            StringBuilder accum = new StringBuilder();
            accum.Append(elem.showMetaData());
            if (elem.payload != null)
            {
                accum.Append(String.Format("\n  payload: {0}", elem.payload.ToString()));
            }
            return accum.ToString();
        }
        //----< write details of element with enumerable Data to string >------

        public static string showElement<Key, Data, T>(this DBElement<Key, Data> elem)
          where Data : IEnumerable<T>  // constraint clause
        {
            StringBuilder accum = new StringBuilder();
            accum.Append(elem.showMetaData());
            if (elem.payload != null)
            {
                IEnumerable<object> d = elem.payload as IEnumerable<object>;
                if (d == null)
                    accum.Append(String.Format("\n  payload: {0}", elem.payload.ToString()));
                else
                {
                    bool first = true;
                    accum.Append(String.Format("\n  payload:\n  "));
                    foreach (var item in elem.payload)  // won't compile without constraint clause
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
            }
            return accum.ToString();
        }
    }
    public static class DBEngineExtensions
    {
        //----< write simple db elements out to Console >------------------
        public static void show<Key, Value, Data>(this DBEngine<Key, Value> db)
        {
            foreach (Key key in db.Keys())
            {
                Value value;
                db.getValue(key, out value);
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;
                Write("\n\n  -- key = {0} --", key);
                Write(elem.showElement());
            }
        }
        //----< write keys to Console >------------------
        public static void showkeys<Key>(this List<Key> keys)
        {
            bool first = true;
            Write("\n  Childrens of the key : ");
            foreach (var item in keys)
            {
                if (first)
                {
                    Write(String.Format("{0}", item));
                    first = false;
                }
                else
                {
                    Write(String.Format(", {0}", item));
                }
            }

            if (first) Write("  \n  No Children for the key\n");
        }

        //----< write enumerable db elements out to Console >--------------
        public static void show<Key, Value, Data, T>(this DBEngine<Key, Value> db)
      where Data : IEnumerable<T>
        {
            foreach (Key key in db.Keys())
            {
                Value value;
                db.getValue(key, out value);
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;
                Write("\n\n  -- key = {0} --", key);
                Write(elem.showElement<Key, Data, T>());
            }
        }

        //----< utility function to check if time is between two Datetime intervals >------------------
        public static bool IsBetween(this DateTime time, DateTime startTime, DateTime endTime)
        {
            if (time.TimeOfDay == startTime.TimeOfDay) return true;
            if (time.TimeOfDay == endTime.TimeOfDay) return true;

            if (startTime.TimeOfDay <= endTime.TimeOfDay)
                return (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay);
            else
                return !(time.TimeOfDay >= endTime.TimeOfDay && time.TimeOfDay <= startTime.TimeOfDay);
        }


    }

#if (TEST_DBEXTENSIONS)

    class TestDBExtensions
    {
        static void Main(string[] args)
        {
            "Testing DBExtensions Package".title('=');
            WriteLine();

            Write("\n --- Test DBElement<int,string> ---");
            DBElement<int, string> elem1 = new DBElement<int, string>();
            elem1.payload = "a payload";
            Write(elem1.showElement<int, string>());

            DBEngine<int, DBElement<int, string>> dbs = new DBEngine<int, DBElement<int, string>>();
            dbs.insert(1, elem1);
            dbs.show<int, DBElement<int, string>, string>();
            WriteLine();

            Write("\n --- Test DBElement<string,List<string>> ---");
            DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
            newelem1.name = "newelem1";
            newelem1.descr = "test new type";
            newelem1.children = new List<string> { "Key1", "Key2" };
            newelem1.payload = new List<string> { "one", "two", "three" };
            Write(newelem1.showElement<string, List<string>, string>());

            DBEngine<string, DBElement<string, List<string>>> dbe = new DBEngine<string, DBElement<string, List<string>>>();
            dbe.insert("key1", newelem1);
            dbe.show<string, DBElement<string, List<string>>, List<string>, string>();

            Write("\n\n");
        }
    }
#endif
}
