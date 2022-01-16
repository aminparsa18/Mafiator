using MafiatorApp.Dtos.Game;
using MafiatorApp.Enums;
using MafiatorApp.Views.Templates;
using Xamarin.Forms;

namespace MafiatorApp.Helpers
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate incomingTextDataTemplate;
        private readonly DataTemplate outgoingTextDataTemplate;
        private readonly DataTemplate incomingVoiceDataTemplate;
        private readonly DataTemplate outgoingVoiceDataTemplate;
        private readonly DataTemplate incomingVideoDataTemplate;
        private readonly DataTemplate outgoingVideoDataTemplate;
        private readonly DataTemplate incomingLikeDataTemplate;
        private readonly DataTemplate outgoingLikeDataTemplate;
        private readonly DataTemplate incomingDissLikeDataTemplate;
        private readonly DataTemplate outgoingDissLikeDataTemplate;

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
            if (item is not GameMessageDto message)
                return null;
            if (message.Sender)
            {
                return message.Type switch
                {
                    GameMessageType.Text => outgoingTextDataTemplate,
                    GameMessageType.Voice => outgoingVoiceDataTemplate,
                    GameMessageType.Video => outgoingVideoDataTemplate,
                    GameMessageType.Like => outgoingLikeDataTemplate,
                    GameMessageType.DissLike => outgoingDissLikeDataTemplate,
                    _ => null
                };
            }

            return message.Type switch
            {
                GameMessageType.Text => incomingTextDataTemplate,
                GameMessageType.Voice => incomingVoiceDataTemplate,
                GameMessageType.Video => incomingVideoDataTemplate,
                GameMessageType.Like => incomingLikeDataTemplate,
                GameMessageType.DissLike => incomingDissLikeDataTemplate,
                _ => null
            };
        }

    }
}