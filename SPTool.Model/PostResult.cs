using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool.Model
{
    [Serializable]
    public partial class PostResult
    {
        private string _msg;
        private int? _result;
        /// <summary>
        /// 
        /// </summary>
        public string Msg
        {
            set { _msg= value; }
            get { return _msg; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Result
        {
            set { _result = value; }
            get { return _result; }
        }
    }
}
