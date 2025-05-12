using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Utilities
{
    public class NPOISimpleExcelTable
    {
        List<NPOISimpleExcelColumn> columns;
        List<NPOISimpleExcelColumn> columnscenter;
        List<NPOISimpleExcelRow> rows;
        List<NPOISimpleExcelRow> rows1;
        List<string> headers;
        List<string> headerscenter;
        List<string> staffprofile_headers;
        List<string[]> multiple_headers;
        List<string> headerright;
        string sheetName;
        int rowIndex = -1; int rowIndex1 = -1;
        string fontFamily = "";
        double fontSize = 0;


        public NPOISimpleExcelTable(string font, double fontSize)
        {
            this.fontFamily = font;
            this.fontSize = fontSize;
        }


        public void AddColumn(string columnName, Type type, NPOIExcelColumnWidth width = NPOIExcelColumnWidth.AUTO)
        {
            if (columns == null)
            {
                columns = new List<NPOISimpleExcelColumn>();
            }

            int idx = columns.Count;
            columns.Add(new NPOISimpleExcelColumn(idx, columnName, type, (int)width));
        }

        public void AddColumnCenter(string columnName, Type type, NPOIExcelColumnWidth width = NPOIExcelColumnWidth.AUTO)
        {
            if (columnscenter == null)
            {
                columnscenter = new List<NPOISimpleExcelColumn>();
            }
            int idx = columnscenter.Count;

            columnscenter.Add(new NPOISimpleExcelColumn(idx, columnName, type, (int)width));
        }
        public void AddRow()
        {
            rowIndex++;

            if (rows == null)
            {
                rows = new List<NPOISimpleExcelRow>();
            }
            rows.Add(new NPOISimpleExcelRow());
            rows[rowIndex].Index = rowIndex;
            rows[rowIndex].Data = new List<NPOISimpleExcelRowData>();
        }

        public void AddRow1()
        {
            rowIndex1++;

            if (rows1 == null)
            {
                rows1 = new List<NPOISimpleExcelRow>();
            }
            rows1.Add(new NPOISimpleExcelRow());
            rows1[rowIndex1].Index = rowIndex1;
            rows1[rowIndex1].Data = new List<NPOISimpleExcelRowData>();
        }

        public void AddHeader(string header)
        {
            if (headers == null)
            {
                headers = new List<string>();
            }

            headers.Add(header);
        }

        public void AddHeadercenter(string header)
        {
            if (headerscenter == null)
            {
                headerscenter = new List<string>();
            }

            headerscenter.Add(header);
        }

        public void AddMultipleHeader(string[] header)
        {
            if (multiple_headers == null)
            {
                multiple_headers = new List<string[]>();
            }

            multiple_headers.Add(header);
            //multiple_headers.AddRange(header);
        }

        public void AddHeaderRight(string header)
        {
            if (headerright == null)
            {
                headerright = new List<string>();
            }

            headerright.Add(header);
        }

        public void AddHeaderForStaffInfoTitle(string header)
        {
            if (staffprofile_headers == null)
            {
                staffprofile_headers = new List<string>();
            }

            staffprofile_headers.Add(header);
        }

        public void SetSheetName(string name)
        {
            this.sheetName = name;
        }

        public void SetData(int column, object data)
        {
            if (rows != null)
            {
                rows[rowIndex].Data.Add(new NPOISimpleExcelRowData(column, data));
            }
        }

        public void SetData(int column, object data, bool isBold, bool isCenter, bool isRight)
        {
            if (rows != null)
            {
                rows[rowIndex].Data.Add(new NPOISimpleExcelRowData(column, data, isBold, isCenter, isRight));
            }
        }

        public void SetDataCenter(int column, object data)
        {
            if (rows1 != null)
            {
                rows1[rowIndex1].Data.Add(new NPOISimpleExcelRowData(column, data));
            }
        }

        public void SetData(string column, object data)
        {
            if (rows != null)
            {
                int colindex = getColumnIndex(column);
                rows[rowIndex].Data.Add(new NPOISimpleExcelRowData(colindex, data));
            }
        }

        public byte[] Generate()
        {
            byte[] bytes = null;
            if (columns != null)
            {
                HSSFWorkbook workbook = null;

                try
                {
                    int excelRows = 0;
                    workbook = new HSSFWorkbook();

                    HSSFFont font = (HSSFFont)workbook.CreateFont();
                    font.FontHeight = 13 * 20;
                    font.FontName = fontFamily;

                    HSSFFont headerfont = (HSSFFont)workbook.CreateFont();
                    headerfont.FontHeight = 13 * 20;
                    headerfont.IsBold = true;
                    headerfont.FontName = fontFamily;

                    HSSFFont columnNamefont = (HSSFFont)workbook.CreateFont();
                    columnNamefont.FontHeight = 13 * 20;
                    columnNamefont.FontName = fontFamily;
                    columnNamefont.IsBold = true;

                    if (sheetName == null)
                    {
                        sheetName = "Sheet1";
                    }
                    ISheet sheet = workbook.CreateSheet(sheetName);

                    if (headerscenter != null)
                    {
                        HSSFCellStyle headerCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headerCellStyle.SetFont(headerfont);
                        headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        headerCellStyle.Alignment = HorizontalAlignment.Center;
                        foreach (var h in headerscenter)
                        {
                            var headerRow = sheet.CreateRow(excelRows);
                            headerRow.CreateCell(0);
                            headerRow.Cells[0].CellStyle = headerCellStyle;
                            headerRow.Cells[0].SetCellValue(h);

                            NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows, excelRows, columns[0].Index, columns[columns.Count - 1].Index);
                            sheet.AddMergedRegion(range);
                            headerRow.HeightInPoints = 35;
                            excelRows++;
                        }
                    }

                    if (headers != null)
                    {
                        HSSFCellStyle headerCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headerCellStyle.SetFont(headerfont);
                        headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        headerCellStyle.Alignment = HorizontalAlignment.Left;
                        foreach (var h in headers)
                        {
                            var headerRow = sheet.CreateRow(excelRows);
                            foreach (var col in columns)
                            {
                                headerRow.CreateCell(col.Index);
                                headerRow.Cells[col.Index].CellStyle = headerCellStyle;
                            }
                            headerRow.Cells[0].SetCellValue(h);

                            NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows, excelRows, columns[0].Index, columns[columns.Count - 1].Index);
                            sheet.AddMergedRegion(range);
                            headerRow.HeightInPoints = 35;
                            excelRows++;
                        }
                    }

                    if (headerright != null)
                    {
                        HSSFCellStyle headerCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headerCellStyle.SetFont(headerfont);
                        headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        headerCellStyle.Alignment = HorizontalAlignment.Right;
                        foreach (var h in headerright)
                        {
                            var headerRow = sheet.CreateRow(excelRows);
                            headerRow.CreateCell(0);
                            headerRow.Cells[0].CellStyle = headerCellStyle;
                            headerRow.Cells[0].SetCellValue(h);

                            NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows, excelRows, columns[0].Index, columns[columns.Count - 1].Index);
                            sheet.AddMergedRegion(range);
                            headerRow.HeightInPoints = 30;
                            excelRows++;
                        }

                    }

                    if (multiple_headers != null)
                    {
                        HSSFCellStyle headerCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headerCellStyle.SetFont(columnNamefont);
                        headerCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        headerCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        headerCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        headerCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        headerCellStyle.WrapText = true;
                        headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        headerCellStyle.Alignment = HorizontalAlignment.Center;

                        foreach (var h in multiple_headers)
                        {
                            var headerRow = sheet.CreateRow(excelRows);
                            //foreach (var col in columns)
                            //{
                            //    headerRow.CreateCell(col.Index);
                            //    headerRow.Cells[col.Index].CellStyle = headerCellStyle;
                            //}
                            for (int i = 0; i < h.Length; i++)
                            {
                                if (i == 0)
                                {
                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                        excelRows + 1, columns[0].Index, columns[1].Index);
                                    sheet.AddMergedRegion(range);
                                }
                                else if (i == 2)
                                {
                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                         excelRows + 1, columns[2].Index, columns[3].Index - 1);
                                    sheet.AddMergedRegion(range);
                                }
                                else if (i == 3)
                                {
                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                         excelRows + 1, columns[3].Index, columns[4].Index - 1);
                                    sheet.AddMergedRegion(range);
                                }
                                else if (i == 4)
                                {

                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                         excelRows + 1, columns[4].Index, columns[5].Index - 1);
                                    sheet.AddMergedRegion(range);

                                }
                                else if (i == 5)
                                {
                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                        excelRows, columns[5].Index, columns[6].Index);
                                    sheet.AddMergedRegion(range);
                                }
                                else if (i == 7)
                                {

                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                         excelRows + 1, columns[6].Index + 1, columns[7].Index);
                                    sheet.AddMergedRegion(range);
                                }

                                else
                                {
                                    headerRow.CreateCell(i);
                                    headerRow.Cells[i].CellStyle = headerCellStyle;
                                    headerRow.Cells[i].SetCellValue(h[i]);
                                }
                                //else if (i == 3)
                                //{
                                //    headerRow.CreateCell(i);
                                //    headerRow.Cells[i].CellStyle = headerCellStyle;
                                //    headerRow.Cells[i].SetCellValue(h[i]);
                                //    NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows,
                                //        excelRows + 1, columns[1].Index, columns[1].Index);
                                //    sheet.AddMergedRegion(range);

                                //} 
                            }
                            //headerRow.Cells[0].SetCellValue(h);

                            //NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows, excelRows, columns[0].Index, columns[columns.Count - 1].Index);
                            //sheet.AddMergedRegion(range);
                            headerRow.HeightInPoints = 35;
                            excelRows++;
                        }

                    }

                    if (staffprofile_headers != null)
                    {
                        HSSFCellStyle headerCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                        headerCellStyle.SetFont(headerfont);
                        headerCellStyle.VerticalAlignment = VerticalAlignment.Center;
                        headerCellStyle.Alignment = HorizontalAlignment.Left;
                        foreach (var h in staffprofile_headers)
                        {
                            var headerRow = sheet.CreateRow(excelRows);
                            headerRow.CreateCell(0);
                            headerRow.Cells[0].CellStyle = headerCellStyle;
                            headerRow.Cells[0].SetCellValue(h);

                            NPOI.SS.Util.CellRangeAddress range = new NPOI.SS.Util.CellRangeAddress(excelRows, excelRows, columns[0].Index, columns[columns.Count - 1].Index);
                            sheet.AddMergedRegion(range);
                            headerRow.HeightInPoints = 20;
                            excelRows++;
                        }
                    }

                    HSSFCellStyle columnNameborderedCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    columnNameborderedCellStyle.SetFont(columnNamefont);
                    columnNameborderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    columnNameborderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    columnNameborderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    columnNameborderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    columnNameborderedCellStyle.WrapText = true;
                    columnNameborderedCellStyle.VerticalAlignment = VerticalAlignment.Center;
                    columnNameborderedCellStyle.Alignment = HorizontalAlignment.Center;

                    HSSFCellStyle borderedCellStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    borderedCellStyle.SetFont(font);
                    borderedCellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                    borderedCellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                    borderedCellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                    borderedCellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                    borderedCellStyle.WrapText = true;
                    borderedCellStyle.VerticalAlignment = VerticalAlignment.Center;

                    var columnNameRow = sheet.CreateRow(excelRows);
                    excelRows++;
                    foreach (var col in columns)
                    {
                        if (col.Width <= 0)
                        {
                            sheet.AutoSizeColumn(col.Index);
                        }
                        else
                        {
                            sheet.AutoSizeColumn(col.Index);
                            sheet.SetColumnWidth(col.Index, col.Width);
                        }
                        columnNameRow.CreateCell(col.Index).SetCellValue(col.Name);
                        columnNameRow.Cells[col.Index].CellStyle = columnNameborderedCellStyle;
                    }

                    if (rows1 != null)
                    {
                        var dataRow = sheet.CreateRow(excelRows);
                        excelRows++;
                        foreach (var data in rows1[0].Data)
                        {
                            SetCellValue(dataRow, data, columnNameborderedCellStyle);
                        }

                    }

                    if (rows != null)
                    {
                        foreach (var r in rows)
                        {
                            var dataRow = sheet.CreateRow(excelRows);
                            excelRows++;
                            foreach (var data in r.Data)
                            {

                                if (data.isBold)
                                {
                                    HSSFCellStyle borderedCellStyleData = (HSSFCellStyle)workbook.CreateCellStyle();
                                    borderedCellStyleData.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                                    borderedCellStyleData.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                                    borderedCellStyleData.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                                    borderedCellStyleData.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                                    borderedCellStyleData.WrapText = true;
                                    borderedCellStyleData.VerticalAlignment = VerticalAlignment.Center;
                                    borderedCellStyleData.SetFont(columnNamefont);

                                    SetCellValue(dataRow, data, borderedCellStyleData);
                                }
                                //else if (data.isCenter)
                                //{
                                //    HSSFCellStyle borderedCellStyleData = (HSSFCellStyle)workbook.CreateCellStyle();
                                //    borderedCellStyleData.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.WrapText = true;
                                //    borderedCellStyleData.VerticalAlignment = VerticalAlignment.Center;
                                //    borderedCellStyleData.SetFont(font);
                                //    borderedCellStyleData.Alignment = HorizontalAlignment.Center;

                                //    SetCellValue(dataRow, data, borderedCellStyleData);
                                //}
                                //else if (data.isRight)
                                //{
                                //    HSSFCellStyle borderedCellStyleData = (HSSFCellStyle)workbook.CreateCellStyle();
                                //    borderedCellStyleData.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                                //    borderedCellStyleData.WrapText = true;
                                //    borderedCellStyleData.VerticalAlignment = VerticalAlignment.Center;
                                //    borderedCellStyleData.SetFont(font);
                                //    borderedCellStyleData.Alignment = HorizontalAlignment.Right;

                                //    SetCellValue(dataRow, data, borderedCellStyleData);
                                //}
                                else
                                {
                                    SetCellValue(dataRow, data, borderedCellStyle);
                                }
                            }
                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.Write(stream);

                        bytes = stream.ToArray();

                        workbook.Close();
                    }

                }
                catch (Exception ex)
                {
                    if (workbook != null)
                    {
                        workbook.Close();
                        workbook = null;
                    }
                }

            }


            return bytes;
        }

        private int getColumnIndex(string colName)
        {
            int colIndex = 0;
            foreach (var col in columns)
            {
                if (col.Name == colName)
                {
                    colIndex = col.Index;
                    break;
                }
            }
            return colIndex;
        }

        void SetCellValue(IRow row, NPOISimpleExcelRowData data, HSSFCellStyle style)
        {
            if (columns[data.ColumnIndex].Type == typeof(string))
            {
                row.CreateCell(data.ColumnIndex).SetCellValue(Convert.ToString(data.Data));
                row.Cells[data.ColumnIndex].CellStyle = style;
            }
            else if (columns[data.ColumnIndex].Type == typeof(int))
            {
                row.CreateCell(data.ColumnIndex).SetCellValue(Convert.ToInt32(data.Data));
                row.Cells[data.ColumnIndex].CellStyle = style;
            }
            else if (columns[data.ColumnIndex].Type == typeof(double))
            {
                row.CreateCell(data.ColumnIndex).SetCellValue(Convert.ToDouble(data.Data));
                row.Cells[data.ColumnIndex].CellStyle = style;
            }
            else if (columns[data.ColumnIndex].Type == typeof(DateTime))
            {
                row.CreateCell(data.ColumnIndex).SetCellValue(Convert.ToDateTime(data.Data));
                row.Cells[data.ColumnIndex].CellStyle = style;
            }
            else if (columns[data.ColumnIndex].Type == typeof(DateTime))
            {
                row.CreateCell(data.ColumnIndex).SetCellValue(Convert.ToDateTime(data.Data));
                row.Cells[data.ColumnIndex].CellStyle = style;
            }
        }
    }

    public enum NPOIExcelColumnWidth
    {
        AUTO = 0,
        S1 = 1000,
        S2 = 2000,
        S3 = 3000,
        S4 = 4000,
        S5 = 5000,
        M1 = 6000,
        M2 = 7000,
        M3 = 8000,
        M4 = 9000,
        M5 = 10000,
        L1 = 11000,
        L2 = 12000,
        L3 = 13000,
        L4 = 14000,
        L5 = 15000
    }

    class NPOISimpleExcelRow
    {
        public int Index { get; set; }
        public List<NPOISimpleExcelRowData> Data { get; set; }
    }

    class NPOISimpleExcelRowData
    {
        public int ColumnIndex { get; set; }
        public object Data { get; set; }
        public bool isBold { get; set; }
        public bool isCenter { get; set; }
        public bool isRight { get; set; }

        public NPOISimpleExcelRowData()
        {

        }

        public NPOISimpleExcelRowData(int colIndex, object data)
        {
            this.ColumnIndex = colIndex;
            this.Data = data;
            //if(data != null)
            //{
            //    Type a = data.GetType();
            //}
            //else
            //{
            //    Type a;
            //}

        }

        public NPOISimpleExcelRowData(int colIndex, object data, bool isBold, bool isCenter, bool isRight)
        {
            this.ColumnIndex = colIndex;
            this.Data = data;
            this.isBold = isBold;
            this.isCenter = isCenter;
            this.isRight = isRight;
        }
    }

    class NPOISimpleExcelColumn
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public int Width { get; set; }

        public NPOISimpleExcelColumn(int index, string name, Type type, int width)
        {
            this.Index = index;
            this.Name = name;
            this.Type = type;
            this.Width = width;
        }
    }
}
