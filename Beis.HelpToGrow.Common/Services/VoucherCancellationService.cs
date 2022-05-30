using Beis.HelpToGrow.Repositories.Interfaces;
using Beis.HelpToGrow.Common.Enums;
using Beis.HelpToGrow.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beis.HelpToGrow.Common.Interfaces;
using Beis.HelpToGrow.Persistence.Models;
using Beis.HelpToGrow.Repositories.Enums;

namespace Beis.HelpToGrow.Common.Services
{
    public class VoucherCancellationService : IVoucherCancellationService
    {
        private readonly ILogger<VoucherCancellationService> _logger;
        private readonly IEnterpriseRepository _repository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IVendorCompanyRepository _vendorCompanyRepository;

        public VoucherCancellationService(ILogger<VoucherCancellationService> logger, IEnterpriseRepository repository, ITokenRepository tokenRepository, IProductPriceRepository productPriceRepository, IEncryptionService encryptionService)
        {
            _logger = logger;
            _repository = repository;
            _tokenRepository = tokenRepository;
            _productPriceRepository = productPriceRepository;
            _encryptionService = encryptionService;
        }

        public async Task<CancellationResponse> CancelToken(token token)
        {
            try
            {
                if (token is null)
                {
                    return CancellationResponse.TokenNotFound;
                }

                if (token.cancellation_status_id.HasValue)
                {
                    _logger.LogInformation("The voucher has already been cancelled with status : {status}", (CancellationStatus)token.cancellation_status_id);
                    return token.cancellation_status_id.Value == (int)CancellationStatus.CannotReapply ? CancellationResponse.FreeTrialExpired : CancellationResponse.AlreadyCancelled;
                }

                if (token.redemption_status_id == (long)RedemptionStatus.Unused)
                {
                    if (token.token_expiry < DateTime.Now)
                    {
                        token.cancellation_status_id = (int)CancellationStatus.Expired;
                        await _tokenRepository.UpdateToken(token);
                        return CancellationResponse.TokenExpired;
                    }
                    else
                    {
                        token.cancellation_status_id = (int)CancellationStatus.NotRedeemed;
                        await _tokenRepository.UpdateToken(token);
                        return CancellationResponse.SuccessfullyCancelled;
                    }

                }

                if (token.redemption_status_id == (long)RedemptionStatus.PendingRedemption || token.redemption_status_id == (long)RedemptionStatus.Redeemed)
                {
                    DateTime freeTrialStartDate = token.redemption_date.HasValue ? token.redemption_date.Value : token.token_valid_from;


                    var productPrice = await _productPriceRepository.GetByProductId(token.product);

                    var freePeriodUnits = productPrice.free_trial_term_unit.ToLower().Trim();

                    DateTime trialPeriodEndDate;
                    switch (freePeriodUnits)
                    {
                        case "month":
                        case "months":
                            {
                                trialPeriodEndDate = freeTrialStartDate.AddMonths(productPrice.free_trial_term_no);
                                break;
                            }
                        case "day":
                        case "days":
                            {
                                trialPeriodEndDate = freeTrialStartDate.AddDays(productPrice.free_trial_term_no);
                                break;
                            }
                        default:
                            {
                                _logger.LogInformation("Unable to calculate free trial period. The product has no free trial term unit");
                                return CancellationResponse.FreeTrialExpired;
                            }
                    }

                    if (trialPeriodEndDate.Date < DateTime.Now)
                    {
                        return CancellationResponse.FreeTrialExpired;
                    }
                }


                token.cancellation_status_id = (int)CancellationStatus.InFreeTrial;
                await _tokenRepository.UpdateToken(token);
                return CancellationResponse.SuccessfullyCancelled;

            }
            catch (Exception e)
            {
                _logger.LogError("There was an unexpected erro cancelling token {token} from the cancellation link : {error}", token?.token_id, e.Message);
                return CancellationResponse.UnknownError;
            }
        }

        private string decryptVoucher(string voucherCode, vendor_company vendorCompany)
        {
            return _encryptionService.Decrypt($"{voucherCode}==", vendorCompany.registration_id + vendorCompany.vendorid);
        }
        public async Task<CancellationResponse> CancelVoucherFromVoucherCode(string voucherCode, string vendorRegistration, string vendorAccessCode)
        { 
            try
            {
                _logger.LogInformation("Attempting to cancel voucher for voucher code {voucherCode}", voucherCode);
                // decrypt token 
                var vendorCompany = _vendorCompanyRepository.GetVendorCompanyByRegistration(vendorRegistration);

                if(vendorCompany == null)
                {
                    return CancellationResponse.UnknownVendorRegistration;
                }
                if(vendorCompany.access_secret != vendorAccessCode)
                {
                    return CancellationResponse.UnknownVendorAccessCode;
                }
                string tokenCode;
                try
                {
                    tokenCode = decryptVoucher(voucherCode, vendorCompany);
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "It was not possible to decrypt voucher {voucherCode} for vendor {vendorRegistration} :  {error}", voucherCode, vendorRegistration, e.Message);
                    return CancellationResponse.UnknownVoucherCode;
                }
                
                if(string.IsNullOrWhiteSpace(tokenCode))
                {
                    _logger.LogError("It was not possible to decrypt voucher {voucherCode} for vendor {vendorRegistration}. No token code was returned.", voucherCode, vendorRegistration);
                    return CancellationResponse.UnknownVoucherCode;
                }

                // get token by token code
                var token = _tokenRepository.GetTokenByTokenCode(tokenCode);

                if(token == null)
                {
                    _logger.LogError("It was not possible to retrieve the token for voucher {voucherCode} and vendor {vendorRegistration}. The token code does not exist.", voucherCode, vendorRegistration);
                    return CancellationResponse.UnknownVoucherCode;
                }

                return await CancelToken(token);

            }
            catch(Exception e)
            {
                _logger.LogError("There was an unexpected erro cancelling token {voucherCode} from the voucher code : {error}", voucherCode, e.Message);
                return CancellationResponse.UnknownError;
            }
        }

        public async Task<CancellationResponse> CancelVoucherFromEmailLink(long enterpriseId, string emailAddress)
        {
            token token = null;
            try
            {
                _logger.LogInformation("Attempting to cancel voucher for enterprise {eid}", enterpriseId);
                var enterprise = await _repository.GetEnterprise(enterpriseId);

                if (enterprise is null)
                {
                    return CancellationResponse.EnterpriseNotFound;
                }

                if (!enterprise.applicant_email_address.Trim().Equals(emailAddress.Trim(), StringComparison.CurrentCultureIgnoreCase))
                {
                    _logger.LogInformation("Invalid email address  {email}", emailAddress);
                    return CancellationResponse.InvalidEmail;
                }

                token = await _tokenRepository.GetTokenByEnterpriseId(enterpriseId);
                return await CancelToken(token);

            }
            catch (Exception e)
            {
                _logger.LogError("There was an unexpected erro cancelling token {token} from the cancellation link : {error}", token?.token_id, e.Message);
                return CancellationResponse.UnknownError;
            }
        }
    }
}
