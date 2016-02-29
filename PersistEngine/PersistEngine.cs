///////////////////////////////////////////////////////////////////
// PersistEngine.cs - Persist the db engine contents to xml      //
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
 * This package provides the persist, unpersit and augument functionality
 * to store the dbengine / xml contents 
 *
 * Public Interface:
 * =================
 * public static void persist_db<Key, Value, Data, T>(this DBEngine<Key, Value> db)
 *            where Data : IEnumerable<T>
 * - Persists DBEngine Collection Type contents to a xml file
 * public static void persist_db<Key, Value, Data>(this DBEngine<Key, Value> db)
 * - Persists DBEngine Primitive Type contents to a xml file
 * public static void augument_db<Key, Value, Data, T>(this DBEngine<Key, Value> db)
 *            where Data : List<T >
 * - Reads xml contents and auguments to a existing collection type db
 * public static void augument_db<Key, Value, Data>(this DBEngine<Key, Value> db)
 * - Reads xml contents and auguments to a existing primitive type db
 *
 * Maintenance:
 * ------------
 * Required Files: DBEngine.cs, DBElement.cs,DBExtensions,Display.cs and
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
using System.Xml.Linq;
using static System.Console;
using System.Xml;

namespace NoSQLDB
{
    public static class PersistEngine
    {
        //----< Persist collection type db to a xml file>-------------------
        public static void persist_db<Key, Value, Data, T>(this DBEngine<Key, Value> db,string fileName)
             where Data : IEnumerable<T>
        {
            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XComment comment = new XComment("Demonstrating XML");
            xml.Add(comment);
            XElement root = new XElement("noSqlDb");
            xml.Add(root);
            XElement keyType = new XElement("keytype");
            keyType.SetValue(typeof(Key));
            root.Add(keyType);
            XElement payload = new XElement("payloadtype");
            payload.SetValue(typeof(Data));
            root.Add(payload);
            foreach (Key key in db.Keys())  //Iterate for each keys
            {
                XElement child = new XElement("record");
                root.Add(child);
                XElement keyNode = new XElement("key");
                keyNode.SetValue(key);
                child.Add(keyNode);
                XElement elementNode = new XElement("element");
                child.Add(elementNode);
                Value value;
                db.getValue(key, out value);
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;

                String timestamp = String.Format("{0}", elem.timeStamp);
                XElement nameNode = new XElement("name");
                nameNode.SetValue(elem.name);

                XElement descrNode = new XElement("descr");
                descrNode.SetValue(elem.descr);
                XElement timestampNode = new XElement("timestamp");
                timestampNode.SetValue(timestamp);
                elementNode.Add(nameNode);
                elementNode.Add(descrNode);
                elementNode.Add(timestampNode);
                persist_child_payload<Key, Value, Data, T>(elem, elementNode);
            }
            try
            {
                xml.Save(fileName);
            }
            catch (Exception) { Console.WriteLine("Invalid Directory Specified"); }
        }

        //----< Helper function to persist payload and children info to a xml file>-------------------
        private static void persist_child_payload<Key, Value, Data, T>(DBElement<Key, Data>  elem, XElement elementNode)
              where Data : IEnumerable<T>
        {
            if (elem.children.Count() > 0)
            {
                XElement childrensNode = new XElement("children");
                elementNode.Add(childrensNode);
                foreach (Key childrenkeys in elem.children)
                {
                    XElement childrenkey = new XElement("key");
                    childrenkey.SetValue(childrenkeys.ToString());
                    childrensNode.Add(childrenkey);
                }
            }

            if (elem.payload != null)
            {
                XElement payLoadNode = new XElement("payload");
                IEnumerable<object> d = elem.payload as IEnumerable<object>;
                if (d == null)
                    payLoadNode.SetValue(elem.payload.ToString());
                else
                {
                    foreach (var item in elem.payload)
                    {
                        XElement itemNode = new XElement("item");
                        itemNode.SetValue(item);
                        payLoadNode.Add(itemNode);
                    }
                }
                elementNode.Add(payLoadNode);
            }
        }
    
