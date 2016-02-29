///////////////////////////////////////////////////////////////
// DBEngine.cs - define noSQL database                       //
// Ver 1.1                                                   //
// Application: Demonstration for CSE687-OOD, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015              //
// Platform:    Dell XPS2700, Core-i7, Windows 10            //
// Author:      Jim Fawcett, CST 4-187, Syracuse University  //
//              (315) 443-3948, jfawcett@twcny.rr.com        //
///////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package implements DBEngine<Key, Value> where Value
 * is the DBElement<key, Data> type.
 *
 * This class is a starter for the DBEngine package you need to create.
 * It doesn't implement many of the requirements for the db, e.g.,
 * It doesn't remove elements, doesn't persist to XML, doesn't retrieve
 * elements from an XML file, and it doesn't provide hook methods
 * for scheduled persistance.
 */
/*
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs, and
 *                 UtilityExtensions.cs only if you enable the test stub
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.1 : 15 Sep 15
 * - fixed a casting bug in one of the extension methods
 * ver 1.0 : 08 Sep 15
 * - first release
 *
 */
 //todo add placeholders for DBEdit and Shard
 //todo add reference to class text XML content

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Project2Starter
{
  public class DBEngine<Key, Value>
  {
    private Dictionary<Key, Value> dbStore;
    public DBEngine()
    {
      dbStore = new Dictionary<Key, Value>();
    }
    public bool insert(Key key, Value val)
    {
      if (dbStore.Keys.Contains(key))
        return false;
      dbStore[key] = val;
      return true;
    }
    public bool getValue(Key key, out Value val)
    {
      if(dbStore.Keys.Contains(key))
      {
        val = dbStore[key];
        return true;
      }
      val = default(Value);
      return false;
    }
    public IEnumerable<Key> Keys()
    {
      return dbStore.Keys;
    }
    /*
     * More functions to implement here
     */
  }

#if(TEST_DBENGINE)

  /////////////////////////////////////////////////////////////////////
  // Local Extension methods class 
  // - Extension methods are static methods of a static class
  //   that extend an existing class by adding functionality
  //   not part of the original class.
  // - These methods are all extending the DBEngine<Key, Value> class
  //   and used just in this test.  Look at DBExtensions for more
  //   flexible methods.
  //
  public static class DBEngineExtensions
  {
    public static void showWithTestType1<Key>(this DBEngine<Key, DBElement<Key, string>> db)
    {
      foreach (Key key in db.Keys())
      {
        DBElement<Key, string> elem;
        db.getValue(key, out elem);
        Write("\n\n  -- key = {0} --", key.ToString());
        Write(elem.showElementWithTestType1<Key>());
      }
    }
    public static void showWithTestType2<Key>(this DBEngine<Key, DBElement<Key, List<string>>> db)
    {
      foreach (Key key in db.Keys())
      {
        DBElement<Key, List<string>> elem;
        db.getValue(key, out elem);
        Write("\n\n  -- key = {0} --", key.ToString());
        Write(elem.showElementWithTestType2<Key>());
      }
    }
  }

  class TestDBEngine
  {
    static void Main(string[] args)
    {
      "Testing DBEngine Package".title('=');
      WriteLine();

      Write("\n --- Test DBElement<int,string> ---");
      DBElement<int, string> elem1 = new DBElement<int, string>();
      elem1.payload = "a payload";
      Write(elem1.showElementWithTestType1<int>());
      WriteLine();

      DBElement<int, string> elem2 = new DBElement<int, string>("Darth Vader", "Evil Overlord");
      elem2.payload = "The Empire strikes back!";
      Write(elem2.showElementWithTestType1<int>());
      WriteLine();

      var elem3 = new DBElement<int, string>("Luke Skywalker", "Young HotShot");
      elem3.payload = "X-Wing fighter in swamp - Oh oh!";
      Write(elem3.showElementWithTestType1<int>());
      WriteLine();

      Write("\n --- Test DBEngine<int,DBElement<int,string>> ---");

      int key = 0;
      Func<int> keyGen = () => { ++key; return key; };  // anonymous function to generate keys

      DBEngine<int, DBElement<int, string>> db = new DBEngine<int, DBElement<int, string>>();
      bool p1 = db.insert(keyGen(), elem1);
      bool p2 = db.insert(keyGen(), elem2);
      bool p3 = db.insert(keyGen(), elem3);
      if (p1 && p2 && p3)
        Write("\n  all inserts succeeded");
      else
        Write("\n  at least one insert failed");
      db.showWithTestType1<int>();
      WriteLine();

      Write("\n --- Test DBElement<string,List<string>> ---");
      DBElement<string, List<string>> newelem1 = new DBElement<string, List<string>>();
      newelem1.name = "newelem1";
      newelem1.descr = "test new type";
      newelem1.payload = new List<string> { "one", "two", "three" };
      Write(newelem1.showElementWithTestType2<string>());
      WriteLine();

      Write("\n --- Test DBElement<string,IEnumWrapper<string>> ---");
      DBElement<string, List<string>> newerelem1 = new DBElement<string, List<string>>();
      newerelem1.name = "newerelem1";
      newerelem1.descr = "better formatting";
      newerelem1.payload = new List<string> { "alpha", "beta", "gamma" };
      newerelem1.payload.Add("delta");
      newerelem1.payload.Add("epsilon");
      Write(newerelem1.showElementWithTestType2<string>());
      WriteLine();

      DBElement<string, List<string>> newerelem2 = new DBElement<string, List<string>>();
      newerelem2.name = "newerelem2";
      newerelem2.descr = "better formatting";
      newerelem1.children.AddRange(new[] { "first", "second" });
      newerelem2.payload = new List<string> { "a", "b", "c" };
      newerelem2.payload.Add("d");
      newerelem2.payload.Add("e");
      Write(newerelem2.showElementWithTestType2<string>());
      WriteLine();

      Write("\n --- Test DBEngine<string,DBElement<string,IEnumWrapper<string>>> ---");

      int seed = 0;
      string skey = seed.ToString();
      Func<string> skeyGen = () => {
        ++seed;
        skey = "string" + seed.ToString();
        skey = skey.GetHashCode().ToString();
        return skey;
      };

      DBEngine<string, DBElement<string, List<string>>> newdb = 
        new DBEngine<string, DBElement<string, List<string>>>();
      newdb.insert(skeyGen(), newerelem1);
      newdb.insert(skeyGen(), newerelem2);
      newdb.showWithTestType2<string>();
      Write("\n\n");
    }
  }
#endif
}
