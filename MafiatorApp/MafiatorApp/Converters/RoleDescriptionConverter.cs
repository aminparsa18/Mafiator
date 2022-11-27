using Mafiator.Common.Data.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MafiatorApp.Converters
{
    public class RoleDescriptionConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GameRole role)
            {
                return role switch
                {
                    GameRole.Citizen =>
                        "The ordinary citizen, who is usually addressed as the same citizen, has no special duty. In the morning, like everyone else, he participates in decisions, he can talk and vote. He sleeps at night and has nothing to do. Some people like this role, because they can use all their thoughts without stress and look for the Mafia. Some even say that they do not have this dull and friendly role",
                    GameRole.Detective =>
                        "The detective is a citizen. The detective can point to one of the people he thinks is the Mafia at any time of the night. This role is very attractive in this game because if the detective can know the identities of different people in the game, citizens can get to know each other and unite against the Mafia group. ",
                    GameRole.Doctor =>
                        "The role of the doctor is among the citizens. The doctor can save one of the people who killed the Mafia at any time of the night, if diagnosed correctly. Of course, the doctor can save himself once during the game.",
                    GameRole.GodFather =>
                        "The most important role among the members of the Mafia belongs to the godfather. This person is the final decision maker in the game and can, in consultation with other members of the Mafia, select one of the citizens and remove him from the game by firing bullets. It should be noted, the godfather inquiry is negative. This means that if the detective asks God at night to confirm in order to identify the role of the godfather, God can not guide him and give a positive inquiry.",
                    GameRole.Gun =>
                        "Gan is a member of the citizen group who has a few bullets with him. In the night stage, he can shoot anyone he wants. Anyone with an arrow can shoot at anyone they want during the morning phase. It is clear that Gan should try to shoot one of the members of the citizen group, and the one who shoots should shoot the one who thinks he is a mafia. The number of shots that Gan can give is agreed upon at the beginning of the game. Usually two bullets are enough for a gunman in most Mafia games.",
                    GameRole.Terrorist =>
                        "The terrorist has the ability to take one of the people in the game out of the game with him. In contrast to the terrorist in the citizen group, there is a devoted character that was described above in the citizen maps.The terrorist must use his ability as long as he is alive.This means that if a skeptic gets a high vote in the voting stage and then goes to the murder voting stage and it is determined that he is going to be executed, he can no longer get up and take someone out of the game with him.The reason is very clear, because it is not clear whether he will survive or not!",
                    GameRole.Sniper =>
                        "A sniper is a member of a citizen group who shoots himself and can shoot at people he thinks are mafia. He does this in the night stage and the result of his work will be announced in the next morning stage. The number of bullets that the sniper has is also determined by agreement before the start of the game.",
                    GameRole.Immortal =>
                        "Immortal is one of the citizens of the Mafia game who lives for 24 hours and then dies as a result of any kind of death. Any kind of death means all the situations that may cause a player to leave the game. Including: death sentence in voting, mafia assassination, shooting with snipers, suicide assassination by a terrorist and all other cases of death in the Mafia game. ",
                    GameRole.Natasha =>
                        "She is a member of the Mafia. His job is to silence someone every night for the next day. He can even silence members of his team or even himself. But no one can be silenced more than once. The rest of his duties are like all other mafias. Like everyone else, he participates in his group's decision to kill people at night. ",
                    GameRole.Priest =>
                        "A citizen priest who plays against Silencer or Natasha. That is, if the priest chooses the person who has been silenced by Natasha correctly, that person will not be silent and can still speak in the game. If the priest makes the wrong choice, in some games it is the rule that the person takes 'extra speech'. That means he can talk more than the allotted time for each player.",
                    GameRole.Mafia =>
                        "This mafia has no other duty in the mafia group except the role of the mafia. Like everyone else, he has the right to speak, to vote, to participate in Mafia decision-making during the night stage of the game, but he has no other work to do.",
                    GameRole.Judge =>
                        "After the verdict of the murders was determined and it was determined which of the two suspected players was to be executed, Gad issued an order to stay the night. The first person to wake up is the judge. The judge is one of the citizens who has the ability to reverse the results of the day's voting. That is, to replace the person selected for execution with the person who was on the murder vote but did not vote. The number of times a judge can do this should be limited. But it is better to have more than one so that the judge does not use this feature only for himself.",
                };
            }

            return "Unknown Character";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}