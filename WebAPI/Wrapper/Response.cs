namespace WebAPI.Wrapper;

public class Response<T>
{

    public T Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public IEnumerable<string> Errors { get; set; }
    public Response()
    {
        
    }

    public Response(T data)
    {
        Data = data;
        Succeeded = true;
    }
}
