using AutoMapper.Internal;
using Microsoft.Extensions.Options;
using salian_api.Config.Extentions;
using salian_api.Dtos.Email;

namespace salian_api.Interface
{
    public interface IMailService
    {
        Task SendEmail(SendMailDto mailRequest);
    }
}
