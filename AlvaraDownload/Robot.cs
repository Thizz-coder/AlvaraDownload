using AlvaraDownload.Services.Excel;
using Helpdesk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlvaraDownload.Services.Web;

namespace AlvaraDownload
{
    public class Robot
    {
        public async Task Executar() 
        {

            var excelService = new ExcelReaderService();
            Console.WriteLine("INICIANDO LEITURA");
            var lista = excelService.Ler("C:\\Automations\\teste\\AlvarasDonwloadExcel.xlsx");

            AlvaraService alvaraService = new AlvaraService();

            foreach (var item in lista)
            {
                
                await alvaraService.DownloadAlvara(item.Cnpj, item.NomeEmpresa);
            }
            


            //ler excel 
            //loop cnpj
            //processar
            //salvar processos
        }
    }
}
