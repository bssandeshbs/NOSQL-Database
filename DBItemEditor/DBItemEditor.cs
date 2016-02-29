///////////////////////////////////////////////////////////////////
// DBItemEditor.cs - Provides interface to add/edit/delete db    //
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
 * This package provides the ability to create/delete/edit metadata,
 * value instance and add/delete relationships 
 *
 * Public Interface:
 * =================
 * public bool updateMetadataInfo<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, String name, String description)
 *        where Data : IEnumerable<T>
 *   - Provides functionality to update metadata information for collection type db1
 * public bool updateMetadataInfo<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, String name, String description)
 *   - Provides functionality to update metadata information for primitive type db1   
 * public bool updatePayloadInfo<Key, Data,T>(DBEngine<Key, DBElement<Key, Data>> dbType, DBElement<Key, Data> dbElem, Key keyVal)
 *       where Data : IEnumerable<T>
 *    - Provides functionality to update value instance for collection type db   
 * public bool updatePayloadInfo<Key,Data>(DBEngine<Key, DBElement<Key, Data>> dbType, DBElement<Key, Data> dbElem,Key keyVal)
 *   -  Provides functionality to update value instance for primitive type db 
 * public bool addKeyValyePair<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, DBElement<Key, Data> dbElem, Key keyVal)
 *       where Data : IEnumerable<T>
 *  - Provides functionality to add key/value pair for collection type db   
 * public bool addKeyValyePair<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, DBElement<Key, Data> dbElem, Key keyVal)
 *  - provides functionality to add key/value pair for primitive type db
 * public bool removeKey<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2,Key key)
 *            where Data : IEnumerable<T>
 *   - provides functionality to remove key for collection type db
 * public bool removeKey<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key key)
 *   - provides functionality to remove key for primitive type db   
 * public bool addChildren<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal,Key newChild)
 *          where Data : IEnumerable<T>
 *  - add children for collection type db  
 *  public bool removeChildren<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, Key newChild)
 *          where Data : IEnumerable<T>
 * - remove children for collection type db  
 *  public bool addChildren<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, Key newChild)   
 *  - add children for primitive type db
 *  public bool removeChildren<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, Key newChild)
 *  - remove children for collection type db
 *     
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs,DBExtensions
 *     Display.cs and UtilityExtensions.cs 
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
    public class DBItemEditor
    {
        //----< constructor >-------------------------------------
        public DBItemEditor()
        {

        }
 
        //----< update metadata info for collection type db >-------------------------------------
        public bool updateMetadataInfo<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, String name, String description)
            where Data : IEnumerable<T>
        {
            DBElement<Key, Data> dbElem;
            dbType2.getValue(keyVal, out dbElem);
            bool changed = false;
            if (name != null)
            {
                changed = true;
                dbElem.name = name;
            }
            if (description != null)
            {
                changed = true;
                dbElem.descr = description;
            }
            if (!changed)
            {
                return false;
            } else
            {
                dbType2.numberOfWrties++;
                if(dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_collection_storage<Key, Data,T>(dbType2);
                }
            }
            return dbType2.update(keyVal, dbElem);
        }

        //----< update metadata info for primitive type db >-------------------------------------
        public bool updateMetadataInfo<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, String name, String description)
        {
            DBElement<Key, Data> dbElem;
            dbType2.getValue(keyVal, out dbElem);
            bool changed = false;
            if (name != null)
            {
                changed = true;
                dbElem.name = name;
            }
            if (description != null)
            {
                changed = true;
                dbElem.descr = description;
            }
            if (!changed)
            {
                return false;
            }
            else
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_primitive_storage<Key, Data>(dbType2);
                }
            }
            return dbType2.update(keyVal, dbElem);
        }

        //----< update payload info for collection type db >-------------------------------------
        public bool updatePayloadInfo<Key, Data,T>(DBEngine<Key, DBElement<Key, Data>> dbType, DBElement<Key, Data> dbElem, Key keyVal)
        where Data : IEnumerable<T>
        {
            bool res = dbType.update(keyVal, dbElem);      
            {
                dbType.numberOfWrties++;
                if (dbType.numberOfWrties >= dbType.max_writes)
                {
                    dbType.numberOfWrties = 0;
                    trigger_collection_storage<Key, Data,T>(dbType);
                }
            }
            return res;
        }

        //----< update payload info for primitive type db >-------------------------------------
        public bool updatePayloadInfo<Key,Data>(DBEngine<Key, DBElement<Key, Data>> dbType, DBElement<Key, Data> dbElem,Key keyVal)
        {
            bool res = dbType.update(keyVal, dbElem);
            {
                dbType.numberOfWrties++;
                if (dbType.numberOfWrties >= dbType.max_writes)
                {
                    dbType.numberOfWrties = 0;
                    trigger_primitive_storage<Key, Data>(dbType);
                }
            }
            return res;

        }

        //----< add key value pair info for collection type db >-------------------------------------
        public bool addKeyValyePair<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, DBElement<Key, Data> dbElem, Key keyVal)
        where Data : IEnumerable<T>
        {
            bool res = dbType2.insert(keyVal, dbElem);
            if(res)
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_collection_storage<Key, Data, T>(dbType2);
                }
            }
            return res;
        }

        //----< add key value pair info for primitive type db >-------------------------------------
        public bool addKeyValyePair<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, DBElement<Key, Data> dbElem, Key keyVal)
        {
            bool res = dbType2.insert(keyVal, dbElem);
            if(res)
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_primitive_storage<Key, Data>(dbType2);
                }
            }
            return res;
        }

        //----< remove key value pair info for collection type db >-------------------------------------
        public bool removeKey<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2,Key key)
             where Data : IEnumerable<T>
        {
            bool res = dbType2.delete(key);
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_collection_storage<Key, Data, T>(dbType2);
                }
            }
            return res;
        }

        //----< remove key value pair info for primitive type db >-------------------------------------
        public bool removeKey<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key key)
        {
            bool res = dbType2.delete(key);
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_primitive_storage<Key, Data>(dbType2);
                }
            }
            return res;

        }

        //----< add children for collection type db >-------------------------------------
        public bool addChildren<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal,Key newChild)
           where Data : IEnumerable<T>
        {
            DBElement<Key, Data> dbElem;
            dbType2.getValue(keyVal, out dbElem);
            dbElem.children.Add(newChild);
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_collection_storage<Key, Data, T>(dbType2);
                }
            }
            return true;
        }

        //----< add children for primitive type db >-------------------------------------
        public bool addChildren<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, Key newChild)
        {
            DBElement<Key, Data> dbElem;
            dbType2.getValue(keyVal, out dbElem);
            dbElem.children.Add(newChild);
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_primitive_storage<Key, Data>(dbType2);
                }
            }
            return true;
        }

        //----< remove children for collection type db >-------------------------------------
        public bool removeChildren<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, Key newChild)
           where Data : IEnumerable<T>
        {
            DBElement<Key, Data> dbElem;
            dbType2.getValue(keyVal, out dbElem);
            bool res = dbElem.children.Remove(newChild);
            if(res)
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_collection_storage<Key, Data, T>(dbType2);
                }
            }
            return res;
        }

        //----< remove children for primitive type db >-------------------------------------
        public bool removeChildren<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType2, Key keyVal, Key newChild)
        {
            DBElement<Key, Data> dbElem;
            dbType2.getValue(keyVal, out dbElem);
            bool res = dbElem.children.Remove(newChild);
            if(res)
            {
                dbType2.numberOfWrties++;
                if (dbType2.numberOfWrties >= dbType2.max_writes)
                {
                    dbType2.numberOfWrties = 0;
                    trigger_primitive_storage<Key, Data>(dbType2);
                }
            }
            return res;
        }

        //----< trigger primitive storage persisting >-------------------------------------
        private void trigger_primitive_storage<Key, Data>(DBEngine<Key, DBElement<Key, Data>> dbType)
        {
            string dir = "..\\..\\..\\..\\input_xml\\";
            string xmlFile3 = "nosqldb_primitive.xml";
            String maxWrites = "\n\n Save call triggered after exceeding maximum " + dbType.max_writes + " writes to DB";
            maxWrites.title();
            dbType.persist_db<Key, DBElement<Key, Data>, Data>(dir + xmlFile3);
        }


        //----< trigger collection persisting >-------------------------------------
        private void trigger_collection_storage<Key, Data, T>(DBEngine<Key, DBElement<Key, Data>> dbType2)
        {
            string dir = "..\\..\\..\\..\\input_xml\\";
            String maxWrites = "\n\n Save call triggered after exceeding maximum " + dbType2.max_writes + " writes to DB";
            maxWrites.title();
            string xmlFile2 = "nosqdldb.xml";
            dbType2.persist_db<Key, DBElement<Key, Data>, List<T>, T>(dir + xmlFile2);
        }
    }

