using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models
{
    public enum SearchCondition
    {
        Same,
        Like
    }
    public class Search
    {
        public Int32? Kdnr { get; set; }
        public SearchCondition KdnrCondition { get; set; } = SearchCondition.Same;
        public String Name { get; set; }
        public String Email { get; set; }
    }
}
