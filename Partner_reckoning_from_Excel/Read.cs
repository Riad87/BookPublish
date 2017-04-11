using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using bookPublishDB;
using System.Xml;

namespace Partner_reckoning_from_Excel
{
    public class Read
    {
        private BookRepository _db = new BookRepository();
        private PayOff payoff = new PayOff();

        public bool ReadFromExcel(string path, string filename, string xmlname, string dest)
        {
            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;

                int rCnt = 0;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(Path.Combine(path, filename), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;

                //XML létrehozáshoz
                string date = System.DateTime.Now.ToShortTimeString();
                string time = date.Replace(':', '_');
                string xmlkimenet = "Betolto" + time + xmlname;

                XmlTextWriter writer = new XmlTextWriter(Path.Combine(dest, xmlkimenet), System.Text.Encoding.UTF8);
                writer.WriteStartDocument(true);
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 2;
                writer.WriteStartElement("PayOffs");

                //excelből olvasás + mentés db-be és xml node készítés
                for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
                {
                    payoff.ISBN = (string)((range.Cells[rCnt, 2] as Excel.Range).Value2).ToString();
                    Console.Write(payoff.ISBN + ", ");

                    payoff.Name = (string)((range.Cells[rCnt, 3] as Excel.Range).Value2).ToString();
                    Console.Write(payoff.Name + ", ");

                    payoff.Quantity = int.Parse(((range.Cells[rCnt, 4] as Excel.Range).Value2).ToString());
                    Console.Write(payoff.Price + ", ");

                    payoff.Price = int.Parse(((range.Cells[rCnt, 5] as Excel.Range).Value2).ToString());
                    Console.WriteLine(payoff.Quantity);

                    _db.SavePayOff(payoff);
                    CreateNode(payoff.ISBN, payoff.Name, payoff.Quantity.ToString(), payoff.Price.ToString(), writer);

                }

                //xml lezárása
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();

                xlWorkBook.Close(true, null, null);
                xlApp.Quit();

                ReleaseObject(xlWorkSheet);
                ReleaseObject(xlWorkBook);
                ReleaseObject(xlApp);

            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Jelenleg nincs feldolgozandó dokumentum, vagy hibás a konfig.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a feldolgozás során!" + ex.Message);
                return false;
            }

            return true;
        }

        private void CreateNode(string isbn, string name, string quantity, string price, XmlTextWriter writer)
        {
            writer.WriteStartElement("Payoff");
            writer.WriteStartElement("ISBN");
            writer.WriteString(isbn);
            writer.WriteEndElement();

            writer.WriteStartElement("Name");
            writer.WriteString(name);
            writer.WriteEndElement();

            writer.WriteStartElement("Quantity");
            writer.WriteString(quantity);
            writer.WriteEndElement();

            writer.WriteStartElement("Price");
            writer.WriteString(price);
            writer.WriteEndElement();
            writer.WriteEndElement();
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
                filename = "Feldolgozott" + time + filename;
                string d = Path.Combine(dest, filename);

                File.Move(s, d);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a fájl átmozhatása közben! " + ex.Message);
            }
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Objektum felszabadítása nem sikerült. " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
