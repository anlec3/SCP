namespace SCP.Models
{
    public interface IQuestionRepository : IRepository<Question>
    {
        void Save();
    }
}
