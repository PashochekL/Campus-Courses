using Quartz;

namespace CampusCourses.Email
{
    public class EmailJob : IJob
    {
        private readonly IEmailSender _emailSender;
        private readonly EmailService _emailService;
        private const int subGroupSize = 20;
        private const int delayBetweenBatchesMs = 5000;

        public EmailJob(IEmailSender emailSender, EmailService emailService)
        {
            _emailSender = emailSender;
            _emailService = emailService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var studentData = await _emailService.GetStudentsEmailsForTomorrowCoursesAsync();

            if (studentData == null || !studentData.Any())
            {
                Console.WriteLine("Нет студентов для отправки уведомлений.");
                return;
            }
            Console.WriteLine($"Отправка писем началась. Всего студентов: {studentData.Count}");

            var groupedByCourse = studentData
                .GroupBy(data => data.CourseName)
                .ToDictionary(group => group.Key, group => group.Select(data => data.Email).ToList());

            foreach (var course in groupedByCourse)
            {
                var courseName = course.Key;
                var emails = course.Value;

                var subGroups = emails
                    .Select((email, index) => new { email, index })
                    .GroupBy(x => x.index / subGroupSize)
                    .Select(group => group.Select(x => x.email).ToList())
                    .ToList();

                foreach (var subGroup in subGroups)
                {
                    var message = new Message(
                        subGroup,
                        "Внимание! Ваш курс начнется завтра.",
                        $"Здравствуйте! Напоминаем, что ваш курс '{courseName}' начнется завтра. Не забудьте!"
                    );

                    try
                    {
                        await _emailSender.SendMessage(message);
                        Console.WriteLine($"Уведомления для подгруппы курса '{courseName}' отправлены ({subGroup.Count} студентов).");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при отправке писем для курса '{courseName}': {ex.Message}");
                    }
                    await Task.Delay(delayBetweenBatchesMs);
                }
            }
            Console.WriteLine("Все письма успешно отправлены.");
        }
    }
}
