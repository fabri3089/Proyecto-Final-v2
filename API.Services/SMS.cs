using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace API.Services
{
    public class SMS
    {
        public string Execute(string accountSid, string authToken, string senderPhone, string destinationPhone, string textToSend)
        {
            try
            {
                return string.Empty; //Evito ejecutar metodo para evitar enviar SMS

                var twilio = new TwilioRestClient(accountSid, authToken);
                var message = twilio.SendMessage(
                    senderPhone, //"+1 256-305-4229", // From (Twilio number)
                    destinationPhone, //"+5492355677581", // To (Destination phone number)
                    textToSend
                    );

                if (message.RestException != null)
                {
                    var error = message.RestException.Message;
                    return error;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
