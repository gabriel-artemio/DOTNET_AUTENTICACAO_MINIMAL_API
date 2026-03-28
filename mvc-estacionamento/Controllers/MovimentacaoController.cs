using mvc_estacionamento.DAL;
using System.Web.Mvc;

namespace mvc_estacionamento.Controllers
{
    public class MovimentacaoController : Controller
    {
        public ActionResult Index()
        {
            var dal = new MovimentacaoDAL();
            var tickets = dal.ListarMovimentacoes();

            return View(tickets);
        }
    }
}