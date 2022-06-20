using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beis.HelpToGrow.Common.Tests
{
    [TestFixture]
    internal class EncryptionServiceTests

    {

        private IEncryptionService _encryptionService;
        private IOptions<EncryptionSettings> _encryptionSettings; 
    

        [SetUp]
        public void Setup()
        {

            _encryptionSettings = Options.Create<EncryptionSettings>(new EncryptionSettings
            {

                VoucherEncryptionInitialVector = "AVRlp67e*baz44sR",
                VoucherEncryptionIteration = 2,
                VoucherEncryptionKeySize = 256,
                VoucherEncryptionSalt = "Dkjkjjkjii"
            });


            _encryptionService = new EncryptionService(_encryptionSettings);
        }


        [TestCase("NNwl_zr2Q-cCOzYTbD3gWLJE_XwXQB-vehgjgTsnNp-wM_1yhaBhbauKu4-K7m_avCzR3x86MCMW9srFJTArc05--NZyrSpp4o1Tx59jYBQzERPq3Rhln-8PYE_ZpOhS", "4488BE15-3774-4E03-AF06-0B2ACE41F345")]
        public void DecryptVoucher(string voucherCode, string salt)
        {
        
            var result = _encryptionService.Decrypt(voucherCode, salt);
            Assert.That(result, Is.EqualTo("{\"EnterpriseId\":1223,\"EmailAddress\":\"paul.cripps@integratedweb.solutions\",\"ProductId\":1}"));
        }


        [TestCase("GwQagSleKMG_h_kPwiNLZnAejWJqhCMd0-Xi-aCp9zqqrFgFADpwEvqhqlZvPw53LsuUGWnUgb0P_1fMZWWiJV_VNoZNojVOqWGv0Bs_LuoP-lJGyf4PNjQBXF9gfAoj", "4488BE15-3774-4E03-AF06-0B2ACE41F345")]
        public void DecryptInvalidVoucher(string voucherCode, string salt)
        {
            Assert.Throws<System.Security.Cryptography.CryptographicException>(() => _encryptionService.Decrypt(voucherCode, salt) ); 
        }

        [TestCase("{\"EnterpriseId\":1223,\"EmailAddress\":\"paul.cripps@integratedweb.solutions\",\"ProductId\":1}", "4488BE15-3774-4E03-AF06-0B2ACE41F345")]
        public void EncryptVoucher(string voucherCode, string salt)
        {
            var result = _encryptionService.Encrypt(voucherCode, salt);
            
            
            Assert.That(result, Is.EqualTo("NNwl_zr2Q-cCOzYTbD3gWLJE_XwXQB-vehgjgTsnNp-wM_1yhaBhbauKu4-K7m_avCzR3x86MCMW9srFJTArc05--NZyrSpp4o1Tx59jYBQzERPq3Rhln-8PYE_ZpOhS"));
        }
    }
}
