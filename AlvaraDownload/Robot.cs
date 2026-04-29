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

            try
            {
                var caminho = "C:\\Automations\\teste\\AlvarasDonwloadExcel.xlsx";
                Console.WriteLine("INICIANDO LEITURA");



                var excelService = new ExcelReaderService();
                AlvaraService alvaraService = new AlvaraService();
                ExcelWriterService excelWriterService = new ExcelWriterService();

                var lista = excelService.Ler(caminho);

                foreach (var item in lista)
                {

                    var resultado = await alvaraService.DownloadAlvara(item.Cnpj, item.NomeEmpresa);
                    excelWriterService.EscreverStatus(caminho, item.LinhaExcel, resultado.StatusExcel);
                }
            }
            catch (Exception ex)
            {
                {
                    Console.WriteLine($"Erro na execução do robô: {ex.Message}");
                }
            }
        }
    }
}
