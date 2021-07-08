using Android.App;
using Android.Content;
using Android.OS;
using Android.Telephony;
using Java.Lang;

namespace MafiatorApp.Droid
{
    [BroadcastReceiver(Name = "com.parsoft.mafiator.MessageReceiver",Exported = true)]
    [IntentFilter(
        new[] {"android.provider.Telephony.SMS_RECEIVED"})]
    public class SmsReceiver : BroadcastReceiver
    {
        public override void OnReceive(
            Context context, Intent intent)
        {
            if (intent.Action.Equals("android.provider.Telephony.SMS_RECEIVED"))
            {
                Bundle bundle = intent.Extras;           //---get the SMS message passed in---
                SmsMessage[] msgs = null;
                string msg_from;
                if (bundle != null)
                {
                    //---retrieve the SMS message received---
                    try
                    {
                       var pdus = (Object[])bundle.Get("pdus");
                        msgs = new SmsMessage[pdus.Length];
                        for (int i = 0; i < msgs.Length; i++)
                        {
                            msgs[i] = SmsMessage.CreateFromPdu((byte[])pdus[i]);
                            msg_from = msgs[i].OriginatingAddress;
                            var msgBody = msgs[i].MessageBody;
                        }
                    }
                    catch (Exception e)
                    {
                        int a = 2;
                    }
                }
            }
        }
    }
}