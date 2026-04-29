using Helpdesk.Utils;
using System;
using Microsoft.Playwright;





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

                var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });

                if (exitCode != 0)
                {
                    throw new Exception("Falha ao instalar o Chromium do Playwright.");
                }

                Robot robot = new Robot();
                await robot.Executar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar o robô: {ex.Message}");
            }
        }
    }
}