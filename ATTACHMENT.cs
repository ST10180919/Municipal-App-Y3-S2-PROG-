//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Municipal_App
{
    using System;
    using System.Collections.Generic;
    
    public partial class ATTACHMENT
    {
        public int ID { get; set; }
        public string NAME_OF_FILE { get; set; }
        public byte[] FILE_BINARY { get; set; }
        public Nullable<int> ISSUE_REPORT_ID { get; set; }
    
        public virtual ISSUE_REPORT ISSUE_REPORT { get; set; }
    }
}