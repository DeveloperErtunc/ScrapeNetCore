﻿namespace Read_Sahibinden.Service.IService;
public interface  IReadDataService
{
    public Task<List<AdvertisementMinModel>> ReadWebPageData();
}
