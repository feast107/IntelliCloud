using System;
using System.Collections.Generic;

namespace ModelLib
{
    abstract public class Rule : Logics
    {
        protected Rule(文件类型 Type)
        {
            this.Type = Type;
        }

        private readonly 文件类型 Type;
        public 文件类型 匹配类型 => Type;
    }

}
