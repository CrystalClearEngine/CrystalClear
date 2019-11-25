using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.EventSystem
{
	public static class Methods
	{
		public static void RaiseEvent(IEvent iEvent)
		{
			iEvent.EventInstance.OnEvent();
		}
	}
}
