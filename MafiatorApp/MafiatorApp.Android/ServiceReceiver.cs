using Android.App;
using Android.Content;
using Android.Telephony;

namespace MafiatorApp.Droid
{
    [BroadcastReceiver(Name = "com.parsoft.mafiator.ServiceReceiver", Enabled = true, Exported = false)]
    [IntentFilter(new[] { TelephonyManager.ActionPhoneStateChanged })]
    public class ServiceReceiver: BroadcastReceiver
    {
        TelephonyManager telephony;

        public override void OnReceive(Context? context, Intent? intent)
        {
            PhoneCallDetector phoneListener = new PhoneCallDetector();
            telephony = (TelephonyManager)context
                .GetSystemService(Context.TelephonyService);
            telephony.Listen(phoneListener, PhoneStateListenerFlags.CallState);
        }

        protected override void Dispose(bool disposing)
        {
            telephony.Listen(null, PhoneStateListenerFlags.None);
            base.Dispose(disposing);
        }
    }
}