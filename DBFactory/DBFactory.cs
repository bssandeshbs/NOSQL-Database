///////////////////////////////////////////////////////////////////
// DBFactory.cs - Creates Immutable Database                     //
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
 * This package provides the ability to create a immutable database  
 *
 * Public Interface:
 * =================
 * Constructor - DBFactory(DBEngine<Key, DBElement<Key, Data>> dbEngine)
 *  - Initializes the dbfactory
 * public void addKey(Key key) 
 *   - Add a new key to the dbFactory List
 * public List<Key> Keys()
 *   - Returns set of keys stored in Immutable DB
 * public bool getValue(Key key, out DBElement<Key, Data> val)
 *   - Returns value associated with the key
 *
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs,DBExtensions and  UtilityExtensions.cs 
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : Initial Version 
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSQLDB
{
    public class DBFactory<Key, Data>
    {
        //Stores the keys in Immutable DB
        private List<Key> keys = new List<Key>();
       
        private DBEngine<Key, DBElement<Key, Data>> db;
        //----< Constructor>-------------------
        public DBFactory(DBEngine<Key, DBElement<Key, Data>> dbEngine)
        {
            db = dbEngine;
        }

        //----< Add a new key to Immutable DB>-------------------
        public void addKey(Key key)
        {
            keys.Add(key); 
        }

        //----< Return list of keys from the Immutable DB>-------------------
        public List<Key> Keys()
        {
            return keys;
        }

        //----< get value associated with the key>-------------------
        public bool getValue(Key key, out DBElement<Key, Data> val)
        {
           bool res = db.getValue(key, out val);
            return res;
        }

    }

#if (TEST_DBFACTORY)
    class DBFactoryTest
    {
        static void Main(string[] args)
        {
            "Testing DBFactory Package".title('=');
            Console.WriteLine();

            Console.Write("\n  All testing of DBFactory class moved to DBFactoryTest package.");
            Console.Write("\n  This allow use of DBFactory package without circular dependencies.");

            Console.Write("\n\n");

        }
    }
#endif
}
