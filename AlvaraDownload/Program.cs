using Helpdesk.Utils;
using System;

namespace AlvaraDownload
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            LogWriter logWriter = new LogWriter();
            logWriter.WriteLog("-----------------------------------IniciandoExecução-----------------------------------");

            Robot robot = new Robot();
            await robot.Executar();
        }
    }
}