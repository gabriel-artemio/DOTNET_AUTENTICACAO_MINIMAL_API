using mvc_estacionamento.DAL;
using mvc_estacionamento.Models;
using System.Web.Mvc;

namespace mvc_estacionamento.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vagasDal = new VagasDAL();
            var veiculosDal = new VeiculoDAL();

            var vm = new ModelsHome
            {
                StatusVagas = vagasDal.ListarStatusVagas(),
                Veiculos = veiculosDal.ListarVeiculos(),
                Vagas = vagasDal.ListarVagas()
            };

            return View(vm);
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

        [HttpPost]
        public ActionResult RegistrarEntrada(Veiculo veiculo)
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