namespace eHandbook.Core.Application.Business.Contracts
{
    public interface IService<T> where T : class
    {
        public Task<T> GetManualByIdAsync(int id);
    }
}