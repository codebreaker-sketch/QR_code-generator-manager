

using System;

namespace qr_website.Models
{
    public class QRCodeEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Data { get; set; } = string.Empty;
        public string ImageBase64 { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public QRCodeType Type { get; set; } = QRCodeType.Generic;
    }

}