#if (TEST_DBITEMEDITOR)
    class DBItemEditorTest
    {
        static void Main(string[] args)
        {

            DBEngine<int, DBElement<int, string>> dbType1 = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbType2 = new DBEngine<string, DBElement<string, List<string>>>();
            DBItemEditor editor = new DBItemEditor();

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
            elem2.children.AddRange(new List<int> { 4, 5 });
            elem2.payload = "Directed by Robert Zemeckis and written by Willian Broyles Jr.";
            editor.addKeyValyePair<int, String>(dbType1, elem2, DBElementExtensions.generate_int_key());
            dbType1.showDB();

            Console.WriteLine("\n\n Before updating metadata");
            IEnumerable<int> keys1 = dbType1.Keys();
            int first = keys1.First();
            dbType1.showDB();
            Console.WriteLine("\n\n After updating metadata");
            editor.updateMetadataInfo<int, String>(dbType1, first, "Reborn -Cast Away", "The guy who survived in deserted insland");

            dbType1.showDB();

            IEnumerable<int> keys = dbType1.Keys();
            int firstDB1Key = keys.ElementAt(0);
            int secondDB1Key = keys.ElementAt(1);

            DBElement<int, string> elem22 = new DBElement<int, string>();
            elem22.name = "Titanic Reborn";
            elem22.descr = "A new movie directed in 2015 with the same plot line";
            elem22.timeStamp = DateTime.Now;
            elem22.children.AddRange(new List<int> { 1 });
            elem22.payload = "The movie will feature same actors but director changes";
            editor.updatePayloadInfo<int, String>(dbType1, elem22, secondDB1Key);

            Console.WriteLine("\n\n Before adding child Instance " + secondDB1Key + " from key " + firstDB1Key);
            dbType1.showDB();
            editor.addChildren<int, string>(dbType1, firstDB1Key, secondDB1Key);
            Console.WriteLine("\n\n After adding child Instance : " + secondDB1Key + " from key " + firstDB1Key);
            dbType1.showDB();

            Console.WriteLine("\n\n Before removing child Instance key " + 114 + " from key " + firstDB1Key);
            dbType1.showDB();
            editor.removeChildren<int, string>(dbType1, firstDB1Key, 114);
            Console.WriteLine("\n\n After removing child Instance key " + 114 + " from key " + firstDB1Key);
            dbType1.showDB();

        }
    }
    #endif
}
