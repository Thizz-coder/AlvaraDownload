using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Playwright;
using AlvaraDownload.Core.Rules;
using AlvaraDownload.Core.Models;


namespace AlvaraDownload.Services.Web
{
    class AlvaraService
    {

        private const string TextoAlvaraPessoaJuridica = "Alvará Pessoa Juridica";
        private const string TextoBotaoImprimir = "Imprimir Imprimir";


        private const string SeletorInputCnpj = "#ctl00_ContentBody_painelPesquisaCpfCmc_tbCpf_I";
        private const string SeletorLinhasLancamento = "[id*='gvLancamentos_DXDataRow']";
        private const string SeletorPrimeiroLancamento = "#ctl00_ContentBody_callbackPanelGeral_gvLancamentos_DXSelBtn0_D";
        private const string SeletorPrimeiraParcela = "#ctl00_ContentBody_callbackPanelGeral_gvParcelas_DXSelBtn0_D";


        private readonly ConfigModel _config;

        public AlvaraService(ConfigModel config)
        {
            _config = config;
        }



        public async Task<ResultadoDownloadAlvaraModel> DownloadAlvara(string cnpj, string nomeEmpresa)
        {
            try
            {
                cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

                using var playwright = await Playwright.CreateAsync();

                await using var browser = await playwright.Chromium.LaunchAsync(new()
                {
                    Headless = _config.Headless,
                });

                var context = await browser.NewContextAsync(new()
                {
                    ViewportSize = new ViewportSize
                    {
                        Width = 1920,
                        Height = 1080
                    }
                });

                var page = await context.NewPageAsync();

                await page.GotoAsync(_config.UrlPortal);
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                await page.GetByText(TextoAlvaraPessoaJuridica).ClickAsync();
                
                Thread.Sleep(6000);
                
                await page.Locator(SeletorInputCnpj).FillAsync(cnpj);
                
                Thread.Sleep(6000);

                await page.Locator("span").Filter(new() { HasText = "Pesquisa" }).ClickAsync();

                Thread.Sleep(6000);
                
                await page.GetByRole(AriaRole.Link, new() { Name = "Selecionar" }).ClickAsync();

                Thread.Sleep(6000);


                await page.WaitForSelectorAsync(SeletorLinhasLancamento);

                var rows = page.Locator(SeletorLinhasLancamento);
                int count = await rows.CountAsync();

                
                if(count == 0)
                {
                    return new ResultadoDownloadAlvaraModel
                    {
                        Sucesso = true,
                        StatusExcel = AlvaraRules.StatusNenhumEmAberto

                    };
                }

                var primeiraSituacao = (await rows.First.Locator("td").Nth(3).InnerTextAsync()).Trim();

                bool primeiraLinhaEmAberto = AlvaraRules.EstaEmAberto(primeiraSituacao);
                bool existeOutroEmAberto = false;

                for (int i= 1 ; i < count; i++)
                {
                    var situacao = (await rows.Nth(i).Locator("td").Nth(3).InnerTextAsync()).Trim();
                    if (AlvaraRules.EstaEmAberto(situacao))
                    {
                        existeOutroEmAberto = true;
                        break;
                    }
                }
                if (!primeiraLinhaEmAberto)
                {
                    return new ResultadoDownloadAlvaraModel
                    {
                        Sucesso = true,
                        StatusExcel = existeOutroEmAberto
                            ? AlvaraRules.StatusExisteOutroEmAbertoMasPrimeiraNao
                            : AlvaraRules.StatusNenhumEmAberto
                    };
                }



                await page.Locator(SeletorPrimeiroLancamento).ClickAsync();
                await page.Locator(SeletorPrimeiraParcela).ClickAsync();

                var download = await page.RunAndWaitForDownloadAsync(async () =>{await page.GetByText(TextoBotaoImprimir).ClickAsync();});

                var fileName = $"{nomeEmpresa}_{cnpj}.pdf";
                Directory.CreateDirectory(_config.PastaDownload);
                await download.SaveAsAsync(Path.Combine(_config.PastaDownload, fileName));

                return new ResultadoDownloadAlvaraModel
                {
                    Sucesso = true,
                    StatusExcel = existeOutroEmAberto
                            ? AlvaraRules.StatusDownloadConcluidoComOutros
                            : AlvaraRules.StatusDownloadConcluido
                };



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao baixar alvará para CNPJ {cnpj}: {ex.Message}");
                return new ResultadoDownloadAlvaraModel
                {
                    Sucesso = false,
                    QuantidadeEmAberto = 0,
                    StatusExcel = AlvaraRules.StatusErro
                };
            }

        }

    }
}
