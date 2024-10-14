using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Municipal_App.Stores
{
    internal class FilterStore
    {
        public string SearchText { get; set; }
        public Action OnFilterEvents { get; set; }
    }
}
