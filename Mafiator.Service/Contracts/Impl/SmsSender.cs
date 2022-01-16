using SmsIrRestfulNetCore;

namespace Mafiator.Service.Contracts.Impl
{
    public class SmsSender : ISmsSender
    {
        //public async Task<string> SendAuthSmsAsync(string code, string phoneNumber)
        //{
        //    var api = new KavenegarApi("4974664138656646584B6635424B48625A54386439546B4F36733051537150736B3263384B3144496D47453D");
        //   var result= await api.Send("1000596446", phoneNumber, code);
        //   return result.StatusText;
        //}

        public string SendAuthSmsAsync(string code, string phoneNumber)
        {
            var restVerificationCodeRespone = new VerificationCode()
                .Send(new Token().GetToken("b49a501e1d1aeeb6860e43a1","456charlie36!"),
                    new RestVerificationCode()
                    {
                        Code = code,
                        MobileNumber = phoneNumber
                    });
            return restVerificationCodeRespone.Message;
        }
    }
}
