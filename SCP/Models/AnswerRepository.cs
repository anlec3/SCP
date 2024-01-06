namespace SCP.Models
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private AppDbContext _appDbContext;
        public AnswerRepository(AppDbContext appDbContext) : base(appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
