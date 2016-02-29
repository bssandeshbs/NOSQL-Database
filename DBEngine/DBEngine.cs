///////////////////////////////////////////////////////////////////
// DBEngine.cs - define noSQL database                           //
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
 * This package implements DBEngine<Key, Value> where Value
 * is the DBElement<key, Data> type.
 *
 * This class provides the functionality to create, update, delete 
 * getValue, size and keys from the Dictionary
 *
 * Public Interface:
 * =================
 * Constructor DBEngine() 
 *  - Creates a new Dictioanry object
 * public int size() 
 *  - Returns the size of the dbEngine
 * public bool insert(Key key, Value val)
 *   - Insert a key to db engine
 * public bool update(Key key, Value val) 
 *   - update the value object 
 * public bool delete(Key key)
 *   - delete the key from db engine
 * public bool getValue(Key key, out Value val)
 *  - get value object for a specified key 
 * public IEnumerable<Key> Keys()
 *  - get keys for dbEngine
 *
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
 * ver 1.3 : 4th Oct 15
 * - added update and remove methods 
 * ver 1.2 : 24 Sep 15
 * - removed extensions methods and tests in test stub
 * - testing is now done in DBEngineTest.cs to avoid circular references
 * ver 1.1 : 15 Sep 15
 * - fixed a casting bug in one of the extension methods
 * ver 1.0 : 08 Sep 15
 * - first release
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace NoSQLDB
{
    public class DBEngine<Key, Value>
    {
        private Dictionary<Key, Value> dbStore;
        public int numberOfWrties { get; set; } = 0;
        public int max_writes { get; set; } = 5;

        //----< constructor>-------------------
        public DBEngine()
        {
            dbStore = new Dictionary<Key, Value>();
        }

        //----< return size of db engine>-------------------
        public int size()
        {
            return dbStore.Count;
        }

        //----< Insert key/value to db engine>-------------------
        public bool insert(Key key, Value val)
        {
            if (dbStore.Keys.Contains(key))
                return false;
            dbStore[key] = val;
            return true;
        }

        //----< update value object for a key>-------------------
        public bool update(Key key, Value val)
        {
            if (dbStore.Keys.Contains(key))
            {
                dbStore[key] = val;
                return true;
            }
            else
            {
                return false;
            }
        }

        //----< Delete a key/value pair>-------------------
        public bool delete(Key key)
        {
            return dbStore.Remove(key);
        }

        //----< get value for a key>-------------------
        public bool getValue(Key key, out Value val)
        {
            if (dbStore.Keys.Contains(key))
            {
                val = dbStore[key];
                return true;
            }
            val = default(Value);
            return false;
        }
        //----< Returns list of keys>-------------------
        public IEnumerable<Key> Keys()
        {
            return dbStore.Keys;
        }
    }

#if (TEST_DBENGINE)

    class TestDBEngine
    {
        static void Main(string[] args)
        {
            "Testing DBEngine Package".title('=');
            WriteLine();

            Write("\n  All testing of DBEngine class moved to DBEngineTest package.");
            Write("\n  This allow use of DBExtensions package without circular dependencies.");

            Write("\n\n");
        }
    }
#endif
}
