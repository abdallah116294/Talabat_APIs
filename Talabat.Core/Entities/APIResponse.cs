﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class APIResponse<T>
    {
        public string Status { get; set; }
        public T Data { get; set; }
    }
}
