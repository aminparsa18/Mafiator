using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Mafiator.Service.Contracts.Impl
{
    public class TwilioSmsSender:ISmsSender
    {
        string accountSid = "ACdda3502d854874895054127465efe452";
        string authToken = "e4180b1ad53883ae3c9bee2e4b3c71e9";
        public string SendAuthSmsAsync(string code, string phoneNumber)
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Your verification code: " + code + Environment.NewLine+" Mafiator",
                from: new Twilio.Types.PhoneNumber("+13153337534"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
            return message.Status.ToString();

        }
    }
}
