using AlvaraDownload.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace AlvaraDownload.Services.Excel
{
    internal class ExcelReaderService
    {
        public List<CnpjInputModel> Ler(string caminho)
        {
            var lista = new List<CnpjInputModel>();

            using(var workbook = new XLWorkbook(caminho))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RowsUsed().Skip(1); // Pula o cabeçalho
                foreach(var row in rows)
                {
                    lista.Add(new CnpjInputModel
                    {
                        NomeEmpresa = row.Cell(1).GetValue<string>(),
                        Cnpj = row.Cell(2).GetValue<string>(),
                        LinhaExcel = row.RowNumber()
                    });

                }
            }
            return lista;
        }   
    }
}
