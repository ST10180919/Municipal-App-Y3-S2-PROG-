﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.ViewModels
{
    internal class RequestDetailsViewModel : ViewModelBase
    {
        public ReportViewModel ServiceRequest { get; }
        public RequestDetailsViewModel(ReportViewModel serviceRequest)
        {
            ServiceRequest = serviceRequest;
        }
    }
}
