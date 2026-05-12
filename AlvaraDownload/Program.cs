using Helpdesk.Utils;
using System;
using Microsoft.Playwright;
using AlvaraDownload.Services.ConfigService;





namespace AlvaraDownload
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                LogWriter logWriter = new LogWriter();
                logWriter.WriteLog("-----------------------------------IniciandoExecução-----------------------------------");

                Console.WriteLine("ROBÔ EM EXECUÇÃO");
                
                var configService = new ConfigService();
                var config = configService.Ler();

                var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });

                if (exitCode != 0)
                {
                    throw new Exception("Falha ao instalar o Chromium do Playwright.");
                }
                
                
                Robot robot = new Robot(config);
                await robot.Executar();

                Console.WriteLine("FIM DE EXECUÇÃO");
                logWriter.WriteLog("-----------------------------------FimExecução-----------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar o robô: {ex.Message}");
            }
        }
    }
}