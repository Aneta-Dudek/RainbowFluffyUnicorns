using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Questore.Services
{
    public class QuestService : IQuestService
    {

        private readonly ISession _session;

        private readonly IStudentDAO _studentDao;

        private readonly IQuestDAO _questDao;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));

        public QuestService(IServiceProvider services, IStudentDAO studentDao, IQuestDAO questDao)
        {
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            _studentDao = studentDao;
            _questDao = questDao;
        }

        public IEnumerable<Quest> GetQuests()
        {
            return _questDao.GetQuests().ToList();

        }

        public void ClaimQuest(int id)
        {
            _questDao.ClaimQuest(id, ActiveStudent.Id);
            UpdateSession();
        }

        private void UpdateSession()
        {
            var updatedStudent = _studentDao.GetStudent(ActiveStudent.Id);
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
        }
    }
}