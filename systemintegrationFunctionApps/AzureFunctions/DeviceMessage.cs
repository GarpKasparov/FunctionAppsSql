using System;
using System.Collections.Generic;
using System.Text;

namespace systemintegrationFunctionApps.AzureFuntions
{
    class DeviceMessage
    {
        public float temperature { get; set; }
        public float humidity { get; set; }
        public long epochtime { get; set; }
      
        public int id { get; set; }
    }
}
