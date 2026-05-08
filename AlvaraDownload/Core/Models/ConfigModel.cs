using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlvaraDownload.Core.Models
{
    public class ConfigModel
    {
        public string CaminhoExcel { get; set; } = string.Empty;
        public string PastaDownload { get; set; } = string.Empty;
        public string UrlPortal { get; set; } = string.Empty;
        public bool Headless { get; set; }
    }
}
