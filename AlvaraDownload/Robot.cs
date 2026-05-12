using AlvaraDownload.Core.Models;
using AlvaraDownload.Services.Excel;
using AlvaraDownload.Services.Web;
using Helpdesk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AlvaraDownload
{
    public class Robot
    {
        private readonly ConfigModel _config;
        
        LogWriter logWriter = new LogWriter();
        
        public Robot(ConfigModel config)
        {
            _config = config;
        }

        public async Task Executar()
        {

            try
            { 
                logWriter.WriteLog("INICIANDO LEITURA");

                var excelService = new ExcelReaderService();
                AlvaraService alvaraService = new AlvaraService(_config);
                ExcelWriterService excelWriterService = new ExcelWriterService();

                var lista = excelService.Ler(_config.CaminhoExcel);

                foreach (var item in lista)
                {
                    logWriter.WriteLog($"Processando item {item.NomeEmpresa}_{item.Cnpj}");
                    var resultado = await alvaraService.DownloadAlvara(item.Cnpj, item.NomeEmpresa);
                    excelWriterService.EscreverStatus(_config.CaminhoExcel, item.LinhaExcel, resultado.StatusExcel);
                }
            }
            catch (Exception ex)
            {

                logWriter.WriteLog($"Erro na execução do robô: {ex.Message}");
                
            }
        }
    }
}
