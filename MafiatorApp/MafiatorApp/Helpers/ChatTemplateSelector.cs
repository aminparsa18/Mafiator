using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Views.Templates;
using Xamarin.Forms;

namespace MafiatorApp.Helpers
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        readonly DataTemplate incomingTextDataTemplate;
        readonly DataTemplate outgoingTextDataTemplate;
        readonly DataTemplate incomingVoiceDataTemplate;
        readonly DataTemplate outgoingVoiceDataTemplate;
        readonly DataTemplate incomingVideoDataTemplate;
        readonly DataTemplate outgoingVideoDataTemplate;
        readonly DataTemplate incomingLikeDataTemplate;
        readonly DataTemplate outgoingLikeDataTemplate;
        readonly DataTemplate incomingDissLikeDataTemplate;
        readonly DataTemplate outgoingDissLikeDataTemplate;

        public ChatTemplateSelector()
        {
            this.incomingTextDataTemplate = new DataTemplate(typeof(IncomingTextView));
            this.outgoingTextDataTemplate = new DataTemplate(typeof(OutgoingTextView));
            this.incomingVoiceDataTemplate= new DataTemplate(typeof(IncomingVoiceView));
            this.outgoingVoiceDataTemplate = new DataTemplate(typeof(OutgoingVoiceView));
            this.incomingVideoDataTemplate = new DataTemplate(typeof(IncomingVideoView));
            this.outgoingVideoDataTemplate = new DataTemplate(typeof(OutgoingVideoView));
            this.incomingLikeDataTemplate=new DataTemplate(typeof(IncomingLikeView));
            this.outgoingLikeDataTemplate=new DataTemplate(typeof(OutgoingLikeView));
            this.incomingDissLikeDataTemplate=new DataTemplate(typeof(IncomingDissLikeView));
            this.outgoingDissLikeDataTemplate=new DataTemplate(typeof(OutgoingDissLikeView));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is GameMessageDto message))
                return null;
            if (message.Sender)
            {
                switch (message.Type)
                {
                    case GameMessageType.Text:
                        return outgoingTextDataTemplate;
                    case GameMessageType.Voice:
                        return outgoingVoiceDataTemplate;
                    case GameMessageType.Video:
                        return outgoingVideoDataTemplate;
                    case GameMessageType.Like:
                        return outgoingLikeDataTemplate;
                    case GameMessageType.DissLike:
                        return outgoingDissLikeDataTemplate;
                    default: return null;
                }
            }

            switch (message.Type)
            {
                case GameMessageType.Text:
                    return incomingTextDataTemplate;
                case GameMessageType.Voice:
                    return incomingVoiceDataTemplate;
                case GameMessageType.Video:
                    return incomingVideoDataTemplate;
                case GameMessageType.Like:
                    return incomingLikeDataTemplate;
                case GameMessageType.DissLike:
                    return incomingDissLikeDataTemplate;
                default: return null;
            }
        }

    }
}