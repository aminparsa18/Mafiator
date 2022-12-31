using Mafiator.Common.Data.Enums;
using Mafiator.Game.Models.Game;

namespace Mafiator.Game.Helpers;

public class ChatTemplateSelector : DataTemplateSelector
{
    private readonly DataTemplate _incomingTextDataTemplate;
    private readonly DataTemplate _outgoingTextDataTemplate;
    private readonly DataTemplate _incomingVoiceDataTemplate;
    private readonly DataTemplate _outgoingVoiceDataTemplate;
    private readonly DataTemplate _incomingVideoDataTemplate;
    private readonly DataTemplate _outgoingVideoDataTemplate;
    private readonly DataTemplate _incomingLikeDataTemplate;
    private readonly DataTemplate _outgoingLikeDataTemplate;
    private readonly DataTemplate _incomingDissLikeDataTemplate;
    private readonly DataTemplate _outgoingDissLikeDataTemplate;

    public ChatTemplateSelector()
    {
        //_incomingTextDataTemplate = new DataTemplate(typeof(IncomingTextView));
        //_outgoingTextDataTemplate = new DataTemplate(typeof(OutgoingTextView));
        //_incomingVoiceDataTemplate = new DataTemplate(typeof(IncomingVoiceView));
        //_outgoingVoiceDataTemplate = new DataTemplate(typeof(OutgoingVoiceView));
        //_incomingVideoDataTemplate = new DataTemplate(typeof(IncomingVideoView));
        //_outgoingVideoDataTemplate = new DataTemplate(typeof(OutgoingVideoView));
        //_incomingLikeDataTemplate = new DataTemplate(typeof(IncomingLikeView));
        //_outgoingLikeDataTemplate = new DataTemplate(typeof(OutgoingLikeView));
        //_incomingDissLikeDataTemplate = new DataTemplate(typeof(IncomingDissLikeView));
        //_outgoingDissLikeDataTemplate = new DataTemplate(typeof(OutgoingDissLikeView));
    }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is not GameMessageDto message)
            return null;
        if (message.Sender)
        {
            return message.Type switch
            {
                GameMessageType.Text => _outgoingTextDataTemplate,
                GameMessageType.Voice => _outgoingVoiceDataTemplate,
                GameMessageType.Video => _outgoingVideoDataTemplate,
                GameMessageType.Like => _outgoingLikeDataTemplate,
                GameMessageType.DissLike => _outgoingDissLikeDataTemplate,
                _ => null
            };
        }

        return message.Type switch
        {
            GameMessageType.Text => _incomingTextDataTemplate,
            GameMessageType.Voice => _incomingVoiceDataTemplate,
            GameMessageType.Video => _incomingVideoDataTemplate,
            GameMessageType.Like => _incomingLikeDataTemplate,
            GameMessageType.DissLike => _incomingDissLikeDataTemplate,
            _ => null
        };
    }
}