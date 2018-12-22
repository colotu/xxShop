using System;
using System.Collections.Generic;

namespace YSWL.OAuth
{
    public class Parameter
    {
        public Parameter(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        private String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String value;

        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public String toParamString()
        {
            return this.name + "=" + this.value;
        }
    }

    /// <summary>
    /// 用于排序参数
    /// </summary>
    public class ParameterComparer : IComparer<Parameter>
    {
        public int Compare(Parameter x, Parameter y)
        {
            if (x.Name == y.Name)
            {
                return string.Compare(x.Value, y.Value);
            }
            else
            {
                return string.Compare(x.Name, y.Name);
            }
        }
    }
}