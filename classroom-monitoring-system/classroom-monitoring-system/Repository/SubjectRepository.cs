using classroom_monitoring_system.Interface;
using classroom_monitoring_system.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace classroom_monitoring_system.Repository
{
    public class SubjectRepository
    {
        private readonly IBaseRepository<Subject> _subject;
        public SubjectRepository(IBaseRepository<Subject> subjectRepository)
        {
            _subject = subjectRepository;
        }
        public Results<Subject> GetSubjectById(Guid subjectId)
        {
            var result = new Results<Subject>();
            result.IsSuccess = true;
            result.Data = _subject.GetByCondition(
                x => x.SubjectId == subjectId).FirstOrDefault() ?? new Subject()
                {
                    CreatedDate = DateTime.Now
                };
            return result;
        }
        private Results<List<string>> IsValid(Subject subject)
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(subject.SubjectName))
            {
                errors.Add("Subject Name is required.");
            }
            if (string.IsNullOrEmpty(subject.SubjectDescription))
            {
                errors.Add("Subject description is required.");
            }
            return new Results<List<string>> { IsSuccess = !errors.Any(), Errors = errors };
        }
        public Results<Subject> SaveSubject(Subject subject)
        {
            var result = new Results<Subject>();
            result.IsSuccess = true;
            result.Data = subject;
            result.Errors = new List<string>();
            var validationResult = IsValid(subject);
            if (!validationResult.IsSuccess)
            {
                result.IsSuccess = false;
                result.Errors = validationResult.Errors;
                return result;
            }
            if (subject.SubjectId == Guid.Empty)
            {
                subject.CreatedDate = DateTime.Now;
                result.Data = _subject.Save(subject);
            }
            else
            {
                var dbData = _subject.GetById(subject.SubjectId);
                dbData.SubjectName = subject.SubjectName;
                dbData.SubjectName = subject.SubjectName;
                result.Data = _subject.Update(dbData);
            }
            return result;
        }
        public Results<Subject> DeleteSubject(Guid[] subjectIds)
        {
            var result = new Results<Subject>();
            result.IsSuccess = true;
            result.DeleteCount = 0;
            if (subjectIds != null && subjectIds.Length > 0)
            {
                int deletedCount = _subject.DeleteWithIds(subjectIds, "SubjectId");
                result.DeleteCount = deletedCount;
                result.Message = $"{deletedCount} room type(s) deleted successfully.";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "No selected for deletion.";
            }
            return result;
        }
        public Results<ListScreenModel<SubjectSearchModel>> GetList(PageModel pageModel)
        {
            var result = new Results<ListScreenModel<SubjectSearchModel>>();
            result.IsSuccess = true;

            var search = !string.IsNullOrEmpty(pageModel.Search) 
                ? JsonConvert.DeserializeObject<SubjectSearchModel>(pageModel.Search) 
                : new SubjectSearchModel();

            Expression<Func<Subject, bool>> filter = x =>
                (
                    (string.IsNullOrEmpty(search.SubjectName) || x.SubjectName.Contains(search.SubjectName))
                );

            var listScreen = new ListScreenModel<SubjectSearchModel>()
            {
                Data = _subject.GetAllWithOptionsAndIncludes(pageModel, filter).Cast<object>().ToList(),
                Page = 1,
                PageSize = pageModel.PageSize,
                DataCount = _subject.GetCountWithOptions(pageModel, filter),
                PageMode = pageModel,
                SearchModel = search
            };
            result.Data = listScreen;
            return result;
        }
    }
}
