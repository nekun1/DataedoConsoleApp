namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ConsoleApp.Models;

    public static class DataReader
    {
        static List<ImportedObject> ImportedObjects;
        /// <summary>
        /// Import object from set file and print it's contents to the console.
        /// </summary>
        /// <param name="fileToImport"> Path to the file. </param>
        public static void ImportAndPrintData(string fileToImport)
        {
            ImportedObjects = new List<ImportedObject>();

            var streamReader = new StreamReader(fileToImport);

            var importedLines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (line.Count() != 0) //check if the line is not empty
                {
                    importedLines.Add(line); 
                }
            }

            //construct objects from read lines
            for (int i = 0; i < importedLines.Count; i++)
            {
                var importedLine = importedLines[i];
                string[] values = new string[7]; //create a fallback array in case the read line is not long enough
                var importedValues = importedLine.Split(';');
                //asign the imported lines array's values to the fallback array
                for (int j = 0; j < importedValues.Length; j++)
                {
                    values[j] = importedValues[j];
                }
                //asign the read values to a new object
                var importedObject = new ImportedObject(values[0], values[1], values[2], values[3], values[4], values[5], values[6]);
                ImportedObjects.Add(importedObject);
            }

            // clear and correct imported data
            foreach (var importedObject in ImportedObjects)
            {
                importedObject.Type = ClearUpData(importedObject.Type, true);
                importedObject.Name = ClearUpData(importedObject.Name);
                importedObject.Schema = ClearUpData(importedObject.Schema);
                importedObject.ParentName = ClearUpData(importedObject.ParentName);
                importedObject.ParentType = ClearUpData(importedObject.ParentType);
            }

            // assign number of children
            for (int i = 0; i < ImportedObjects.Count(); i++)
            {
                var importedObject = ImportedObjects.ToArray()[i];
                foreach (var impObj in ImportedObjects)
                {
                    if (impObj.ParentType != importedObject.Type || impObj.ParentName != importedObject.Name)
                        continue;

                    importedObject.NumberOfChildren = 1 + importedObject.NumberOfChildren;
                }
            }
            //print every entry from the list
            foreach (var database in ImportedObjects)
            {
                if (database.Type != "DATABASE")
                    continue;
                PrintDataBase(database);
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Format the object's properties into a more readable form
        /// </summary>
        /// <param name="data">The data to format</param>
        /// <param name="toUpper">Should ToUpper() be used?</param>
        /// <returns></returns>
        static string ClearUpData(string data, bool toUpper = false)
        {
            string clearedUp = data.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
            if (toUpper)
                clearedUp = clearedUp.ToUpper();
            
            return clearedUp;
        }

        /// <summary>
        /// Print the contents of the imported list in a easily readable form
        /// </summary>
        /// <param name="database">The ImportedObject to print from</param>
        static void PrintDataBase(ImportedObject database)
        {
            Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

            // print all database's tables
            foreach (var table in ImportedObjects)
            {
                if (table.ParentType.ToUpper() != database.Type || table.ParentName != database.Name)
                    continue;

                Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                // print all table's columns
                foreach (var column in ImportedObjects)
                {
                    if (column.ParentType.ToUpper() != table.Type || column.ParentName != table.Name)
                        continue;

                    Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable == "1" ? "accepts nulls" : "with no nulls")}");
                }
            }
        }
    }

}
