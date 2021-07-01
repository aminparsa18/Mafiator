using System;
using System.Collections.Generic;

namespace Mafiator.Data.Dtos
{
    public class ControllerDto
    {
        public string AreaName { get; set; }
        public IList<Attribute> ControllerAttributes { get; set; }
        public string ControllerDisplayName { get; set; }
        public string ControllerId => $"{AreaName}:{ControllerName}";
        public string ControllerName { get; set; }
        public IList<ActionDto> MvcActions { get; set; } = new List<ActionDto>();
    }
}
