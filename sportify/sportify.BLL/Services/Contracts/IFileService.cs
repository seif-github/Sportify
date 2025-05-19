using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services.Contracts
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
