using Android.Runtime;
using Android.Telephony;
using MafiatorApp.Models;

namespace MafiatorApp.Droid
{
    public class PhoneCallDetector:PhoneStateListener
    {
        public override void OnCallStateChanged([GeneratedEnum] CallState state, string phoneNumber)
        {
            base.OnCallStateChanged(state, phoneNumber);
            if(state== CallState.Ringing)
            {
                Xamarin.Forms.MessagingCenter.Send(new PhoneStateModel(), "Ringing");
            }else if (state == CallState.Idle)
            {
                Xamarin.Forms.MessagingCenter.Send(new PhoneStateModel(), "Idle");

            }
        }
    }
}