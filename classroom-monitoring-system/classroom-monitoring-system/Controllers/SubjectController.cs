using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using classroom_monitoring_system.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace classroom_monitoring_system.Controllers
{
    public class SubjectController : Controller
    {
        private readonly SubjectRepository _subject;
        public SubjectController(IBaseRepository<Subject> subjectRepository)
        {
            _subject = new SubjectRepository(subjectRepository);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SubjectListScreen(PageModel pageModel)
        {
            var result = _subject.GetList(pageModel);
            return View(result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult SubjectSearch(SubjectSearchModel subjectSearchModel)
        {
            var pageModel = new PageModel();
            pageModel.Search = JsonConvert.SerializeObject(subjectSearchModel);

            return RedirectToAction("RoomTypeListScreen", "Subject", pageModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public int DeleteSubject([FromBody] Guid[] selected)
        {
            var result = _subject.DeleteSubject(selected);
            return result.DeleteCount;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult SubjectEdit(Subject subject)
        {
            var result = _subject.GetSubjectById(subject.SubjectId);
            var editScreen = new EditScreenModel<Subject>()
            {
                Data = result.Data
            };
            if (subject.SubjectId != Guid.Empty)
            {
                return View(editScreen);
            }

            return View(editScreen);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Save(Subject subject)
        {
            var result = _subject.SaveSubject(subject);
            if (!result.IsSuccess)
            {
                var editScreen = new EditScreenModel<Subject>()
                {
                    Data = result.Data,
                    ErrorMessages = result.Errors ?? new List<string> { }
                };
                return View("SubjectEdit", editScreen);
            }

            return RedirectToAction("SubjectListScreen", "Subject");
        }
    }
}
