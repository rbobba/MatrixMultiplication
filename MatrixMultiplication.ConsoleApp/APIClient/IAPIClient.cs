using System.Threading.Tasks;

namespace MatrixMultiplication.ConsoleApp
{
    public interface IAPIClient
    {
        
        Task<T> GetAsync<T>(string uri);
        Task<T> PostAsync<T>(string uri, T data);

        
    }
}
