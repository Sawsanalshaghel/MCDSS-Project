using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahp_cbr_app
{
    public class ProgressUpdatedEventArgs
    {
        public int Value { get; set; }
        public int Maximum { get; set; }
        public string Text { get; set; }
    }
}
