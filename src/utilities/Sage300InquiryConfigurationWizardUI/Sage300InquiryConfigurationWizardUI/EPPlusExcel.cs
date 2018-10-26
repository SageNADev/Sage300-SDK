// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#region Imports
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace Sage300InquiryConfigurationWizardUI
{
    public static class EPPlusExcel
    {
        /// <summary>
        /// Open Excel
        /// </summary>
        /// <param name="transRec">Transaction Log Record</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static ExcelPackage openExcel(string fileName)
        {
            ExcelPackage excel = null;

            try
            {
                excel = new ExcelPackage(new System.IO.FileInfo(fileName));
            }
            catch (Exception evt)
            {
                Exception ex = new Exception(string.Format("Open Excel File Fail: {0}! {1}", fileName, evt.Message));
                throw ex;
            }

            return excel;
        }

        /// <summary>
        /// Close Excel File
        /// </summary>
        /// <param name="excel"></param>
        public static void closeExcel(ExcelPackage excel)
        {
            excel.Dispose();
        }

        /// <summary>
        /// Get the list of worksheets from an excel file
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        public static List<String> excelSheets(ExcelPackage excel)
        {
            List<string> wsSheets = new List<string>();

            foreach (var ws in excel.Workbook.Worksheets)
            {
                wsSheets.Add(ws.Name);
            }

            return wsSheets;
        }

        /// <summary>
        /// Read excel cell value
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static string excelCellValue(ExcelWorksheet ws, int row, int col)
        {
            object obj = ws.Cells[row, col].Value;
            if (obj != null)
                return obj.ToString();
            else
                return "";
            //return ws.Cells[row, col].Value.ToString();
        }
    }
}
