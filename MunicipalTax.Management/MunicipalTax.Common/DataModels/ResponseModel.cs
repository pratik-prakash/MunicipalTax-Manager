namespace MunicipalTax.Common.DataModels
{
    public class ResponseModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="payload"></param>
        public ResponseModel(bool success, string message, object payload)
        {
            this.Success = success;
            this.Message = message;
            this.Payload = payload;
        }

        /// <summary>
        /// Success status.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Status message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Response data.
        /// </summary>
        public object Payload { get; set; }
    }
}
