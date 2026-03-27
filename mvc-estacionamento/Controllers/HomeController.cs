using mvc_estacionamento.DAL;
using mvc_estacionamento.Models;
using System.Web.Mvc;

namespace mvc_estacionamento.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Carros()
        {
            var dal = new VeiculoDAL();
            var carros = dal.ListarVeiculos();

            return View(carros);
        }

        [HttpPost]
        public ActionResult Carros(Veiculo veiculo)
        {
            var dal = new VeiculoDAL();
            dal.Inserir(veiculo);

            return RedirectToAction("Carros");
        }

        public ActionResult Vagas()
        {
            return View();
        }
    }
}