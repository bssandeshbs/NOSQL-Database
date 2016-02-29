///////////////////////////////////////////////////////////////////
// Scheduler.cs - Scheduler to persist db engine contents        //
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
 * This package provides the scheduler functionality to persist the db engine contents 
 * to a xml file based on timestamp or number of writes
 *
 * Public Interface:
 * =================
 * Constructor - Scheduler(DBEngine<Key, Value> dbEngine, int interval)
 *        - Takes DBEngine and the interval to persist the db engine contents to xml
 *
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs,DBExtensions,PersistenceEngine.cs and
 *                 UtilityExtensions.cs 
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
using System.Timers;

namespace NoSQLDB
{
    //----< Scheduler class used to persist contents to db >----------------
    public class Scheduler<Key, Value, Data, T>
         where Data : IEnumerable<T>
    {
        private DBEngine<Key, Value> db;
        private Timer schedular { get; set; } = new Timer();
        private int timeInterval;  // time interval to invoke scheduler
        private int writeCountDB1 = 0;  // counter to run scheduler runs 3 times 

        //----< Constructor>-------------------
        public Scheduler(DBEngine<Key, Value> dbEngine, int interval)
        {
            db = dbEngine;
            timeInterval = interval;
            schedular.Interval = timeInterval;
            schedular.AutoReset = true;
            schedular.Enabled = true;
       
            schedular.Elapsed += (object source, ElapsedEventArgs e) =>
            {
                if (writeCountDB1 <= 2)   // Run scheduler for 3 times
                {
                    string xmlFile1 = "nosqdldb_primitive.xml";
                    string xmlFile2 = "nosqdldb.xml";
                    writeCountDB1++;
                    if (typeof(DBElement<Key, T>).ToString() == typeof(Value).ToString())
                    {
                        Console.Write("\n Scheduled save of NOSQL key/Value database to persistent storage file {0} at time {1} ", xmlFile1, e.SignalTime);
                        db.persist_db<Key, Value, T>(xmlFile1);
                    }
                    else
                    {
                        Console.Write("\n Scheduled save of NOSQL key/Value database to persistent storage file {0} at time {1} ", xmlFile2, e.SignalTime);
                        db.persist_db<Key, Value, Data, T>(xmlFile2);
                    }
                }
                else
                { //Disable scheduler after 3 saves
                schedular.Enabled = false;
                }

        };
        }
}
#if (TEST_SCHEDULER)
    class SchedulerTest
    {
        static void Main(string[] args)
        {
            DBEngine<int, DBElement<int, string>> dbType1 = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbCollectionType = new DBEngine<string, DBElement<string, List<string>>>();
            DBItemEditor editor = new DBItemEditor();
            
            "\nDemonstrating Requirement #2 - Primitive Type DB".title();
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

            int timeInterval = 2000;
            Scheduler<int, DBElement<int, string>, List<string>, string> sch = new Scheduler<int, DBElement<int, string>, List<string>, string>(dbType1, timeInterval);

        }
    }
#endif
}