        //----< Persist primitive type db to a xml file>-------------------
        public static void persist_db<Key, Value, Data>(this DBEngine<Key, Value> db,String fileName)
        {
            XDocument xml = new XDocument();
            xml.Declaration = new XDeclaration("1.0", "utf-8", "yes");
            XComment comment = new XComment("Demonstrating XML");
            xml.Add(comment);
            XElement root = new XElement("noSqlDb");
            xml.Add(root);
            XElement keyType = new XElement("keytype");
            keyType.SetValue(typeof(Key));
            root.Add(keyType);
            XElement payload = new XElement("payloadtype");
            payload.SetValue(typeof(Data));
            root.Add(payload);

            foreach (Key key in db.Keys())  //Iterate for each keys
            {
                XElement child = new XElement("record");
                root.Add(child);
                XElement keyNode = new XElement("key");
                keyNode.SetValue(key);
                child.Add(keyNode);
                XElement elementNode = new XElement("element");
                child.Add(elementNode);
                Value value;
                db.getValue(key, out value);
                DBElement<Key, Data> elem = value as DBElement<Key, Data>;

                String timestamp = String.Format("{0}", elem.timeStamp);
                XElement nameNode = new XElement("name");
                nameNode.SetValue(elem.name);
                XElement descrNode = new XElement("descr");
                descrNode.SetValue(elem.descr);
                XElement timestampNode = new XElement("timestamp");
                timestampNode.SetValue(timestamp);

                elementNode.Add(nameNode);
                elementNode.Add(descrNode);
                elementNode.Add(timestampNode);

                persist_child_payload_primtive<Key, Value, Data>(elem, elementNode);
            }
            try
            {
                xml.Save(fileName);
            } catch (Exception) {
                Console.WriteLine("Invalid Directory Specified");
            }
        }

        //----< Helper function to persist payload and children info to a xml file>-------------------
        private static void persist_child_payload_primtive<Key, Value, Data>(DBElement<Key, Data> elem, XElement elementNode)
        {
            if (elem.children.Count() > 0)
            {
                XElement childrensNode = new XElement("children");
                elementNode.Add(childrensNode);
                foreach (Key childrenkeys in elem.children)
                {
                    XElement childrenkey = new XElement("key");
                    childrenkey.SetValue(childrenkeys.ToString());
                    childrensNode.Add(childrenkey);
                }
            }

            if (elem.payload != null)
            {
                XElement payLoadNode = new XElement("payload");
                payLoadNode.SetValue(elem.payload);
                elementNode.Add(payLoadNode);
            }
        }

