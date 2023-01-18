using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Controllers
{
    public interface IController
    {
        public abstract Task<IActionResult> get(int? id);

    }
}
