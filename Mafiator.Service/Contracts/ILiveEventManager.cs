using System;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts;

public interface ILiveEventManager
{
    Task<Tuple<string, string>> CreateLiveEvent(string liveEventName);
}
