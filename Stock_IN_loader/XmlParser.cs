﻿using System;
using System.Linq;
using System.Xml.Linq;
using bookPublishDB;
using System.IO;
using System.Globalization;

namespace Stock_IN_loader
{
    class XmlParser
    {
        private string path;
        private BookRepository _db = new BookRepository();

        public XmlParser(string path)
        {
            this.path = path;
        }

        public bool XmlRead()
        {            
            try
            {
                var xml = XDocument.Load(path);

                var elements = from node in xml.Elements("Books").Elements("Book")
                               where node != null
                               select node;

                Books book = new Books();

                foreach (var item in elements)
                {
                    Console.WriteLine("Felolvasott könyv: " + item.Element("Name").Value + ", ISBN száma: " + item.Element("ISBN").Value + " " + item.Element("ValidFrom").Value + " " + item.Element("ValidTo").Value);

                    book.ISBN = item.Element("ISBN").Value;
                    book.Name = item.Element("Name").Value;
                    book.ItemNumber = int.Parse(item.Element("ItemNumber").Value);
                    book.NetValue = int.Parse(item.Element("NetValue").Value);
                    book.Vat = int.Parse(item.Element("Vat").Value);
                    book.GrossValue = int.Parse(item.Element("GrossValue").Value);
                    book.ValidFrom = DateTime.ParseExact(item.Element("ValidFrom").Value, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);
                    book.ValidTo = DateTime.ParseExact(item.Element("ValidTo").Value, "yyyy-mm-dd", System.Globalization.CultureInfo.InvariantCulture);
                    book.Author = _db.GetAuthor((int.Parse(item.Element("Author_ID").Value)));
                    //Console.WriteLine(book.ValidTo.ToString());

                    SaveData(book);
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Jelenleg nincs feldolgozandó dokumentum, vagy hibás a konfig.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt a xml feldolgozása közben: " + ex);
                return false;
            }

            return true;
        }

        public void SaveData(Books book)
        {
            try
            {                
                if (_db.BookIsExists(book.ISBN))
                {
                    _db.SaveBooks(book);
                    Console.WriteLine("Mentés sikeres.");
                }
                else
                {
                    Console.WriteLine("Ilyen ISBN számmal már létezik könyv! Adat betöltése megszakítva.");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba az adatok mentése: " + ex);
            }
        }

        public void MoveFile(string source, string dest, string filename)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            try
            {
                string s = Path.Combine(source, filename);
                string date = System.DateTime.Now.ToShortTimeString();
                string time = date.Replace(':', '_');
                filename = "Feldolgozott"+time+ filename; 
                string d = Path.Combine(dest, filename);

                File.Move(s, d);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a fájl átmozhatása közben! " + ex.Message);
            }
        }

    }
}
