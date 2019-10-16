using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Pei.BolUploader.Entities
{
    public partial class BolItem
    {
        public BolItem()
        {
            Images = new List<BolImage>();
            UploadDate = DateTime.Now;
        }
        public string BolNumber { get; set; }
        public String DocType { get; set; }
        public DateTime UploadDate { get; set; }
        public String UploadBy { get; set; }

        public List<BolImage> Images { get; set; }
    }
    public class BolImage
    {
        public string BolNumber { get; set; }
        public int Order { get; set; }
        public String Data { get; set; }
    }

    public class BolUploadJob
    {
        public Guid JobId { get; set; }
        public DateTime CreateDate { get; set; }
        public String Status { get; set; }
        public String Message { get; set; }
        public String BusinessKey { get; set; }
        /// <summary>
        /// indicate which step is processing
        /// </summary>
        public int ProcessIndex { get; set; }
    }
}
