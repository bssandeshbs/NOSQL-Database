///////////////////////////////////////////////////////////////////
// Executive.cs - Test Requirements of Project 2                 //
// Ver 1.2                                                       //
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
 * This package begins the demonstration of meeting requirements.
 * - DBEngine objects are created and demonstration package is invoked
 *   to demonstrate all the requirements
 *
 *
 * Public Interface:
 * =================
 * This class doesnt contain any public interface functions
 *
 * Maintenance:
 * ------------
 * Required Files: 
 *   TestExec.cs,  DBElement.cs, DBEngine, Display, 
 *   DBExtensions.cs, UtilityExtensions.cs
 *
 * Build Process:  devenv Project2Starter.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *
 * Maintenance History:
 * --------------------
 * ver 1.2 : 5 Oct 15
 * - added new methods in main()
 * ver 1.1 : 24 Sep 15
 * ver 1.0 : 18 Sep 15
 * - first release
 */
using System;
using System.Collections.Generic;

namespace NoSQLDB
{
  class Executive
  {
      
     static void Main(string[] args)
     {
      DBEngine<int, DBElement<int, string>> dbType = new DBEngine<int, DBElement<int, string>>();
      DBEngine<string, DBElement<string, List<string>>> dbCollectionType = new DBEngine<string, DBElement<string, List<string>>>();
      DBItemEditor editor = new DBItemEditor();

      ReqDemos demos = new ReqDemos();
      demos.TestR2(dbType, dbCollectionType, editor);
      demos.TestR3(dbType,dbCollectionType, editor);
      demos.TestR3_NonPrimitive(dbType, dbCollectionType, editor);
      demos.TestR4(dbType, dbCollectionType, editor);
      demos.TestR4_NonPrimitive(dbType, dbCollectionType, editor);
      demos.TestR5(dbType, dbCollectionType);
      demos.TestR7(dbType, dbCollectionType);
      demos.testR7c(dbType, dbCollectionType);
      demos.TestR8();
      demos.testR9();
      demos.testR12();
      demos.TestR6(dbType, dbCollectionType);
    
      Console.ReadKey();
    }
  }
}
