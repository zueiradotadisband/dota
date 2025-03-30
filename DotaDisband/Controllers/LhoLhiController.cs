using Microsoft.AspNetCore.Mvc;

namespace DotaDisband.Controllers
{
    public class LhoLhiController : Controller
    {
        private const int ItensPorPagina = 5;  

        public IActionResult Index(int pagina = 1)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/db/bdpalavras.txt");

            List<string> palavras = new List<string>();

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            palavras.Add(linha);
                        }
                    }
                }
                else
                {
                    TempData["Mensagem"] = "Arquivo não encontrado.";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = $"Erro ao ler o arquivo: {ex.Message}";
            }

            int totalPaginas = (int)System.Math.Ceiling((double)palavras.Count / ItensPorPagina);

            var palavrasPagina = palavras.Skip((pagina - 1) * ItensPorPagina).Take(ItensPorPagina).ToList();

            ViewData["Palavras"] = palavrasPagina;
            ViewData["PaginaAtual"] = pagina;
            ViewData["TotalPaginas"] = totalPaginas;

            return View();
        }

        [HttpPost]
        public IActionResult SalvarPalavra(string palavra)
        {            
            if (string.IsNullOrEmpty(palavra))
            {
                TempData["Mensagem"] = "Por favor, insira uma palavra válida.";
                return RedirectToAction("Index");
            }

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/db/bdpalavras.txt");

            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, append: true))
                {
                    sw.WriteLine(palavra); 
                }
                TempData["Palavra"] = palavra;
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = $"Erro ao salvar a palavra: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
