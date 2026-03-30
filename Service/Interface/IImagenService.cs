namespace ReyesAR.Service.Interface
{
    public interface IImagenService
    {
        Task<string> GuardarImagenAsync(IFormFile file);
    }
}
