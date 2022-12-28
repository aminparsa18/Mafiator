using Mafiator.Service.Contracts;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Mafiator.Service.Services;

public class TwilioSmsSender : ISmsSender
{
    private const string AccountSid = "AC2387f4af8183aa627c2536d15ea9bfe2";
    private const string AuthToken = "7671e598cfee7e8d183c647710f39db3";

    public string SendAuthSmsAsync(string code, string phoneNumber)
    {
        TwilioClient.Init(AccountSid, AuthToken);
        var message = MessageResource.Create(
            body: string.Join("Your verification code: ", code, Environment.NewLine, " Mafiator"),
            from: new Twilio.Types.PhoneNumber("+19036021059"),
            to: new Twilio.Types.PhoneNumber(phoneNumber)
        );
        return message.Status.ToString();
    }
}