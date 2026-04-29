using ClosedXML.Excel;

namespace AlvaraDownload.Services.Excel
{
    public class ExcelWriterService
    {
        public void EscreverStatus(string caminho, int linhaExcel, string status)
        {
            using var workbook = new XLWorkbook(caminho);
            var worksheet = workbook.Worksheet(1);

            worksheet.Cell(linhaExcel, 3).Value = status;

            workbook.Save();
        }
    }
}