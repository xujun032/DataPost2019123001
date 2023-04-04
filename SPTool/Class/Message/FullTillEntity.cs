using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool
{
  public  class FullTillEntity
    {
        public string  TILL { get; set; }

        public List<string> TILLITEM { get; set; }

        public List<string> TILL_PMNT { get; set; }

        public List<string> TILL_PROMO { get; set; }

        public List<string> TILLITEM_PMNT { get; set; }

        public List<string> TILLITEM_PROMO { get; set; }
    }
}
