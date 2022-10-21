using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using TrabalhoCriptografia.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TrabalhoCriptografia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("Index", "");
        }
        public JsonResult Criptografar(string texto, string key)
        {
            var result = new CriptografiaHelper().Criptografar(texto, key);
            return Json(result);
        }

        public JsonResult Descriptografar(string texto, string key)
        {
            var result = new CriptografiaHelper().Descriptografar(texto, key);
            return Json(result);
        }
        private List<char> ConvertStringToBitArray(string text)
        {
            var bits = Encoding.UTF8.GetBytes(text)
                .SelectMany(@byte => Convert.ToString(@byte, 2).PadLeft(8, '0').ToCharArray());

            return bits.ToList();
        }

        public string Teste()
        {
            string a = new CriptografiaHelper().Criptografar("texto a ser criptografado", "pedrochave");

            string b = new CriptografiaHelper().Descriptografar(a, "pedrochave");

            return b;
        }

    }
}