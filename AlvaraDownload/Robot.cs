using AlvaraDownload.Services.Excel;
using Helpdesk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlvaraDownload
{
    public class Robot
    {
        public async Task Executar()
        {

            var excelService = new ExcelReaderService();
            var lista = excelService.Ler("C:\\Automations\\teste\\AlvarasDonwloadExcel.xlsx");

            Console.WriteLine("INICIANDO LEITURA");

            foreach(var item in lista)
            {
                Console.WriteLine($"Processando CNPJ: {item.Cnpj} e Nome Empresa:{item.NomeEmpresa}");
            }
            Console.WriteLine($"LEITURA FINALIZADA TOTAL: {lista.Count}");


            //ler excel 
            //loop cnpj
            //processar
            //salvar processe
        }
    }
}
