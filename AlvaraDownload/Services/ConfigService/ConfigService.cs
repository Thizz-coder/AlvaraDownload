using AlvaraDownload.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlvaraDownload.Services.ConfigService
{
    public class ConfigService
    {
        public ConfigModel Ler()
        {
            var caminhoConfig = Path.Combine(AppContext.BaseDirectory, "config.json");

            if (!File.Exists(caminhoConfig))
            {
                throw new FileNotFoundException("Arquivo config.json. não encontrado", caminhoConfig);

            }
            var json = File.ReadAllText(caminhoConfig);
            var config = JsonSerializer.Deserialize<ConfigModel>(json);

            if (config == null)

            {
                throw new Exception("Não foi possível carregar o config.json");
            }

            var baseDir = AppContext.BaseDirectory;

            if (!Path.IsPathRooted(config.CaminhoExcel))
            {
                config.CaminhoExcel = Path.Combine(baseDir, config.CaminhoExcel);
            }

            if (!Path.IsPathRooted(config.PastaDownload))
            {
                config.PastaDownload = Path.Combine(baseDir, config.PastaDownload);
            }

            return config;

        }
    }
}
