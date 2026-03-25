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
                    var cnpj = row.Cell(2).GetValue<string>();
                    var nome = row.Cell(1).GetValue<string>();

                    lista.Add(new CnpjInputModel
                    {
                        Cnpj = cnpj,
                        NomeEmpresa = nome
                    });

                }
            }
            return lista;
        }   
    }
}
