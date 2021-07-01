using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos
{
    public class ActionDto
    {
        public IList<Attribute> ActionAttributes { get; set; }
        public string ActionDisplayName { get; set; }
        public string ActionId => $"{ControllerId}:{ActionName}";
        public string ControllerId { get; set; }
        public string ActionName { get; set; }
        public bool IsSecuredAction { get; set; }
    }
}
