using AlvaraDownload.Services.Excel;
using AlvaraDownload.Services. ;
using Helpdesk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlvaraDownload.Services.Web;
using AlvaraDownload.Core.Models;

namespace AlvaraDownload
{
    public class Robot
    {
        private readonly ConfigModel _config;

        public Robot(ConfigModel config)
        {
            _config = config;
        }

        public async Task Executar()
        {

            try
            { 
                
                Console.WriteLine("INICIANDO LEITURA");



                var excelService = new ExcelReaderService();
                AlvaraService alvaraService = new AlvaraService(_config);
                ExcelWriterService excelWriterService = new ExcelWriterService();

                var lista = excelService.Ler(_config.CaminhoExcel);

                foreach (var item in lista)
                {

                    var resultado = await alvaraService.DownloadAlvara(item.Cnpj, item.NomeEmpresa);
                    excelWriterService.EscreverStatus(_config.CaminhoExcel, item.LinhaExcel, resultado.StatusExcel);
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
