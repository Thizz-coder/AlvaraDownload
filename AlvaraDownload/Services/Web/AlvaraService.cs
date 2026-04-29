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
        public async Task<ResultadoDownloadAlvaraModel> DownloadAlvara(string cnpj, string nomeEmpresa)
        {
            try
            {
                cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

                using var playwright = await Playwright.CreateAsync();

                await using var browser = await playwright.Chromium.LaunchAsync(new()
                {
                    Headless = false
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

                await page.GotoAsync("https://www.blumenau.sc.gov.br/cidadao/");
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                await page.GetByText("Alvará Pessoa Juridica").ClickAsync();
                
                Thread.Sleep(6000);
                
                await page.Locator("#ctl00_ContentBody_painelPesquisaCpfCmc_tbCpf_I").FillAsync(cnpj);
                
                Thread.Sleep(6000);

                await page.Locator("span").Filter(new() { HasText = "Pesquisa" }).ClickAsync();

                Thread.Sleep(6000);
                
                await page.GetByRole(AriaRole.Link, new() { Name = "Selecionar" }).ClickAsync();

                Thread.Sleep(6000);


                await page.WaitForSelectorAsync("[id*='gvLancamentos_DXDataRow']");

                var rows = page.Locator("[id*='gvLancamentos_DXDataRow']");
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
                        StatusExcel = AlvaraRules.StatusNenhumEmAberto
                    };
                }


                await page.Locator("#ctl00_ContentBody_callbackPanelGeral_gvLancamentos_DXSelBtn0_D").ClickAsync();
                await page.Locator("#ctl00_ContentBody_callbackPanelGeral_gvParcelas_DXSelBtn0_D").ClickAsync();

                var download = await page.RunAndWaitForDownloadAsync(async () =>{await page.GetByText("Imprimir Imprimir").ClickAsync();});

                var fileName = $"{nomeEmpresa}_{cnpj}.pdf";
                await download.SaveAsAsync(Path.Combine("C:\\Temp_Alvara\\", fileName));

                return new ResultadoDownloadAlvaraModel
                {
                    Sucesso = true,
                    StatusExcel = existeOutroEmAberto
                            ? AlvaraRules.StatusDownloadConcluidoComOutros
                            : AlvaraRules.StatusDownloadConcluido
                };













                //if (count == 0)
                //    return;
                //Thread.Sleep(2000);
                //// 🔥 PRIMEIRA LINHA
                //var primeiraSituacao = (await rows.First
                //    .Locator("td")
                //    .Nth(3)
                //    .InnerTextAsync())
                //    .Trim();

                //bool baixar = AlvaraRules.EstaEmAberto(primeiraSituacao);
                //Thread.Sleep(2000);
                //// 🔍 RESTANTE
                //bool existeOutro = false;

                //for (int i = 1; i < count && !existeOutro; i++)
                //{
                //    var situacao = (await rows.Nth(i).Locator("td").Nth(3).InnerTextAsync()).Trim();

                //    existeOutro = AlvaraRules.EstaEmAberto(situacao);
                //}
                //Thread.Sleep(2000);
                // 🚀 EXECUÇÃO
                //if (baixar)
                //{
                //    await page.Locator("#ctl00_ContentBody_callbackPanelGeral_gvLancamentos_DXSelBtn0_D").ClickAsync();
                //    await page.Locator("#ctl00_ContentBody_callbackPanelGeral_gvParcelas_DXSelBtn0_D").ClickAsync();
                //    var download = await page.RunAndWaitForDownloadAsync(async () =>
                //    {
                //        await page.GetByText("Imprimir Imprimir").ClickAsync();
                //    });

                //    var fileName = $"`{nomeEmpresa}_{cnpj}.pdf";

                //    Thread.Sleep(2000);
                //    await download.SaveAsAsync(Path.Combine("C:\\Temp_Alvara\\", fileName));
                //    await page.CloseAsync();
                // }


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
