using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SendDataToExternalAPI.Services.ModelsDTO;
using SendDataToExternalAPI.Services.Services.Contracts;
using SendDataToExternalAPI.Web.Helpers;

namespace SendDataToExternalAPI.Web.Controllers
{
    public class FormController : Controller
    {
        private readonly OrderChecker _orderChecker;
        private readonly IFormService formService;
        private readonly IToastNotification toastNotification;
        public FormController(IFormService formService, IToastNotification toastNotification, OrderChecker orderChecker)
        {
            this.formService = formService;
            this.toastNotification = toastNotification;
            this._orderChecker = orderChecker;
        }

        public async Task<IActionResult> CreateForm()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForm(FormDTO data)
        {       
            if (ModelState.IsValid)
            {
                var isSend = false;

                while (isSend != true)
                {
                    isSend = await formService.SendForm(data);
                }
                return Accepted(1);              
            }
            return StatusCode(404);         
        }

        [HttpGet("GetUpdateForOrder/{orderNo}")]
        //https://localhost:44354/Form/GetUpdateForOrder/4
        //https://localhost:44354/GetUpdateForOrder?orderNo=4
        //https://localhost:44354/GetUpdateForOrder/4
        public IActionResult GetUpdateForOrder(int orderNo)
        {
            var result = _orderChecker.GetUpdate(orderNo);
            if (result.New)
                return new ObjectResult(result);
            return NoContent();
        }
    }
}
