namespace SCP.Models
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        private AppDbContext _appDbContext;
        public QuestionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
