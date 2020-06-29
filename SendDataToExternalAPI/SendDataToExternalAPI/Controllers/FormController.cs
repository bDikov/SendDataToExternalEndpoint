using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SendDataToExternalAPI.Services.ModelsDTO;
using SendDataToExternalAPI.Services.Services.Contracts;


namespace SendDataToExternalAPI.Web.Controllers
{
    public class FormController : Controller
    {
        private readonly IFormService formService;
        private readonly IToastNotification toastNotification;
        public FormController(IFormService formService, IToastNotification toastNotification)
        {
            this.formService = formService;
            this.toastNotification = toastNotification;
        }

        public async Task<IActionResult> CreateForm()
        {
            return await Task.Run(() => View());
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendForm(FormDTO data)
        {
            if (ModelState.IsValid)
            {
                var isSend = false;

                while (isSend != true)
                {
                    isSend = await formService.SendForm(data);
                }

                this.toastNotification.AddSuccessToastMessage("The form is send successfuly!");

                return RedirectToAction("CreateForm");
            }
            this.toastNotification.AddErrorToastMessage("Invalid Email");
            return View("CreateForm");
        }
    }
}
