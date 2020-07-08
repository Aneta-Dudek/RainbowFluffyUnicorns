using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Text.Json;

namespace Questore.Services
{
    public class QuestService : QuestDAO, IQuestService
    {

        private readonly ISession _session;

        private readonly IStudentDAO _studentDao;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));

        public QuestService(IServiceProvider services, IConfiguration configuration, IStudentDAO studentDao) : base(configuration)
        {
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            _studentDao = studentDao;
        }

        public void ClaimQuest(int id)
        {
            ClaimQuest(id, ActiveStudent.Id);
            UpdateSession();
        }

        private void UpdateSession()
        {
            var updatedStudent = _studentDao.GetStudent(ActiveStudent.Id);
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
        }
    }
}