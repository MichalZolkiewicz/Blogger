namespace WebAPI.Wrapper;

public class Response<T> : Response
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

public class Response
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }

    public Response()
    {
        
    }

    public Response(bool succeded, string message)
    {
        Succeeded = succeded;
        Message = message;
    }
}
