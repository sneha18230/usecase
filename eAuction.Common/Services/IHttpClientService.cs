﻿using Auction.Commom.Model;
using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IHttpClientService
    {
        Task<Product> ExecuteGet<T>(string url);

        Task<T> ExecutePost<T>(string url, T item);

        Task<bool> ExecuteDelete(string url);
    }
}
