using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Common.Config
{
    public class EncryptionSettings
    {
        public string VoucherEncryptionSalt { get; set; }
        public int VoucherEncryptionIteration { get; set; }
        public string VoucherEncryptionInitialVector { get; set; }
        public int VoucherEncryptionKeySize { get; set; }
        public string Salt => VoucherEncryptionSalt;
    }
}