        //----< Augument xml file db contents to existing collection type db>-------------------
        public static void augument_db<Key, Value, Data, T>(this DBEngine<Key, Value> db,string fileName)
             where Data : List<T>
        { try
            {
                XDocument newDoc = null;
                newDoc = XDocument.Load(fileName);
                WriteLine("\n\nInput XML\n");
                WriteLine(newDoc.ToString());
                XElement keyElement = newDoc.Root.Element("keytype");
                XElement payloadElement = newDoc.Root.Element("payloadtype");
                String keyType = keyElement.Value;
                String payLoad = payloadElement.Value; // check the key-type
                if (typeof(Key).ToString() == keyType && typeof(Data).ToString() == payLoad)
                {
                    IEnumerable<XElement> elements = newDoc.Root.Elements("record");
                    foreach (XElement item in elements)  //Extract each record
                    {
                        string KeyStringval = item.Element("key").Value;
                        Key key = (Key)Convert.ChangeType(KeyStringval, typeof(Key));
                        DBElement<Key, Data> dbElement = new DBElement<Key, Data>();
                        XElement myelement = item.Element("element");
                        dbElement.name = myelement.Element("name").Value;
                        dbElement.descr = myelement.Element("descr").Value;
                        dbElement.timeStamp = DateTime.Now;
                        IEnumerable<XElement> childrensElements = myelement.Elements("children").Elements("key");
                        foreach (XElement child in childrensElements) {
                            Key childKey = (Key)Convert.ChangeType(child.Value, typeof(Key));
                            dbElement.children.AddRange(new[] { childKey });
                        }
                        IEnumerable<XElement> payloadElements = myelement.Elements("payload").Elements("item");
                        List<T> payloadlist = new List<T>();
                        foreach (XElement child in payloadElements) {
                            T payLoadKey = (T)Convert.ChangeType(child.Value, typeof(T));
                            payloadlist.Add(payLoadKey);
                        }
                        dbElement.payload = payloadlist as Data;
                        Value valueType = (Value)Convert.ChangeType(dbElement, typeof(Value));
                        db.insert(key, valueType);
                    }
                } else {
                    Console.WriteLine("Not matching db");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("File Not Found to Read");
            }
        }

        //----< Augument xml file db contents to existing primtive type db>-------------------
        public static void augument_db<Key, Value, Data>(this DBEngine<Key, Value> db,string fileName)
        {
            XDocument newDoc = null;
            try
            {
                newDoc = XDocument.Load(fileName);
                WriteLine("\n\nInput XML\n");
                WriteLine(newDoc.ToString());
                XElement keyElement = newDoc.Root.Element("keytype");
                XElement payloadElement = newDoc.Root.Element("payloadtype");

                String keyType = keyElement.Value;
                String payLoad = payloadElement.Value; // check the key-type
                if (typeof(Key).ToString() == keyType && typeof(Data).ToString() == payLoad)
                {
                    IEnumerable<XElement> elements = newDoc.Root.Elements("record");
                    foreach (XElement item in elements)  // Extract each record
                    {
                        string KeyStringval = item.Element("key").Value;
                        Key key = (Key)Convert.ChangeType(KeyStringval, typeof(Key));
                        DBElement<Key, Data> dbElement = new DBElement<Key, Data>();

                        XElement myelement = item.Element("element");
                        dbElement.name = myelement.Element("name").Value;
                        dbElement.descr = myelement.Element("descr").Value;
                        dbElement.timeStamp = DateTime.Now;

                        IEnumerable<XElement> childrensElements = myelement.Elements("children").Elements("key");
                        foreach (XElement child in childrensElements)
                        {
                            Key childKey = (Key)Convert.ChangeType(child.Value, typeof(Key));
                            dbElement.children.AddRange(new[] { childKey });
                        }
                        Data payLoadData = (Data)Convert.ChangeType(myelement.Element("payload").Value, typeof(Data));
                        dbElement.payload = payLoadData;
                        Value valueType = (Value)Convert.ChangeType(dbElement, typeof(Value));
                        db.insert(key, valueType);
                    }
                }
                else
                {
                    Console.WriteLine("Not matching db");
                }
            } catch(Exception )
            {
                Console.WriteLine("File Not Found to Read");
            }
        }
    }
#if (TEST_PERSISTENG)
    class PersistEngineTest
{

