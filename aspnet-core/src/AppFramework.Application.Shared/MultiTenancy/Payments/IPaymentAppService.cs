﻿using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AppFramework.MultiTenancy.Dto;
using AppFramework.MultiTenancy.Payments.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppFramework.MultiTenancy.Payments
{
    public interface IPaymentAppService : IApplicationService
    {
        Task<PaymentInfoDto> GetPaymentInfo(PaymentInfoInput input);

        Task<long> CreatePayment(CreatePaymentDto input);

        Task CancelPayment(CancelPaymentDto input);

        Task<PagedResultDto<SubscriptionPaymentListDto>> GetPaymentHistory(GetPaymentHistoryInput input);

        List<PaymentGatewayModel> GetActiveGateways(GetActiveGatewaysInput input);

        Task<SubscriptionPaymentDto> GetPaymentAsync(long paymentId);

        Task<SubscriptionPaymentDto> GetLastCompletedPayment();

        Task BuyNowSucceed(long paymentId);

        Task NewRegistrationSucceed(long paymentId);

        Task UpgradeSucceed(long paymentId);

        Task ExtendSucceed(long paymentId);

        Task PaymentFailed(long paymentId);

        Task SwitchBetweenFreeEditions(int upgradeEditionId);

        Task UpgradeSubscriptionCostsLessThenMinAmount(int editionId);

        Task<bool> HasAnyPayment();
    }
}