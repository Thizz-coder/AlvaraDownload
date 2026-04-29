using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlvaraDownload.Core.Rules
{
    public static class AlvaraRules
    {
        public const string StatusDownloadConcluido = "Donwload concluído";
        public const string StatusDownloadConcluidoComOutros = "Donwload concluído, existem outros alvarás em aberto";
        public const string StatusNenhumEmAberto = "Nenhum alvará em situação em aberto";
        public const string StatusErro = "Erro ao processar o download por gentileza verificar manualmente o CNPJ";
        
        public static bool EstaEmAberto(string situacao)
        {
            return !string.IsNullOrWhiteSpace(situacao)
                && situacao.Contains("ATIVO/EM ABERTO");
        }
    }
}
