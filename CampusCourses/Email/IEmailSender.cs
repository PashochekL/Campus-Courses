namespace CampusCourses.Email
{
    public interface IEmailSender
    {
        public Task SendMessage(Message message);
    }
}
