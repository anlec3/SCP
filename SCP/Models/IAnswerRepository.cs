namespace SCP.Models
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        void Save();
    }
}
