using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseOperation.DBContract
{
    public class ParameterReturnValue
    {
        private string _ParaName;
        private object _ParaValue;
        private int _ParaIndex;

        public ParameterReturnValue()
        {
            _ParaName = "";
            _ParaValue = "";
            _ParaIndex = -1;
        }

        public string ParameterName
        {
            get { return _ParaName; }
            set { _ParaName = value; }
        }

        public object ParameterValue
        {
            get { return _ParaValue; }
            set { _ParaValue = value; }
        }

        public int ParameterIndex
        {
            get { return _ParaIndex; }
            set { _ParaIndex = value; }
        } 


    }
}
