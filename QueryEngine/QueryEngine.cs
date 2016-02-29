///////////////////////////////////////////////////////////////////
// QueryEngine.cs - Query Engine to query Dictionary             //
// Ver 1.0                                                       //
// Application: NoSQL Key/Value Database CSE-687, Project#2      //
// Language:    C#, ver 6.0, Visual Studio 2015                  //
// Platform:    ASUS TP300L, Core-i5, Windows 8                  //
// Author:      Sandesh Shashidhara, Syracuse University         //
//              (315) 751-4826, sbellurs@syr.edu                 //
///////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 * This package provides functionalities to perform the queries
 *
 * Public Interface:
 * =================
 * constructor - QueryEngine() 
 *   - sets the db engine
 * public bool getValueForKey(Key key,out DBElement<Key,Data> dbElement)
 *   - get value for key 
 *  public bool getChildren(Key key, out List<Key> children)
 *   - get children for a key
 *  Func<Key, bool> defineQuery(string pattern)
 *   - Delegate to define query
 *  Func<Key, bool> defineMetadataQuery(string pattern)
 *   - Delegate to search metadata name and description
 *  Func<Key, bool> defineTimeStampQuery(DateTime startTime, DateTime endTime)
 *   - Delegate which defines timestamp search
 *  bool processQuery(Func<Key, bool> queryPredicate, DBFactory<Key, Data> dbFactory)
 *    - Delegate to process query
 *  public List<Key> searchKeyPattern(String pattern)
 *     - Function which searches key pattern
 *  public List<Key> searchMetadataPattern(String pattern)
 *     -Function which searches metadata pattern
 *  public List<Key> searchTimeStamp(DateTime startTime,DateTime endTime)
 *   - Function which searches time stamp 
 * public List<Key> searchTimeStamp(DateTime startTime)
 *   - Function which searches timestamp 
 *
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs,DBExtensions,PersistenceEngine.cs and
 *                 UtilityExtensions.cs,DBItemEditor.cs,DBFactory.cs 
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
using static System.Console;

namespace NoSQLDB
{
    public class QueryEngine<Key,Data>
    {
        private DBEngine<Key, DBElement<Key,Data>> db;
        
        //----<Constructor>-------------------
        public QueryEngine(DBEngine<Key, DBElement<Key, Data>> dbEngine)
        {
            db = dbEngine;
        }

        //----<get value for key>-------------------
        public bool getValueForKey(Key key,out DBElement<Key,Data> dbElement)
        {
            DBElement<Key, Data> element;
            bool res  = db.getValue(key, out element);
            dbElement = element;
            return res;
        }

        //----<get children for key>-------------------
        public bool getChildren(Key key, out List<Key> children)
        {
            DBElement<Key, Data> element;
            bool res = db.getValue(key, out element);

            List<Key> l = element.children;
            children = l;
            
            return res;
        }

        //----<Delegate to check key match>-------------------
        Func<Key, bool> defineQuery(string pattern)
        {
            Func<Key, bool> queryPredicate = (Key key) =>
            {
                if (key == null)
                    return false;
                if (key.ToString().StartsWith(pattern))  // string test will be captured by lambda
                    return true;
                return false;
            };
            return queryPredicate;
        }

        //----<Delegate to check metadata match>-------------------
        Func<Key, bool> defineMetadataQuery(string pattern)
        {
            Func<Key, bool> queryPredicate = (Key key) =>
            {
                if (key == null)
                    return false;

                DBElement<Key, Data> value;
                db.getValue(key, out value);
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;

                if (elem.name.Contains(pattern) || elem.descr.Contains(pattern))  // string test will be captured by lambda
                    return true;
                return false;
            };
            return queryPredicate;
        }

        //----<Delegate which checks timedate stamp>-------------------
        Func<Key, bool> defineTimeStampQuery(DateTime startTime, DateTime endTime)
        {
            Func<Key, bool> queryPredicate = (Key key) =>
            {
                if (key == null)
                    return false;

                DBElement<Key, Data> value;
                db.getValue(key, out value);

                DateTime time = value.timeStamp;
                bool res =  time.IsBetween(startTime, endTime);
                return res;
            };
            return queryPredicate;
        }


        //----< process query using queryPredicate >-----------------------
        bool processQuery(Func<Key, bool> queryPredicate, DBFactory<Key, Data> dbFactory)
        { 
            bool res = false;
            foreach(var key in db.Keys())
            {
                if (queryPredicate(key))
                {
                    res = true;
                    dbFactory.addKey(key);
                }
            }
            if (res)
                return true;
            return false;
        }

        //----<Perform search on key>-------------------
        public List<Key> searchKeyPattern(String pattern)
        {
            DBFactory<Key, Data> dbFactory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = defineQuery(pattern);
            bool result = processQuery(query, dbFactory);
            return dbFactory.Keys();
         }

        //----<Perform search on metadata pattern>------------------
        public List<Key> searchMetadataPattern(String pattern)
        {

            DBFactory<Key, Data> dbFactory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = defineMetadataQuery(pattern);
            bool result = processQuery(query, dbFactory);
            return dbFactory.Keys();
         }

        //----<Perform search on timedatestamp with start and end time>-------------------
        public List<Key> searchTimeStamp(DateTime startTime,DateTime endTime)
        {

            DBFactory<Key, Data> dbFactory = new DBFactory<Key,  Data>(db);
            Func<Key, bool> query = defineTimeStampQuery(startTime, endTime);
            bool result = processQuery(query, dbFactory);
            return dbFactory.Keys();
        }

        //----<Perform search on timedatestamp with default endtime>-------------------
        public List<Key> searchTimeStamp(DateTime startTime)
        {

            DBFactory<Key, Data> dbFactory = new DBFactory<Key, Data>(db);
            Func<Key, bool> query = defineTimeStampQuery(startTime, DateTime.Now);
            // process query
            bool result = processQuery(query, dbFactory);
            return dbFactory.Keys();
        }
    }

#if (TEST_DBQUERYENGINE)
    class QueryEngineTest
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
            DBElement<int, string> queryElementPrimitive;
            IEnumerable<int> dbType1keys = dbType1.Keys();
            int lastKeyDB1 = dbType1keys.Last();

            Write("\n\n\n Input Key :" + lastKeyDB1);
            queryEnginePrimitive.getValueForKey(lastKeyDB1, out queryElementPrimitive);
            queryElementPrimitive.showElement();

            List<int> childrenDB1 = new List<int>();
            IEnumerable<int> keys1 = dbType1.Keys();
            int lastKey1 = keys1.Last();
            Write("\n\n\n  Input Key :" + lastKey1);
            StringBuilder accum = new StringBuilder();
            accum.Append(String.Format(" children of key: {0}", lastKey1.ToString()));
            queryEnginePrimitive.getChildren(lastKey1, out childrenDB1);
            childrenDB1.showkeys();
            Write("\n\n\n ");

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
