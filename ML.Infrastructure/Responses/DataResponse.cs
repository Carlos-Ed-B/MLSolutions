using ML.Infrastructure.Enums;
using Newtonsoft.Json;

namespace ML.Infrastructure.Responses
{
    public class DataResponse<T>
    {
        public T Content { get; set; }
        public string Message { get; set; }
        public CustomTypeResultEnum TypeResult { get; set; }

        public DataResponse(string content)
        {
            var res = JsonConvert.DeserializeObject<DataResponse<T>>(content);
            this.Content = res.Content;
            this.Message = res.Message;
            this.TypeResult = res.TypeResult;
        }
        public DataResponse() { }
    }
}
