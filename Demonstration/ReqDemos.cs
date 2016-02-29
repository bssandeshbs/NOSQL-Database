///////////////////////////////////////////////////////////////////
// ReqDemos.cs - Demonstration Package to show requirements      //
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
 * This package contains test methods to demonstrate all the requirements of the project 2
 *
 * Public Interface:
 * =================
 * public void TestR2(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
 *     - Function which demonstrates req 2 - creating a value instance
 * public void TestR3(DBEngine<int, DBElement<int, string>> dbType1,DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
 *      - Function which demonstrates req 3 - addition and deletion of key/value pairs
 * public void TestR3_NonPrimitive(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
 *      - Function which demonstrates req 3 for collection type 
 * public void TestR4(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
 *       - Function which demonstrates req 4 - editing of value instance for primitive type 
 * public void TestR4_NonPrimitive(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
 *       - Function which demomnstrates req 4 for non primitive type
 * public void TestR5(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
 *       - Function  which demonstrates req 5 for augumenting and restoring db    
 *  public void TestR6(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
 *        - Function  which demonstrates scheduler functionality
 * public void TestR7(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
 *       - Function to run queries on db engine to get value and children of a key
 * public void testR7c(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
 *       -Function which calls al the query functions   
 *  public void testR9()
 *     - Function which demonstrates project structure
 *  public void testR12()
 *      -Function which demonstrates categories requirement #12
 * public void testR8()
 *   -Function which demonstrates Immutable database creation requirement
 *
 * Maintenance:
 * ------------
 * Required Files: 
 *   ReqDemos.cs, DBExtensions.cs, DBEngine.cs, DBElement.cs, UtilityExtensions
 *   DBItemEditor.cs, PersistEngine.cs, Scheduler.cs, QueryEngine.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.0 : 1 Oct 15
 *
 */

using System;
using System.Collections.Generic;
using static System.Console;
using System.Linq;
using System.Text;

using System.Xml.Linq;
using System.Xml;

namespace NoSQLDB
{
    public class ReqDemos
    {
        //----< Demonstrating req 2 - creating generic key/value in-memory database>-------------------
        public void TestR2(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
        {
            //Demonstrating primitive type
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
            elem2.children.AddRange(new List<int> { 113, 116 });
            elem2.payload = "Directed by Robert Zemeckis and written by Willian Broyles Jr.";
            editor.addKeyValyePair<int, String>(dbType1, elem2, DBElementExtensions.generate_int_key());
            dbType1.showDB();

            //Demostrating IEnumberable Type 
            "\nDemonstrating Requirement #2 - Collection Type DB".title();
            DBElement<string, List<string>> newerelem1 = new DBElement<string, List<string>>();
            newerelem1.name = "Movie Name - The Good the Bad and the Ugly";
            newerelem1.descr = "A bounty hunting scam joins two men in an uneasy alliance ";
            newerelem1.payload = new List<string> { "Clint Eastwood", " Eli Wallach", "Lee Van Cleef" };
            String key = "The Good, the Bad and the Ugly";
            editor.addKeyValyePair<string, List<String>, string>(dbType2, newerelem1, key);

            DBElement<string, List<string>> newerelem2 = new DBElement<string, List<string>>();
            newerelem2.name = "Movie Name - Django Unchained";
            newerelem2.descr = "With the help of a German hunter, a freed slave sets to rescue";
            newerelem2.children.AddRange(new[] { key, "Life Is Beautiful" });
            newerelem2.payload = new List<string> { "Jamie Foxx", "Christoph Waltz", "Leonardo DiCaprio" };
            newerelem2.payload.Add("Quentin Tarantino");
            String key1 = "Django Unchained";
            editor.addKeyValyePair<string, List<String>, string>(dbType2, newerelem2, key1);
            dbType2.showEnumerableDB();
        }

        //----< Demonstrating req 3 - addition/deletion of key/value database for primitive type db>-------------------
        public void TestR3(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
        {
            "\nDemonstrating Requirement #3 Primitive Type".title();
            int key1 = DBElementExtensions.generate_int_key();
            WriteLine("\n\n Addition of Key/value pair");
            WriteLine(" Before Adding Key : " + key1);
            dbType1.showDB();

            WriteLine("\n\n After adding key :" + key1);
            DBElement<int, string> elem1 = new DBElement<int, string>();
            elem1.name = "Titanic";
            elem1.descr = "A seventeen-year-old aristocrat falls in love with a kind";
            elem1.timeStamp = DateTime.Now;
            elem1.children.AddRange(new List<int> { 114, 116 });
            elem1.payload = "Stars: Leonardo DiCaprio, Kate Winslet, Billy Zane";
            editor.addKeyValyePair<int, String>(dbType1, elem1, key1);
            dbType1.showDB();

            IEnumerable<int> keys1 = dbType1.Keys();
            int first = keys1.First();
            WriteLine("\n\n Removal of Key/value pair");
            WriteLine(" Before removing key :" + first);
            dbType1.showDB();

            WriteLine("\n\n After removing key :" + first);
            editor.removeKey<int, string>(dbType1, first);
            dbType1.showDB();
        }

        //----< Demonstrating req 3 - addition/deletion of key/value database for collection type db>-------------------
        public void TestR3_NonPrimitive(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
        {
            "\nDemonstrating Requirement #3 Collection Type".title();
            WriteLine("\n\n Addition of Key/value pair");
            String movie_name = "3 Idiots";
            WriteLine(" Before Adding Key : " + movie_name);
            dbType2.showEnumerableDB();

            DBElement<string, List<string>> newerelem3 = new DBElement<string, List<string>>();

            newerelem3.name = "Movie Name: 3 Idiots";
            newerelem3.descr = "3 Friends revist the college days and recall memories";
            newerelem3.children.AddRange(new[] { "The Good, the Bad and the Ugly", "Django Unchained" });
            newerelem3.payload = new List<string> { "Aamir Khan", "Madhavan", "Mona Singh" };
            editor.addKeyValyePair<string, List<String>, string>(dbType2, newerelem3, movie_name);
            WriteLine("\n\n After adding key :" + movie_name);
            dbType2.showEnumerableDB();

            IEnumerable<string> keys = dbType2.Keys();
            String first = keys.First();
            WriteLine("\n\n Removal of Key/value pair");
            WriteLine(" Before removing key :" + first);
            dbType2.showEnumerableDB();

            editor.removeKey<string, List<string>, string>(dbType2, first);

            WriteLine("\n\n After removing key :" + first);
            dbType2.showEnumerableDB();
        }

        //----< Demonstrating req 4 - editing metadata,value instance of key/value database primitive type>-------------------
        public void TestR4(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
        {
            "\n\nDemonstrating Requirement #4 Updating Metadata - Primitive Type DB".title();
            IEnumerable<int> keys = dbType1.Keys();
            int firstDB1Key = keys.ElementAt(0);
            int secondDB1Key = keys.ElementAt(1);
            WriteLine("\n\n Before updating Metadata for key : " + firstDB1Key);
            dbType1.showDB();

            WriteLine("\n\n After updating Metadata for key : " + firstDB1Key);
            editor.updateMetadataInfo<int, String>(dbType1, firstDB1Key, "Reborn -Cast Away", "The guy who survived in deserted insland");
            dbType1.showDB();

            "\nDemonstrating Requirement #4 Editing Value Instance Info - Primitive Type DB".title();
            WriteLine("\n\n Before updating Value Instance for key : " + secondDB1Key);
            dbType1.showDB();

            WriteLine("\n\n After updating Value Instance for key : " + secondDB1Key);
            DBElement<int, string> elem2 = new DBElement<int, string>();
            elem2.name = "Titanic Reborn";
            elem2.descr = "A new movie directed in 2015 with the same plot line";
            elem2.timeStamp = DateTime.Now;
            elem2.children.AddRange(new List<int> { 114 });
            elem2.payload = "The movie will feature same actors but director changes";
            editor.updatePayloadInfo<int, String>(dbType1, elem2, secondDB1Key);
            dbType1.showDB();

            "\nDemonstrating Requirement #4 Addition of child instances - Primitive Type DB ".title();
            WriteLine("\n\n Before adding child Instance " + secondDB1Key + " to key " + firstDB1Key);
            dbType1.showDB();
            editor.addChildren<int, string>(dbType1, firstDB1Key, secondDB1Key);
            WriteLine("\n\n After adding child Instance : " + secondDB1Key + " to key " + firstDB1Key);
            dbType1.showDB();

            "\nDemonstrating Requirement #4 Removal of child instances - Primitive DB ".title();
            WriteLine("\n\n Before removing child Instance key " + 113 + " from key " + firstDB1Key);
            dbType1.showDB();
            editor.removeChildren<int, string>(dbType1, firstDB1Key, 113);
            WriteLine("\n\n After removing child Instance key " + 113 + " from key " + firstDB1Key);
            dbType1.showDB();
        }

        //----< Demonstrating req 4 - editing metadata,value instance of key/value collection primitive type>-------------------
        public void TestR4_NonPrimitive(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2, DBItemEditor editor)
        {
            "\nDemonstrating Requirement #4 Updating Metadata - Collection Type DB".title();

            IEnumerable<string> db2Keys = dbType2.Keys();
            String firstDB2Key = db2Keys.ElementAt(0);
            String secondDB2Key = db2Keys.ElementAt(1);
            WriteLine("\n\n Before updating Metadata for key : " + firstDB2Key);
            dbType2.showEnumerableDB();

            WriteLine("\n\n After updating Metadata for key : " + firstDB2Key);
            editor.updateMetadataInfo<string, List<string>, string>(dbType2, firstDB2Key, "Django Unchained Reborn", " German Hunter helps to resuce a slave wife");
            dbType2.showEnumerableDB();

            "\nDemonstrating Requirement #4 Editing Value Instance Info - Collection Type DB".title();
            WriteLine("\n\n Before updating Value Instance for key : " + secondDB2Key);
            dbType2.showEnumerableDB();

            WriteLine("\n\n After updating Value Instance for key : " + secondDB2Key);
            DBElement<string, List<string>> newerelem3 = new DBElement<string, List<string>>();
            newerelem3.name = "3 Idiots Remade";
            newerelem3.descr = " They think differently, even as the rest of the world called them idiots";
            newerelem3.children.AddRange(new[] { "Django Unchained Remake" });
            newerelem3.payload = new List<string> { "Rajkumar Hirani", "Amir Khan", "Abhijat Joshi" };
            editor.updatePayloadInfo<string, List<string>, string>(dbType2, newerelem3, secondDB2Key);
            dbType2.showEnumerableDB();

            "\nDemonstrating Requirement #4 Addition of child instances - Collection DB".title();
            WriteLine("\n\n Before adding child Instance :" + secondDB2Key + " to key : " + firstDB2Key);
            dbType2.showEnumerableDB();
            editor.addChildren<string, List<string>, string>(dbType2, firstDB2Key, secondDB2Key);
            WriteLine("\n\n After adding child Instance :" + secondDB2Key + " to key :" + firstDB2Key);
            dbType2.showEnumerableDB();

            "\nDemonstrating Requirement #4 Removal of child instances - Collection DB".title();
            string keyChild = "Django Unchained Remake";
            WriteLine("\n\n Before removing child Instance :" + keyChild + " from key :" + secondDB2Key);
            dbType2.showEnumerableDB();
            editor.removeChildren<string, List<string>, string>(dbType2, secondDB2Key, "Django Unchained Remake");
            WriteLine("\n\n After removing child Instance :" + keyChild + " from key :" + secondDB2Key);
            dbType2.showEnumerableDB();
        }

        //----< Demonstrating req 5 - Demonstrating persisting, unpersisting and restoring db>-------------------
        public void TestR5(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
        {
            string dir = "..\\..\\..\\..\\input_xml\\";
            string xmlFile1 = "nosqdldb_primitive.xml";
            "\n\nDemonstrating Requirement #5 Primitive DB".title();
            dbType1.persist_db<int, DBElement<int, string>, string>(dir+xmlFile1);
            WriteLine("\nSuccesfully persisted dbengine contents to xml file :" + xmlFile1);

            string xmlFile2 = "nosqdldb.xml";
            "\n\nDemonstrating Requirement #5 - Collection DB".title();
            dbType2.persist_db<string, DBElement<string, List<string>>, List<string>, string>(dir+xmlFile2);
            WriteLine("\nSuccesfully persisted dbengine contents to xml file :  " + xmlFile2);

            string xmlFile3 = "read_nosqldb_primitive.xml";
            "\n\nDemonstrating Requirement #5 - Augumenting DB - Primitive DB".title();
            WriteLine("\n\n Before Augumenting DB from xml file : " + xmlFile3);
            dbType1.showDB();
            dbType1.augument_db<int, DBElement<int, string>, string>(dir+xmlFile3);
            WriteLine("\n\n After Augumenting DB from xml file : " + xmlFile3);
            dbType1.showDB();

            string xmlFile4 = "read_nosqdldb.xml";
            "\n\nDemonstrating Requirement #5 - Augumenting DB - Collection DB".title();
            WriteLine("\n\n Before Augumenting DB from xml file : " + xmlFile4);
            dbType2.showEnumerableDB();
            dbType2.augument_db<string, DBElement<string, List<string>>, List<string>, string>(dir+xmlFile4);
            WriteLine("\n\n After Augumenting DB from xml file : " + xmlFile4);
            dbType2.showEnumerableDB();

            DBEngine<int, DBElement<int, string>> dbType1New = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbType2New = new DBEngine<string, DBElement<string, List<string>>>();

            string xmlFile5 = "read_nosqldb_primitive.xml";
            "\n\nDemonstrating Requirement #5 - Restoring DB - Primitive DB".title();
            WriteLine("\n\n Before Restoring DB from xml file : " + xmlFile5);
            dbType1New.showDB();
            dbType1New.augument_db<int, DBElement<int, string>, string>(dir+xmlFile5);
            WriteLine("\n\n After Restoring DB from xml file : " + xmlFile5);
            dbType1New.showDB();

            string xmlFile6 = "read_nosqdldb.xml";
            "\n\nDemonstrating Requirement #5 - Restoring DB - Collection DB".title();
            WriteLine("\n\n Before Restoring DB from xml file : " + xmlFile6);
            dbType2New.showEnumerableDB();
            dbType2New.augument_db<string, DBElement<string, List<string>>, List<string>, string>(dir+xmlFile6);
            WriteLine("\n\n After Restoring DB from xml file : " + xmlFile6);
            dbType2New.showEnumerableDB();
        }

        //----< Demonstrating req 6 - function which trigger scheduler>-------------------
        public void TestR6(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
        {
            "Demonstrating Requirement #6 - Scheduler Save Process".title();
            int timeInterval = 5000;
            WriteLine("\n Time Interval of primitive db set to : " + timeInterval);
            Scheduler<int, DBElement<int, string>, List<string>, string> sch = new Scheduler<int, DBElement<int, string>, List<string>, string>(dbType1, timeInterval);

            int timeInterval2 = 10000;
            WriteLine("\n Time Interval of collection db set to : " + timeInterval2);
            Scheduler<string, DBElement<string, List<string>>, List<string>, string> sch2 = new Scheduler<string, DBElement<string, List<string>>, List<string>, string>(dbType2, timeInterval2);

        }

        //----< Demonstrating req 7 - Queries - Value, children of a key>-------------------
        public void TestR7(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
        {
            "\n Demonstrating Requirement #7 - Queries - Primitive DB Contents".title();
            dbType1.showDB();
            "\n Demonstrating Requirement #7 - Queries - Collection DB Contents".title();
            dbType2.showEnumerableDB();
            QueryEngine<int, string> queryEnginePrimitive = new QueryEngine<int, string>(dbType1);
            "\n Demonstrating Requirement #7A - The value of a specified key - Primitive DB".title();
            DBElement<int, string> queryElementPrimitive;
            IEnumerable<int> dbType1keys = dbType1.Keys();
            int lastKeyDB1 = dbType1keys.Last();
            Write("\n Input Key :" + lastKeyDB1);
            Write("\n Value of Key \n");
            queryEnginePrimitive.getValueForKey(lastKeyDB1, out queryElementPrimitive);
            queryElementPrimitive.showElement();
            QueryEngine<string, List<string>> queryEngine = new QueryEngine<string, List<string>>(dbType2);
            "\n Demonstrating Requirement #7A - The value of a specified key - Collection DB".title();
            DBElement<string, List<string>> queryElement;
            IEnumerable<string> keys = dbType2.Keys();
            String lastKey = keys.Last();
            Write("\n  Input Key :" + lastKey);
            Write("\n Value of Key \n");
            queryEngine.getValueForKey(lastKey, out queryElement);
            queryElement.showEnumerableElement();
            "\n Demonstrating Requirement #7B - The children of a specified key - Primitive DB".title();
            List<int> childrenDB1 = new List<int>();
            IEnumerable<int> keys1 = dbType1.Keys();
            int firstKey1 = keys1.First();
            Write("\n  Input Key :" + firstKey1);
            StringBuilder accum = new StringBuilder();
            accum.Append(String.Format(" children of key: {0}", firstKey1.ToString()));
            queryEnginePrimitive.getChildren(firstKey1, out childrenDB1);
            childrenDB1.showkeys();
            "\n Demonstrating Requirement #7B- The children of a specified key - Collection DB".title();
            List<String> children;
            IEnumerable<string> keys2 = dbType2.Keys();
            String firstKey2 = keys2.First();
            Write("\n  Input Key :" + firstKey2);
            StringBuilder accum2 = new StringBuilder();
            accum2.Append(String.Format(" children of key: {0}", firstKey2));
            queryEngine.getChildren(firstKey2, out children);
            children.showkeys();
            String lastKey2 = keys2.Last();
            Write("\n\n  Input Key :" + lastKey2);
            StringBuilder accum3 = new StringBuilder();
            accum3.Append(String.Format(" children of key: {0}", lastKey2));
            queryEngine.getChildren(lastKey2, out children);
            children.showkeys();
        }

        //----<Demonstrating queries :keys matching a pattern - primitive >-------------------
        public void testR7c(DBEngine<int, DBElement<int, string>> dbType1, DBEngine<string, DBElement<string, List<string>>> dbType2)
        {
            "\nDemonstrating Requirement #7C - The set of all keys matching a specified pattern  - Primitive DB".title();
            QueryEngine<int, string> queryEngine = new QueryEngine<int, string>(dbType1);
            QueryEngine<string, List<string>> queryEngineType2 = new QueryEngine<string, List<string>>(dbType2);

            string inputString = "11";
            Write("\n\n  Input Search String :" + inputString);
            List<int> resultList = queryEngine.searchKeyPattern(inputString);
            foreach (int key in resultList)
                Write("\n  found \"{0}\" in key \"{1}\"", inputString, key);

            testR7c2(dbType2, queryEngineType2);
            testR7d(dbType2, queryEngineType2);
            testR7d_primitive(dbType1, queryEngine);
            testR7e_primitiveType(dbType1, queryEngine);
            testR7e(dbType2, queryEngineType2);
        }

        //----<Demonstrating queries :keys matching a pattern - collection db >-------------------
        private void testR7c2(DBEngine<string, DBElement<string, List<string>>> dbType2, QueryEngine<string, List<string>> queryEngine)
        {
            "\nDemonstrating Requirement #7C - The set of all keys matching a specified pattern  - Collection DB".title();
            string inputString = "Django";
            Write("\n\n  Input Search String :" + inputString);
            List<String> resultList = queryEngine.searchKeyPattern(inputString);
            foreach (String key in resultList)
                Write("\n  found \"{0}\" in key \"{1}\"", inputString, key);
        }

        //----<Demonstrating queries :search in metadata section - primitive >-------------------
        private void testR7d_primitive(DBEngine<int, DBElement<int, string>> dbType1, QueryEngine<int, string> queryEngine)
        {
            "\n Demonstrating Requirement #7D - All keys that contain a specified string in their metadata section - Primitive DB".title();
            string inputString = "survived";
            Write("\n\n  Input Search String :" + inputString);
            List<int> resultList = queryEngine.searchMetadataPattern(inputString);
            foreach (int key in resultList)
                Write("\n  found \"{0}\" in key \"{1}\"", inputString, key);
        }

        //----<Demonstrating queries :search in metadata section - collection >-------------------
        private void testR7d(DBEngine<string, DBElement<string, List<string>>> dbType2, QueryEngine<string, List<string>> queryEngine)
        {
            "\n Demonstrating Requirement #7 - All keys that contain a specified string in their metadata section - Collection DB".title();
            string inputString = "Hunter";
            Write("\n\n  Input Search String :" + inputString);
            List<String> resultList = queryEngine.searchMetadataPattern(inputString);
            foreach (String key in resultList)
                Write("\n  found \"{0}\" in key \"{1}\"", inputString, key);

        }

        //----<Demonstrating queries :search within timestamp - collection db >-------------------
        private void testR7e(DBEngine<string, DBElement<string, List<string>>> dbType2, QueryEngine<string, List<string>> queryEngine)
        {
            "\n Demonstrating Requirement #7E - keys that contain values written within a specified time-date interval - CollectionDB".title();
            DateTime startDate = new DateTime(2014, DateTime.Today.Month, DateTime.Today.Day, 00, 00, 01);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            List<String> resultList = queryEngine.searchTimeStamp(startDate);
            foreach (String key in resultList)
                Write("\n  found key within \"{0}\" within range \"{1}\" {2}", key, startDate, endDate);
            WriteLine("\n\n");
        }

        //----<Demonstrating queries :search within timestamp - primitive db >-------------------
        private void testR7e_primitiveType(DBEngine<int, DBElement<int, string>> dbType1, QueryEngine<int, string> queryEngine)
        {
            "\n Demonstrating Requirement #7E - keys that contain values written within a specified time-date interval ".title();

            DateTime startDate = new DateTime(2014, DateTime.Today.Month, DateTime.Today.Day, 00, 00, 01);
            DateTime endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59);

            List<int> resultList = queryEngine.searchTimeStamp(startDate);
            foreach (int key in resultList)
                Write("\n  found key within \"{0}\" within range \"{1}\" {2}", key, startDate, endDate);
        }

        //----<Demonstrating req 8 - creating immutable db >-------------------
        public void TestR8()
        {
            "Demonstrating Requirement #8 - Creation of Immutable DB".title();
            WriteLine("\n\n DFFactory object is created for each query and result of the keys are added to a List");
            WriteLine("The DBFactory doesn't provide any functions for editing of value object, Hence It acts as a Immutable database");
            WriteLine("The Functions - AddKey at line 60 and Keys() at line 66 demonstrate immutable database ");
        }

        //----<Demonstrating categories >-------------------
        public void testR12()
        {
            DBEngine<string, DBElement<string, List<string>>> dbType2New = new DBEngine<string, DBElement<string, List<string>>>();
            string dir = "..\\..\\..\\..\\input_xml\\";
            string xmlFile6 = "categories.xml";

            "\n\nDemonstrating Requirement #12 - Categories DB".title();
            WriteLine("\n\n Creating Categories DB from xml file : " + xmlFile6);
            dbType2New.augument_db<string, DBElement<string, List<string>>, List<string>, string>(dir+xmlFile6);
            dbType2New.showEnumerableDB();
            "\n\nQueries on Categories DB".title();

            String inputKey = "Food Products";         
            QueryEngine<string, List<string>> queryEngine = new QueryEngine<string, List<string>>(dbType2New);

            DBElement<string, List<string>> queryElement;
            IEnumerable<string> keys = dbType2New.Keys();
            String first = keys.First();
            queryEngine.getValueForKey(first, out queryElement);
            List<String> values = queryElement.payload;
            Write("\nList of keys for the db items in category \""+ inputKey+ "\" are ");
            foreach (String key in values)
            Write(" {0}, ", key);

            List<String> children;
            String child = values[values.Count - 2];
            queryEngine.getChildren(child, out children);
            Write("\nList of categories to which \""+ values[values.Count - 2]+ "\" belong are ");
            foreach (String key1 in children)
                Write(" {0}, ", key1);
            WriteLine("\n");
        }

        //----<Demonstrating Project Structure - Req 9 >-------------------
        public void testR9()
        {
            string dir = "..\\..\\..\\..\\input_xml\\";
            string xmlFile6 = "project_structure.xml";
            "\n\nDemonstrating Requirement #9 - Project Structure with dependencies".title();
            XDocument newDoc = null;
            try
            {
                newDoc = XDocument.Load(dir + xmlFile6);
                Console.WriteLine("\n\n");
                Console.WriteLine(newDoc.ToString());
            } catch(Exception)
            {
                Console.WriteLine("Failed to read the xml :" + xmlFile6);
            }
        }
    }
#if (TEST_REQDEMOS)
class ReqDemosTest
{
    static void Main(string[] args)
    {
        "Demonstrating Project Requirements".title('=');
            ReqDemos demos = new ReqDemos();
            DBEngine<int, DBElement<int, string>> dbType = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbCollectionType = new DBEngine<string, DBElement<string, List<string>>>();
            DBItemEditor editor = new DBItemEditor();
            
            demos.TestR2(dbType, dbCollectionType, editor);
            demos.TestR3(dbType, dbCollectionType, editor);
            demos.TestR3_NonPrimitive(dbType, dbCollectionType, editor);
            demos.TestR4(dbType, dbCollectionType, editor);
            demos.TestR4_NonPrimitive(dbType, dbCollectionType, editor);
            demos.TestR4_NonPrimitive(dbType, dbCollectionType, editor);
            demos.TestR7(dbType, dbCollectionType);
            demos.TestR5(dbType, dbCollectionType);
            demos.testR7c(dbType,dbCollectionType);
            demos.TestR6(dbType, dbCollectionType);
            demos.TestR8();
            demos.testR9();
            demos.testR12();
        }

}
#endif
}

