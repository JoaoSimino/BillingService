using BillingService.Domain.DTOs;

namespace BillingService.Domain.Entities;

public static class FaturaExtensions
{
    public static FaturaDtoResponse EntityToDto(this Fatura fatura, FaturaDtoResponse fdr)
    {
        fdr.Status = fatura.Status;
        fdr.NumeroParcelas = fatura.NumeroParcelas;
        fdr.Id = fatura.Id;

        return fdr;

    }
}
