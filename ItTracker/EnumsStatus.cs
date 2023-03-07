using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTracker
{
    public static class EnumsStatus
    {
        public enum StatusOrder
        {
            Open = 1,
            InWork = 2,
            OnClarify = 3,
            Closed = 4,
            OnConfirmation = 5
        }
        public enum ClarifationMode
        {
            Customer = 1,
            Specialist = 2
        }
    }
}
