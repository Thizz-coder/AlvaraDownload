using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlvaraDownload.Core.Rules
{
    public static class AlvaraRules
    {
        public static bool EstaEmAberto(string situacao)
        {
            return !string.IsNullOrWhiteSpace(situacao)
                && situacao.Contains("ATIVO/EM ABERTO");
        }
    }
}
