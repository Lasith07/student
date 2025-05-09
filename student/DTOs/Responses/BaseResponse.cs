using System;

namespace DemoAPI.DTOs.Responses
{
    public class BaseResponse
    {
       
        public int status_code { get; set; }
        public object? data { get; set; }
        public MessageDTO? messageDTO { get; set; }

        
        public BaseResponse()
        {
           
        }

        
        public BaseResponse(int status_code, object? data)
        {
            this.status_code = status_code;
            this.data = data ?? throw new ArgumentNullException(nameof(data));  
        }

        
        public BaseResponse(int status_code, MessageDTO? messageDTO)
        {
            
            this.messageDTO = messageDTO ?? throw new ArgumentNullException(nameof(messageDTO));  
        }
    }
}
