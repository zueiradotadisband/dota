using Microsoft.AspNetCore.Mvc;

namespace DotaDisband.Controllers
{
    public class LhoLhiController : Controller
    {
        private const int ItensPorPagina = 5;  // Número de palavras por página

        // Método para ler o arquivo e retornar as palavras com paginação
        public IActionResult Index(int pagina = 1)
        {
            // Caminho para o arquivo
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/db/bdpalavras.txt");

            // Lista para armazenar as palavras lidas
            List<string> palavras = new List<string>();

            try
            {
                // Verificar se o arquivo existe
                if (System.IO.File.Exists(filePath))
                {
                    // Usando StreamReader para ler o arquivo linha por linha
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            palavras.Add(linha); // Adiciona cada linha na lista
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

            // Paginação: Calcular o número total de páginas
            int totalPaginas = (int)System.Math.Ceiling((double)palavras.Count / ItensPorPagina);

            // Pega os itens da página solicitada
            var palavrasPagina = palavras.Skip((pagina - 1) * ItensPorPagina).Take(ItensPorPagina).ToList();

            // Passa a lista de palavras e os dados de paginação para a View
            ViewData["Palavras"] = palavrasPagina;
            ViewData["PaginaAtual"] = pagina;
            ViewData["TotalPaginas"] = totalPaginas;

            return View();
        }

        [HttpPost]
        public IActionResult SalvarPalavra(string palavra)
        {
            // Verifique se a palavra foi recebida
            if (string.IsNullOrEmpty(palavra))
            {
                TempData["Mensagem"] = "Por favor, insira uma palavra válida.";
                return RedirectToAction("Index");
            }

            // Caminho para o arquivo
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/db/bdpalavras.txt");

            try
            {
                // O arquivo já existe, então adicione a nova palavra na próxima linha
                using (StreamWriter sw = new StreamWriter(filePath, append: true))
                {
                    sw.WriteLine(palavra); // Adiciona a palavra na próxima linha
                }
               
                // Armazenando a palavra no TempData para exibir na página
                TempData["Palavra"] = palavra;
            }
            catch (Exception ex)
            {
                // Caso haja algum erro, armazene a mensagem de erro
                TempData["Mensagem"] = $"Erro ao salvar a palavra: {ex.Message}";
            }

            // Redireciona de volta para a página inicial
            return RedirectToAction("Index");
        }
    }
}
