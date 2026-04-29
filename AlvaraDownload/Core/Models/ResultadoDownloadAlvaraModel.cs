using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlvaraDownload.Core.Models
{
    public class ResultadoDownloadAlvaraModel
    {
       
            public bool Sucesso { get; set; }
            public int QuantidadeEmAberto { get; set; }
            public int? IndicePrimeiroEmAberto { get; set; }
            public string StatusExcel { get; set; } = string.Empty;  

    }
}
