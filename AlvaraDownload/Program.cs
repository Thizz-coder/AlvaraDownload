using ClosedXML.Excel;

using var workbook = new XLWorkbook("C:\\Users\\thiag\\Downloads\\AlvasBlumenau.xlsx");
var ws = workbook.Worksheet(1);

var valor = ws.Cell(1, 1).GetString();

Console.WriteLine(valor);