    static void Main(string[] args)
    {
        "Demonstrating Persist Engine".title('=');
            DBEngine<int, DBElement<int, string>> dbType1 = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbType2 = new DBEngine<string, DBElement<string, List<string>>>();
       
            //Demonstrating primitive type
            DBElement<int, string> elem1 = new DBElement<int, string>();
            elem1.name = "Jurassic World";
            elem1.descr = "Story on escape from giant creatures";
            elem1.timeStamp = DateTime.Now;
            elem1.payload = "A giant creature attacks the park and becomes a killing machine";
            dbType1.insert(DBElementExtensions.generate_int_key(), elem1);

            DBElement<int, string> elem2 = new DBElement<int, string>();
            elem2.name = "Cast Away";
            elem2.descr = "Story of surviving a crash landing on a deserted island.";
            elem2.timeStamp = DateTime.Now;
            elem2.children.AddRange(new List<int> { 4, 5 });
            elem2.payload = "Directed by Robert Zemeckis and written by Willian Broyles Jr.";
            dbType1.insert(DBElementExtensions.generate_int_key(), elem2);
            dbType1.showDB();

            //Demostrating IEnumberable Type 
            DBElement<string, List<string>> newerelem1 = new DBElement<string, List<string>>();
            newerelem1.name = "Movie Name - The Good the Bad and the Ugly";
            newerelem1.descr = "A bounty hunting scam joins two men in an uneasy alliance ";
            newerelem1.payload = new List<string> { "Clint Eastwood", " Eli Wallach", "Lee Van Cleef" };
            String key = "The Good, the Bad and the Ugly";
            dbType2.insert(key, newerelem1);

            DBElement<string, List<string>> newerelem2 = new DBElement<string, List<string>>();
            newerelem2.name = "Movie Name - Django Unchained";
            newerelem2.descr = "With the help of a German hunter, a freed slave sets to rescue";
            newerelem2.children.AddRange(new[] { key, "Life Is Beautiful" });
            newerelem2.payload = new List<string> { "Jamie Foxx", "Christoph Waltz", "Leonardo DiCaprio" };
            newerelem2.payload.Add("Quentin Tarantino");
            String key1 = "Django Unchained";
            dbType2.insert(key1, newerelem2);
            dbType2.showEnumerableDB();

            string xmlFile1 = "nosqdldb_primitive.xml";
            dbType1.persist_db<int, DBElement<int, string>, string>(xmlFile1);
            WriteLine("\nSuccesfully persisted dbengine contents to xml file :" + xmlFile1);

            string xmlFile2 = "nosqdldb.xml";
            dbType2.persist_db<string, DBElement<string, List<string>>, List<string>, string>(xmlFile2);
            WriteLine("\nSuccesfully persisted dbengine contents to xml file :  " + xmlFile2);

            string xmlFile3 = "read_nosqldb_primitive.xml";
            WriteLine("\n\n Before Augumenting DB from xml file : " + xmlFile3);
            dbType1.showDB();
            dbType1.augument_db<int, DBElement<int, string>, string>(xmlFile3);
            WriteLine("\n\n After Augumenting DB from xml file : " + xmlFile3);
            dbType1.showDB();

            string xmlFile4 = "read_nosqdldb.xml";
            WriteLine("\n\n Before Augumenting DB from xml file : " + xmlFile4);
            dbType2.showEnumerableDB();
            dbType2.augument_db<string, DBElement<string, List<string>>, List<string>, string>(xmlFile4);
            WriteLine("\n\n After Augumenting DB from xml file : " + xmlFile4);
            dbType2.showEnumerableDB();

            DBEngine<int, DBElement<int, string>> dbType1New = new DBEngine<int, DBElement<int, string>>();
            DBEngine<string, DBElement<string, List<string>>> dbType2New = new DBEngine<string, DBElement<string, List<string>>>();

            string xmlFile5 = "read_nosqldb_primitive.xml";
            WriteLine("\n\n Before Restoring DB from xml file : " + xmlFile5);
            dbType1New.showDB();
            dbType1New.augument_db<int, DBElement<int, string>, string>(xmlFile5);
            WriteLine("\n\n After Restoring DB from xml file : " + xmlFile5);
            dbType1New.showDB();

            string xmlFile6 = "read_nosqdldb.xml";
            WriteLine("\n\n Before Restoring DB from xml file : " + xmlFile6);
            dbType2New.showEnumerableDB();
            dbType2New.augument_db<string, DBElement<string, List<string>>, List<string>, string>(xmlFile6);
            WriteLine("\n\n After Restoring DB from xml file : " + xmlFile6);
            dbType2New.showEnumerableDB();
        }

}
#endif
}
