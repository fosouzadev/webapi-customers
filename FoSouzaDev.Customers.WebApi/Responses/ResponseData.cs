namespace FoSouzaDev.Customers.WebApi.Responses
{
    public sealed class ResponseData<T>
    {
        public ResponseData(T? data)
        {
            this.Data = data;
        }

        public ResponseData(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public T? Data { get; private set; }
        public string? ErrorMessage { get; private set; }
    }
}